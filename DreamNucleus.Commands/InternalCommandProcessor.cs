using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.ExceptionServices;
using System.Threading.Tasks;
using DreamNucleus.Commands.Builder;
using DreamNucleus.Commands.Notifications;
using DreamNucleus.Commands.Pipelines;
using DreamNucleus.Commands.Results;

namespace DreamNucleus.Commands
{
    internal sealed class InternalCommandProcessor : ICommandProcessor, IDisposable
    {
        private readonly ICommandsBuilder _commandsBuilder;
        private readonly IAsyncCommand _initialCommand;
        private readonly ILifetimeScopeDependencyService _dependencyService;

        public InternalCommandProcessor(ICommandsBuilder commandsBuilder, ILifetimeScopeService lifetimeScopeService, IAsyncCommand initialCommand)
        {
            _commandsBuilder = commandsBuilder;
            _initialCommand = initialCommand;
            _dependencyService = lifetimeScopeService.BeginLifetimeScope(this);
        }

        public async Task<TSuccessResult> ProcessAsync<TCommand, TSuccessResult>(ISuccessResult<TCommand, TSuccessResult> command)
            where TCommand : IAsyncCommand
        {
            TSuccessResult result;
            try
            {
                var executingPipeline = _commandsBuilder.ExecutingPipelines.Reverse() // TODO: always reversing?
                    .Aggregate((ExecutingPipeline)new FinalExecutingPipeline(this),
                        (current, executingPipelineType) => (ExecutingPipeline)_dependencyService.Resolve(executingPipelineType, typeof(ExecutingPipeline), current));

                result = await executingPipeline.ExecutingAsync(command).ConfigureAwait(false);
            }
            catch (Exception exception)
            {
                var exceptionPipeline = _commandsBuilder.ExceptionPipelines.Reverse()
                    .Aggregate((ExceptionPipeline)new FinalExceptionPipeline(),
                        (current, exceptionPipelineType) => (ExceptionPipeline)_dependencyService.Resolve(exceptionPipelineType, typeof(ExceptionPipeline), current));

                var newResult = await exceptionPipeline.ExceptionAsync(command, exception).ConfigureAwait(false);

                if (newResult is Exception newException)
                {
                    ExceptionDispatchInfo.Capture(newException).Throw();
                }

                return (TSuccessResult)newResult; // TODO: I don't think this should go back into the outgoing...
            }

            var executedPipeline = _commandsBuilder.ExecutedPipelines.Reverse()
                .Aggregate((ExecutedPipeline)new FinalExecutedPipeline(),
                    (current, executedPipelineType) => (ExecutedPipeline)_dependencyService.Resolve(executedPipelineType, typeof(ExecutedPipeline), current));

            return await executedPipeline.ExecutedAsync(command, result).ConfigureAwait(false);
        }

        internal async Task<TSuccessResult> ExecuteAsync<TCommand, TSuccessResult>(ISuccessResult<TCommand, TSuccessResult> command)
            where TCommand : IAsyncCommand
        {
            var isInitialCommand = command == _initialCommand;

            var allClassTypes = GetAllConcreteClassTypes(typeof(TCommand));

            // TODO: only create with IAsyncCommand, if none then throw a handler not found exception.. an analyzer could check this
            var firstRegisteredClassType = allClassTypes.First(t => _dependencyService.IsRegistered(typeof(IAsyncCommandHandler<,>).MakeGenericType(t, typeof(TSuccessResult))));

            var handlerType = typeof(IAsyncCommandHandler<,>).MakeGenericType(firstRegisteredClassType, typeof(TSuccessResult));
            var handler = _dependencyService.Resolve(handlerType);

            var pipelines = _commandsBuilder.Pipelines.Select(p => (Pipeline)_dependencyService.Resolve(p)).ToList();

            // run executing pipeline and notification
            foreach (var pipeline in pipelines)
            {
                if (pipeline.GetType().GetTypeInfo().GetCustomAttribute<SingletonAttribute>() == null || isInitialCommand)
                {
                    await pipeline.ExecutingAsync(command).ConfigureAwait(false);
                }
            }

            if (_commandsBuilder.ExecutingNotifications.TryGetValue(typeof(TCommand), out var executingNotifications))
            {
                foreach (var executingNotification in executingNotifications)
                {
                    await ((Task)typeof(IExecutingNotification<>).MakeGenericType(typeof(TCommand))
                        .GetTypeInfo().GetMethod("OnExecutingAsync", new[] { command.GetType() })
                        .Invoke(_dependencyService.Resolve(executingNotification), new object[] { command })).ConfigureAwait(false);
                }
            }

            Task<TSuccessResult> task = null;
            try
            {
                task = (Task<TSuccessResult>)handlerType.GetTypeInfo().GetMethod("ExecuteAsync", new[] { command.GetType() }).Invoke(handler, new object[] { command });

                var result = await task.ConfigureAwait(false);

                if (_commandsBuilder.ExecutedNotifications.TryGetValue(typeof(TCommand), out var executedNotifications))
                {
                    foreach (var executedNotification in executedNotifications)
                    {
                        await ((Task)typeof(IExecutedNotification<,>).MakeGenericType(typeof(TCommand), typeof(TSuccessResult))
                            .GetTypeInfo().GetMethod("OnExecutedAsync", new[] { command.GetType(), typeof(TSuccessResult) })
                            .Invoke(_dependencyService.Resolve(executedNotification), new object[] { command, result })).ConfigureAwait(false);
                    }
                }

                pipelines.Reverse();
                foreach (var pipeline in pipelines)
                {
                    if (pipeline.GetType().GetTypeInfo().GetCustomAttribute<SingletonAttribute>() == null || isInitialCommand)
                    {
                        await pipeline.ExecutedAsync(command, result).ConfigureAwait(false);
                    }
                }
            }
            catch (Exception exception)
            {
                // if the command was not async, we will need to get the inner exception
                if (exception is TargetInvocationException targetInvocationException)
                {
                    exception = targetInvocationException.InnerException;
                }

                // run exception notification and pipeline
                if (_commandsBuilder.ExceptionNotifications.TryGetValue(typeof(TCommand), out var exceptionNotifications))
                {
                    foreach (var exceptionNotification in exceptionNotifications)
                    {
                        await ((Task)typeof(IExceptionNotification<>).MakeGenericType(typeof(TCommand))
                            .GetTypeInfo().GetMethod("OnExecptionAsync", new[] { command.GetType(), typeof(Exception) })
                            .Invoke(_dependencyService.Resolve(exceptionNotification), new object[] { command, exception })).ConfigureAwait(false);
                    }
                }

                pipelines.Reverse();
                foreach (var pipeline in pipelines)
                {
                    if (pipeline.GetType().GetTypeInfo().GetCustomAttribute<SingletonAttribute>() == null || isInitialCommand)
                    {
                        await pipeline.ExceptionAsync(command, exception).ConfigureAwait(false);
                    }
                }

                ExceptionDispatchInfo.Capture(exception).Throw();
            }

            return await task.ConfigureAwait(false);
        }

        public async Task<CommandProcessorSuccessResult<TSuccessResult>> ProcessResultAsync<TCommand, TSuccessResult>(ISuccessResult<TCommand, TSuccessResult> command)
            where TCommand : IAsyncCommand
        {
            try
            {
                return new CommandProcessorSuccessResult<TSuccessResult>(await ProcessAsync(command).ConfigureAwait(false));
            }
            catch (Exception exception)
            {
                return new CommandProcessorSuccessResult<TSuccessResult>(exception);
            }
        }

        private static IEnumerable<Type> GetAllConcreteClassTypes(Type type)
        {
            var types = new List<Type>
            {
                type
            };
            while (type.GetTypeInfo().BaseType != null)
            {
                type = type.GetTypeInfo().BaseType;

                if (!type.GetTypeInfo().IsAbstract)
                {
                    types.Add(type);
                }
            }
            return types;
        }

        public void Dispose()
        {
            _dependencyService.Dispose();
        }
    }
}

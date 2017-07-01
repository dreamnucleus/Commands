using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Slipstream.CommonDotNet.Commands.Builder;
using Slipstream.CommonDotNet.Commands.Notifications;
using Slipstream.CommonDotNet.Commands.Pipelines;
using Slipstream.CommonDotNet.Commands.Results;

namespace Slipstream.CommonDotNet.Commands
{
    internal class InternalCommandProcessor : ICommandProcessor, IDisposable
    {
        public Guid Id { get; } = Guid.NewGuid();

        private readonly ICommandsBuilder commandsBuilder;
        private readonly IAsyncCommand initialCommand;
        private readonly ILifetimeScopeDependencyService dependencyService;

        public InternalCommandProcessor(ICommandsBuilder commandsBuilder, ILifetimeScopeService lifetimeScopeService, IAsyncCommand initialCommand)
        {
            Console.WriteLine("InternalCommandProcessor created " + Id);
            this.commandsBuilder = commandsBuilder;
            this.initialCommand = initialCommand;
            this.dependencyService = lifetimeScopeService.BeginLifetimeScope(this);
        }



        public async Task<TSuccessResult> ProcessAsync<TCommand, TSuccessResult>(ISuccessResult<TCommand, TSuccessResult> command)
            where TCommand : IAsyncCommand
        {
            var isInitialCommand = command == initialCommand;

            var allClassTypes = GetAllConcreteClassTypes(typeof(TCommand));

            // TODO: only create with IAsyncCommand, if none then throw a handler not found exception
            var firstRegisteredClassType = allClassTypes.First(t => dependencyService.IsRegistered(typeof(IAsyncCommandHandler<,>).MakeGenericType(t, typeof(TSuccessResult))));

            var handlerType = typeof(IAsyncCommandHandler<,>).MakeGenericType(firstRegisteredClassType, typeof(TSuccessResult));
            var handler = dependencyService.Resolve(handlerType);

            var pipelines = commandsBuilder.Pipelines.Select(p => (Pipeline)dependencyService.Resolve(p)).ToList();

            // run executing pipeline and notification
            foreach (var pipeline in pipelines)
            {
                if (pipeline.GetType().GetTypeInfo().GetCustomAttribute<SingletonAttribute>() == null || isInitialCommand)
                {
                    await pipeline.ExecutingAsync(command);
                }
            }

            if (commandsBuilder.ExecutingNotifications.TryGetValue(typeof(TCommand), out var executingNotifications))
            {
                foreach (var executingNotification in executingNotifications)
                {
                    typeof(IExecutingNotification<>).MakeGenericType(typeof(TCommand))
                        .GetTypeInfo().GetMethod("OnExecutingAsync", new[] { command.GetType() })
                        .Invoke(dependencyService.Resolve(executingNotification), new object[] { command });
                }
            }

            var task = (Task<TSuccessResult>)handlerType.GetTypeInfo().GetMethod("ExecuteAsync", new[] { command.GetType() }).Invoke(handler, new object[] { command });

            try
            {
                var result = await task;

                if (commandsBuilder.ExecutedNotifications.TryGetValue(typeof(TCommand), out var executedNotifications))
                {
                    foreach (var executedNotification in executedNotifications)
                    {
                        typeof(IExecutedNotification<,>).MakeGenericType(typeof(TCommand), typeof(TSuccessResult))
                            .GetTypeInfo().GetMethod("OnExecutedAsync", new[] { command.GetType(), typeof(TSuccessResult) })
                            .Invoke(dependencyService.Resolve(executedNotification), new object[] { command, result });
                    }
                }

                pipelines.Reverse();
                foreach (var pipeline in pipelines)
                {
                    if (pipeline.GetType().GetTypeInfo().GetCustomAttribute<SingletonAttribute>() == null || isInitialCommand)
                    {
                        await pipeline.ExecutedAsync(command, result);
                    }
                }

                return await task;
            }
            catch (Exception exception)
            {
                // run exception notification and pipeline
                if (commandsBuilder.ExceptionNotifications.TryGetValue(typeof(TCommand), out var exceptionNotifications))
                {
                    foreach (var exceptionNotification in exceptionNotifications)
                    {
                        typeof(IExceptionNotification<>).MakeGenericType(typeof(TCommand))
                            .GetTypeInfo().GetMethod("OnExecptionAsync", new[] { command.GetType(), typeof(Exception) })
                            .Invoke(dependencyService.Resolve(exceptionNotification), new object[] { command, exception });
                    }
                }

                pipelines.Reverse();
                foreach (var pipeline in pipelines)
                {
                    if (pipeline.GetType().GetTypeInfo().GetCustomAttribute<SingletonAttribute>() == null || isInitialCommand)
                    {
                        await pipeline.ExceptionAsync(command, exception);
                    }
                }

                throw;
            }
        }

        public async Task<CommandProcessorSuccessResult<TSuccessResult>> ProcessResultAsync<TCommand, TSuccessResult>(ISuccessResult<TCommand, TSuccessResult> command)
            where TCommand : IAsyncCommand
        {
            try
            {
                return new CommandProcessorSuccessResult<TSuccessResult>(await ProcessAsync(command));
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
            dependencyService.Dispose();
            Console.WriteLine("InternalCommandProcessor disposed " + Id);
        }
    }
}

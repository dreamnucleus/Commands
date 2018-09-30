using System;
using System.Linq;
using Autofac;
using DreamNucleus.Commands.Autofac;
using DreamNucleus.Commands.Results;

namespace DreamNucleus.Commands.Tests.Common
{
    public static class Helpers
    {
        public static ICommandProcessor CreateDefaultCommandProcessor()
        {
            var containerBuilder = new ContainerBuilder();

            containerBuilder.RegisterType<AsyncIntCommandHandler>().As<IAsyncCommandHandler<AsyncIntCommand, int>>();
            containerBuilder.RegisterType<IntCommandHandler>().As<IAsyncCommandHandler<IntCommand, int>>();

            containerBuilder.RegisterType<AsyncExceptionCommandHandler>().As<IAsyncCommandHandler<AsyncExceptionCommand, Unit>>();
            containerBuilder.RegisterType<ExceptionCommandHandler>().As<IAsyncCommandHandler<ExceptionCommand, Unit>>();

            var commandsBuilder = new AutofacCommandsBuilder(containerBuilder);

            commandsBuilder.Use<SingletonPipeline>();
            commandsBuilder.Use<RepeatPipeline>();

            commandsBuilder.Use<IntCommandExecutingNotification>();
            commandsBuilder.Use<IntCommandExecutedNotification>();
            commandsBuilder.Use<ExceptionCommandExceptionNotification>();

            commandsBuilder.Use<ExecutingPipeline>();
            commandsBuilder.Use<ExecutedPipeline>();
            commandsBuilder.Use<ExceptionPipeline>();

            containerBuilder.RegisterInstance(commandsBuilder).SingleInstance();
            containerBuilder.RegisterType<AutofacLifetimeScopeService>().As<ILifetimeScopeService>();
            containerBuilder.RegisterType<CommandProcessor>().As<ICommandProcessor>();

            var container = containerBuilder.Build();

            return container.Resolve<ICommandProcessor>();
        }

        // TODO: don't repeat
        public static ResultProcessor<ObjectResult> CreateDefaultResultProcessor(Action<ResultRegister<ObjectResult>> resultRegisterAction = null)
        {
            var containerBuilder = new ContainerBuilder();

            containerBuilder.RegisterType<AsyncIntCommandHandler>().As<IAsyncCommandHandler<AsyncIntCommand, int>>();
            containerBuilder.RegisterType<IntCommandHandler>().As<IAsyncCommandHandler<IntCommand, int>>();

            containerBuilder.RegisterType<AsyncExceptionCommandHandler>().As<IAsyncCommandHandler<AsyncExceptionCommand, Unit>>();
            containerBuilder.RegisterType<ExceptionCommandHandler>().As<IAsyncCommandHandler<ExceptionCommand, Unit>>();

            var commandsBuilder = new AutofacCommandsBuilder(containerBuilder);

            commandsBuilder.Use<SingletonPipeline>();
            commandsBuilder.Use<RepeatPipeline>();

            commandsBuilder.Use<IntCommandExecutingNotification>();
            commandsBuilder.Use<IntCommandExecutedNotification>();
            commandsBuilder.Use<ExceptionCommandExceptionNotification>();

            containerBuilder.RegisterInstance(commandsBuilder).SingleInstance();
            containerBuilder.RegisterType<AutofacLifetimeScopeService>().As<ILifetimeScopeService>();
            containerBuilder.RegisterType<CommandProcessor>().As<ICommandProcessor>();

            var container = containerBuilder.Build();

            var resultRegister = new ResultRegister<ObjectResult>();
            resultRegisterAction?.Invoke(resultRegister);

            var resultParsers = resultRegister.Emit();
            if (resultParsers.Any())
            {
                return new ResultProcessor<ObjectResult>(resultRegister.Emit(), commandsBuilder,
                    new AutofacLifetimeScopeService(container.BeginLifetimeScope()));
            }
            else
            {
                return new ResultProcessor<ObjectResult>(commandsBuilder,
                    new AutofacLifetimeScopeService(container.BeginLifetimeScope()));
            }
        }
    }
}

using System;
using System.Linq;
using Autofac;
using DreamNucleus.Commands.Autofac;
using DreamNucleus.Commands.Results;

namespace DreamNucleus.Commands.Tests.Common
{
    public static class Helpers
    {
        public static ICommandProcessor CreateDefaultCommandProcessor(
            Action<ContainerBuilder> containerBuilderAction,
            Action<AutofacCommandsBuilder> autofacCommandsBuilderAction)
        {
            var containerBuilder = new ContainerBuilder();
            containerBuilderAction.Invoke(containerBuilder);

            var commandsBuilder = new AutofacCommandsBuilder(containerBuilder);
            autofacCommandsBuilderAction.Invoke(commandsBuilder);

            containerBuilder.RegisterInstance(commandsBuilder).SingleInstance();
            containerBuilder.RegisterType<AutofacLifetimeScopeService>().As<ILifetimeScopeService>();
            containerBuilder.RegisterType<CommandProcessor>().As<ICommandProcessor>();

            var container = containerBuilder.Build();

            return container.Resolve<ICommandProcessor>();
        }

        // TODO: don't repeat
        public static ResultProcessor<ObjectResult> CreateDefaultResultProcessor(
            Action<ContainerBuilder> containerBuilderAction,
            Action<AutofacCommandsBuilder> autofacCommandsBuilderAction,
            Action<ResultRegister<ObjectResult>> resultRegisterAction)
        {
            var containerBuilder = new ContainerBuilder();
            containerBuilderAction.Invoke(containerBuilder);

            var commandsBuilder = new AutofacCommandsBuilder(containerBuilder);
            autofacCommandsBuilderAction.Invoke(commandsBuilder);

            containerBuilder.RegisterInstance(commandsBuilder).SingleInstance();
            containerBuilder.RegisterType<AutofacLifetimeScopeService>().As<ILifetimeScopeService>();
            containerBuilder.RegisterType<CommandProcessor>().As<ICommandProcessor>();

            var container = containerBuilder.Build();

            var resultRegister = new ResultRegister<ObjectResult>();
            resultRegisterAction.Invoke(resultRegister);

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

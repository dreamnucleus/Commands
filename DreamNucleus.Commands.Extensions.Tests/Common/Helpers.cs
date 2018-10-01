using System;
using Autofac;
using DreamNucleus.Commands.Autofac;

namespace DreamNucleus.Commands.Extensions.Tests.Common
{
    public static class Helpers
    {
        public static ICommandProcessor CreateDefaultCommandProcessor(
            Action<ContainerBuilder> containerBuilderAction = null,
            Action<AutofacCommandsBuilder> autofacCommandsBuilderAction = null)
        {
            var containerBuilder = new ContainerBuilder();
            containerBuilderAction?.Invoke(containerBuilder);

            var commandsBuilder = new AutofacCommandsBuilder(containerBuilder);
            autofacCommandsBuilderAction?.Invoke(commandsBuilder);

            containerBuilder.RegisterInstance(commandsBuilder).SingleInstance();
            containerBuilder.RegisterType<AutofacLifetimeScopeService>().As<ILifetimeScopeService>();
            containerBuilder.RegisterType<CommandProcessor>().As<ICommandProcessor>();

            var container = containerBuilder.Build();

            return container.Resolve<ICommandProcessor>();
        }
    }
}

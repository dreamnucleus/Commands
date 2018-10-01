using System;
using Autofac;

namespace DreamNucleus.Commands.Autofac.Tests.Common
{
    public static class Helpers
    {
        public static ILifetimeScopeService CreateLifetimeScopeService(
            Action<ContainerBuilder> containerBuilderAction = null,
            Action<AutofacCommandsBuilder> autofacCommandsBuilderAction = null)
        {
            var containerBuilder = new ContainerBuilder();
            containerBuilderAction?.Invoke(containerBuilder);

            var commandsBuilder = new AutofacCommandsBuilder(containerBuilder);
            autofacCommandsBuilderAction?.Invoke(commandsBuilder);

            containerBuilder.RegisterType<AutofacLifetimeScopeService>().As<ILifetimeScopeService>();

            var container = containerBuilder.Build();

            return container.Resolve<ILifetimeScopeService>();
        }
    }
}

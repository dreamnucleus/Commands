using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

            containerBuilder.RegisterType<AutofacLifetimeScopeService>().As<ILifetimeScopeService>();

            var commandsBuilder = new AutofacCommandsBuilder(containerBuilder);
            autofacCommandsBuilderAction?.Invoke(commandsBuilder);

            var container = containerBuilder.Build();

            return container.Resolve<ILifetimeScopeService>();
        }
    }
}

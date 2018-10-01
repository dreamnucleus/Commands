using Autofac;
using DreamNucleus.Commands.Autofac;
using DreamNucleus.Commands.Extensions.Unique;
using DreamNucleus.Commands.Results;

namespace DreamNucleus.Commands.Extensions.Tests.Common
{
    public static class Helpers
    {
        public static ICommandProcessor CreateDefaultCommandProcessor()
        {
            var containerBuilder = new ContainerBuilder();

            containerBuilder.RegisterType<FakeUniqueManager>().As<IUniqueManager>().SingleInstance();

            containerBuilder.RegisterType<UniqueCommandHandler>().As<IAsyncCommandHandler<UniqueCommand, Unit>>();


            var commandsBuilder = new AutofacCommandsBuilder(containerBuilder);

            commandsBuilder.Use<UniquePipeline>();

            containerBuilder.RegisterInstance(commandsBuilder).SingleInstance();
            containerBuilder.RegisterType<AutofacLifetimeScopeService>().As<ILifetimeScopeService>();
            containerBuilder.RegisterType<CommandProcessor>().As<ICommandProcessor>();

            var container = containerBuilder.Build();

            return container.Resolve<ICommandProcessor>();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

            commandsBuilder.Use<TestPipeline>();
            containerBuilder.RegisterInstance(commandsBuilder).SingleInstance();
            containerBuilder.RegisterType<AutofacLifetimeScopeService>().As<ILifetimeScopeService>();
            containerBuilder.RegisterType<CommandProcessor>().As<ICommandProcessor>();

            var container = containerBuilder.Build();

            return container.Resolve<ICommandProcessor>();
        }
    }
}

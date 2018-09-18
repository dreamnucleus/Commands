using System;
using Autofac;
using Slipstream.CommonDotNet.Commands.Autofac;
using Xunit;

namespace Slipstream.CommonDotNet.Commands.Tests
{
    public class CommandProcessorTests
    {
        private readonly ICommandProcessor _commandProcessor;

        public CommandProcessorTests()
        {
            var containerBuilder = new ContainerBuilder();
            containerBuilder.RegisterType<TestCommandHandler>().As<IAsyncCommandHandler<TestCommand, int>>();

            var commandsBuilder = new AutofacCommandsBuilder(containerBuilder);
            containerBuilder.RegisterInstance(commandsBuilder).SingleInstance();
            containerBuilder.RegisterType<AutofacLifetimeScopeService>().As<ILifetimeScopeService>();
            containerBuilder.RegisterType<CommandProcessor>().As<ICommandProcessor>();
            var container = containerBuilder.Build();

            _commandProcessor = container.Resolve<ICommandProcessor>();
        }

        [Fact]
        public void Test()
        {
            var input = 2;
            var result = _commandProcessor.ProcessAsync(new TestCommand(input)).Result;
            Assert.Equal(input, result);
        }
    }
}

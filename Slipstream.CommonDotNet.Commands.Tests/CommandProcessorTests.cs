using System;
using System.Threading.Tasks;
using Autofac;
using Slipstream.CommonDotNet.Commands.Autofac;
using Slipstream.CommonDotNet.Commands.Results;
using Xunit;

namespace Slipstream.CommonDotNet.Commands.Tests
{
    public class CommandProcessorTests
    {
        private readonly ICommandProcessor _commandProcessor;

        public CommandProcessorTests()
        {
            var containerBuilder = new ContainerBuilder();

            containerBuilder.RegisterType<IntCommandHandler>().As<IAsyncCommandHandler<IntCommand, int>>();
            containerBuilder.RegisterType<ExceptionCommandHandler>().As<IAsyncCommandHandler<ExceptionCommand, Unit>>();

            var commandsBuilder = new AutofacCommandsBuilder(containerBuilder);

            commandsBuilder.Use<TestPipeline>();

            containerBuilder.RegisterInstance(commandsBuilder).SingleInstance();
            containerBuilder.RegisterType<AutofacLifetimeScopeService>().As<ILifetimeScopeService>();
            containerBuilder.RegisterType<CommandProcessor>().As<ICommandProcessor>();

            var container = containerBuilder.Build();

            _commandProcessor = container.Resolve<ICommandProcessor>();
        }

        [Fact]
        public void IntReturn()
        {
            var input = 2;
            var result = _commandProcessor.ProcessAsync(new IntCommand(input)).Result;
            Assert.Equal(input, result);
        }

        [Fact]
        public async Task Exception()
        {
            await Assert.ThrowsAsync<Exception>(async () => await _commandProcessor.ProcessAsync(new ExceptionCommand()));
        }
    }
}

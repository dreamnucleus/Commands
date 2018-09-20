using System;
using System.Threading.Tasks;
using Autofac;
using Slipstream.CommonDotNet.Commands.Autofac;
using Xunit;

namespace Slipstream.CommonDotNet.Commands.Extensions.Tests
{
    public class CommandProcessorTests
    {
        private readonly ICommandProcessor _commandProcessor;

        public CommandProcessorTests()
        {
            var containerBuilder = new ContainerBuilder();

            containerBuilder.RegisterType<MockLockManager>().As<ILockManager>().SingleInstance();

            containerBuilder.RegisterType<SemaphoreCommandHandler>().As<IAsyncCommandHandler<SemaphoreCommand, int>>();

            var commandsBuilder = new AutofacCommandsBuilder(containerBuilder);

            commandsBuilder.Use<SemaphorePipeline>();

            containerBuilder.RegisterInstance(commandsBuilder).SingleInstance();
            containerBuilder.RegisterType<AutofacLifetimeScopeService>().As<ILifetimeScopeService>();
            containerBuilder.RegisterType<CommandProcessor>().As<ICommandProcessor>();

            var container = containerBuilder.Build();

            _commandProcessor = container.Resolve<ICommandProcessor>();
        }

        [Fact]
        public async Task Semaphore()
        {
            const int input = 2;
            await Assert.ThrowsAsync<Exception>(async () => await Task.WhenAll(_commandProcessor.ProcessAsync(new SemaphoreCommand(input)), _commandProcessor.ProcessAsync(new SemaphoreCommand(input))));
        }
    }
}

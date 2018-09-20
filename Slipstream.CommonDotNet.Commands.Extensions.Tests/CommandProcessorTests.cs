using System;
using System.Threading.Tasks;
using Autofac;
using Slipstream.CommonDotNet.Commands.Autofac;
using Slipstream.CommonDotNet.Commands.Results;
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

            containerBuilder.RegisterType<SemaphoreCommandHandler>().As<IAsyncCommandHandler<SemaphoreCommand, Unit>>();

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
            var taskCompletionSource = new TaskCompletionSource<Unit>();

            Func<Task> waitFunc = async () => await taskCompletionSource.Task;
            Func<Task> setFunc = () =>
            {
                taskCompletionSource.SetResult(Unit.Value);
                return Task.CompletedTask;
            };

            await Assert.ThrowsAsync<Exception>(async () =>
            {
                var waitTask = _commandProcessor.ProcessAsync(new SemaphoreCommand(waitFunc));
                var setTask = _commandProcessor.ProcessAsync(new SemaphoreCommand(setFunc));

                await Task.WhenAny(waitTask, setTask);
                await setFunc();
                await waitTask;
                await setTask;
            });
        }
    }
}

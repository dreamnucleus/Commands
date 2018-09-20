using System;
using System.Threading;
using System.Threading.Tasks;
using Autofac;
using Slipstream.CommonDotNet.Commands.Autofac;
using Slipstream.CommonDotNet.Commands.Extensions.Results;
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

            async Task Wait() => await taskCompletionSource.Task;

            var count = 0;

            Task Set()
            {
                Interlocked.Increment(ref count);
                Assert.Equal(1, count); // ensure we only ever got here once
                taskCompletionSource.SetResult(Unit.Value);
                return Task.CompletedTask;
            }

            await Assert.ThrowsAsync<ConflictException>(async () =>
            {
                var waitTask = _commandProcessor.ProcessAsync(new SemaphoreCommand(Wait));
                var setTask = _commandProcessor.ProcessAsync(new SemaphoreCommand(Set));

                await Task.WhenAny(waitTask, setTask);
                await Set();
                await waitTask;
                Assert.True(waitTask.IsCompleted && !waitTask.IsFaulted);
                await setTask;
            });
        }

        [Fact]
        public async Task Semaphore1()
        {
            await Assert.ThrowsAsync<ConflictException>(async () =>
            {
                await _commandProcessor.ProcessAsync(new SemaphoreCommand(async () =>
                    {
                        await _commandProcessor.ProcessAsync(new SemaphoreCommand(async () => await Task.Delay(-1)));
                    }));
            });
        }

        [Fact]
        public async Task Semaphore2()
        {
            var count = 0;

            Task Increment()
            {
                Interlocked.Increment(ref count);
                return Task.CompletedTask;
            }

            await _commandProcessor.ProcessAsync(new SemaphoreCommand(Increment)).ContinueWith(
                async _ =>
                {
                    await _commandProcessor.ProcessAsync(new SemaphoreCommand(Increment));
                });

            Assert.Equal(2, count);
        }

        // TODO: test renew
    }
}

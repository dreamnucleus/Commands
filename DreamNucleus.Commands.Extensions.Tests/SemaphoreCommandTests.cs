using System.Threading;
using System.Threading.Tasks;
using DreamNucleus.Commands.Extensions.Results;
using DreamNucleus.Commands.Extensions.Tests.Common;
using DreamNucleus.Commands.Results;
using Xunit;

// TODO: REMOVE
namespace DreamNucleus.Commands.Extensions.Tests
{
    public class SemaphoreCommandTests
    {
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
                var commandProcessor = Helpers.CreateDefaultCommandProcessor();

                var waitTask = commandProcessor.ProcessAsync(new SemaphoreCommand(Wait));
                var setTask = commandProcessor.ProcessAsync(new SemaphoreCommand(Set));

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
            var commandProcessor = Helpers.CreateDefaultCommandProcessor();

            await Assert.ThrowsAsync<ConflictException>(async () =>
            {
                await commandProcessor.ProcessAsync(new SemaphoreCommand(async () =>
                    {
                        await commandProcessor.ProcessAsync(new SemaphoreCommand(async () => await Task.Delay(-1)));
                    }));
            });
        }

        [Fact]
        public async Task Semaphore2()
        {
            var commandProcessor = Helpers.CreateDefaultCommandProcessor();

            var count = 0;

            Task Increment()
            {
                Interlocked.Increment(ref count);
                return Task.CompletedTask;
            }

            await commandProcessor.ProcessAsync(new SemaphoreCommand(Increment)).ContinueWith(
                async _ =>
                {
                    await commandProcessor.ProcessAsync(new SemaphoreCommand(Increment));
                });

            Assert.Equal(2, count);
        }

        // TODO: test renew

        [Fact]
        public async Task SemaphoreHash()
        {
            var commandProcessor = Helpers.CreateDefaultCommandProcessor();

            const int id1 = 1;
            const int id2 = 2;

            var result1 = await commandProcessor.ProcessAsync(new SemaphoreHashCommand(id1, async () =>
            {
                var result2 = await commandProcessor.ProcessAsync(new SemaphoreHashCommand(id2, async () => await Task.Delay(1)));
                Assert.Equal(id2, result2);
            }));

            Assert.Equal(id1, result1);
        }

        [Fact]
        public async Task SemaphoreHash1()
        {
            var commandProcessor = Helpers.CreateDefaultCommandProcessor();

            const int id = 1;

            await Assert.ThrowsAsync<ConflictException>(async () =>
            {
                await commandProcessor.ProcessAsync(new SemaphoreHashCommand(id, async () =>
                {
                    await commandProcessor.ProcessAsync(new SemaphoreHashCommand(id, async () => await Task.Delay(-1)));
                }));
            });
        }
    }
}

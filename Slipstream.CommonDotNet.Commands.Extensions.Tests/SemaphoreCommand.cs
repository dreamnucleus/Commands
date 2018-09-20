using System;
using System.Threading.Tasks;
using Slipstream.CommonDotNet.Commands.Results;

namespace Slipstream.CommonDotNet.Commands.Extensions.Tests
{
    [Semaphore]
    public class SemaphoreCommand : ISuccessResult<SemaphoreCommand, Unit>
    {
        public Func<Task> Func { get; }

        public SemaphoreCommand(Func<Task> func)
        {
            Func = func;
        }
    }

    public class SemaphoreCommandHandler : IAsyncCommandHandler<SemaphoreCommand, Unit>
    {
        public async Task<Unit> ExecuteAsync(SemaphoreCommand command)
        {
            await command.Func();
            return Unit.Value;
        }
    }
}

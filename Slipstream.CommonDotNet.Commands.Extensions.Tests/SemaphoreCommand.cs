using System.Threading.Tasks;
using Slipstream.CommonDotNet.Commands.Results;

namespace Slipstream.CommonDotNet.Commands.Extensions.Tests
{
    [Semaphore]
    public class SemaphoreCommand : ISuccessResult<SemaphoreCommand, int>
    {
        public int Input { get; }

        public SemaphoreCommand(int input)
        {
            Input = input;
        }
    }

    public class SemaphoreCommandHandler : IAsyncCommandHandler<SemaphoreCommand, int>
    {
        public async Task<int> ExecuteAsync(SemaphoreCommand command)
        {
            await Task.Delay(20);
            return command.Input;
        }
    }
}

using System.Threading.Tasks;
using DreamNucleus.Commands.Results;

namespace DreamNucleus.Commands.Tests.Common
{
    public class IntCommand : ISuccessResult<IntCommand, int>
    {
        public int Input { get; }

        public IntCommand(int input)
        {
            Input = input;
        }
    }

    public class IntCommandHandler : IAsyncCommandHandler<IntCommand, int>
    {
        public Task<int> ExecuteAsync(IntCommand command)
        {
            return Task.FromResult(command.Input);
        }
    }
}

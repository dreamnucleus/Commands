using System.Threading.Tasks;
using DreamNucleus.Commands.Results;

namespace DreamNucleus.Commands.Tests.Common
{
    public class CombinedCommand : ISuccessResult<CombinedCommand, int>
    {
        public int Input { get; }

        public CombinedCommand(int input)
        {
            Input = input;
        }
    }

    public class CombinedCommandHandler : IAsyncCommandHandler<CombinedCommand, int>
    {
        private readonly ICommandProcessor _commandProcessor;

        public CombinedCommandHandler(ICommandProcessor commandProcessor)
        {
            _commandProcessor = commandProcessor;
        }

        public async Task<int> ExecuteAsync(CombinedCommand command)
        {
            var result1 = await _commandProcessor.ProcessAsync(new IntCommand(command.Input)).ConfigureAwait(false);

            var result2 = await _commandProcessor.ProcessAsync(new AsyncIntCommand(command.Input)).ConfigureAwait(false);

            return result1 + result2;
        }
    }
}

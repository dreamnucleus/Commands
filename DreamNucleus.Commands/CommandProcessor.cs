using System.Threading.Tasks;
using DreamNucleus.Commands.Builder;
using DreamNucleus.Commands.Results;

namespace DreamNucleus.Commands
{
    public class CommandProcessor : ICommandProcessor
    {
        private readonly ICommandsBuilder _commandsBuilder;
        private readonly ILifetimeScopeService _lifetimeScopeService;

        public CommandProcessor(ICommandsBuilder commandsBuilder, ILifetimeScopeService lifetimeScopeService)
        {
            _commandsBuilder = commandsBuilder;
            _lifetimeScopeService = lifetimeScopeService;
        }

        public async Task<TSuccessResult> ProcessAsync<TCommand, TSuccessResult>(ISuccessResult<TCommand, TSuccessResult> command)
            where TCommand : IAsyncCommand
        {
            using (var internalCommandProcessor = new InternalCommandProcessor(_commandsBuilder, _lifetimeScopeService, command))
            {
                return await internalCommandProcessor.ProcessAsync(command).ConfigureAwait(false);
            }
        }

        public async Task<CommandProcessorSuccessResult<TSuccessResult>> ProcessResultAsync<TCommand, TSuccessResult>(ISuccessResult<TCommand, TSuccessResult> command)
            where TCommand : IAsyncCommand
        {
            using (var internalCommandProcessor = new InternalCommandProcessor(_commandsBuilder, _lifetimeScopeService, command))
            {
                return await internalCommandProcessor.ProcessResultAsync(command).ConfigureAwait(false);
            }
        }
    }
}

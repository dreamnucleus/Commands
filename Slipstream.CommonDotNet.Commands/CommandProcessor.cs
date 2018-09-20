using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Slipstream.CommonDotNet.Commands.Builder;
using Slipstream.CommonDotNet.Commands.Notifications;
using Slipstream.CommonDotNet.Commands.Pipelines;
using Slipstream.CommonDotNet.Commands.Results;

namespace Slipstream.CommonDotNet.Commands
{
    public class CommandProcessor : ICommandProcessor
    {
        private readonly ICommandsBuilder _commandsBuilder;
        private readonly ILifetimeScopeService _lifetimeScopeService;

        public CommandProcessor(ICommandsBuilder commandsBuilder, ILifetimeScopeService lifetimeScopeService)
        {
            this._commandsBuilder = commandsBuilder;
            this._lifetimeScopeService = lifetimeScopeService;
        }

        public async Task<TSuccessResult> ProcessAsync<TCommand, TSuccessResult>(ISuccessResult<TCommand, TSuccessResult> command)
            where TCommand : IAsyncCommand
        {
            using (var internalCommandProcessor =
                new InternalCommandProcessor(_commandsBuilder, _lifetimeScopeService, command))
            {
                return await internalCommandProcessor.ProcessAsync(command);
            }
        }

        public async Task<CommandProcessorSuccessResult<TSuccessResult>> ProcessResultAsync<TCommand, TSuccessResult>(ISuccessResult<TCommand, TSuccessResult> command)
            where TCommand : IAsyncCommand
        {
            using (var internalCommandProcessor =
                new InternalCommandProcessor(_commandsBuilder, _lifetimeScopeService, command))
            {
                return await internalCommandProcessor.ProcessResultAsync(command);
            }
        }
    }
}

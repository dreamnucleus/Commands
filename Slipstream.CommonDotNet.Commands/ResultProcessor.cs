using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Slipstream.CommonDotNet.Commands.Builder;
using Slipstream.CommonDotNet.Commands.Results;

namespace Slipstream.CommonDotNet.Commands
{
    public class ResultProcessor<TReturn>
    {
        private readonly IReadOnlyDictionary<Type, Func<object, TReturn>> resultParsers;

        private readonly ICommandProcessor commandProcessor;

        public ResultProcessor(ICommandsBuilder commandsBuilder, ILifetimeScopeService lifetimeScopeService)
            : this(new Dictionary<Type, Func<object, TReturn>>(), commandsBuilder, lifetimeScopeService)
        {
        }

        public ResultProcessor(Dictionary<Type, Func<object, TReturn>> resultParsers, 
            ICommandsBuilder commandsBuilder, ILifetimeScopeService lifetimeScopeService)
        {
            this.resultParsers = resultParsers;
            this.commandProcessor = new CommandProcessor(commandsBuilder, lifetimeScopeService);
        }

        // can i make one which only returns the success
        public ResultRegisterProcessor<TCommand, TSuccessResult, TReturn> For<TCommand, TSuccessResult>(ISuccessResult<TCommand, TSuccessResult> command)
            where TCommand : IAsyncCommand
        {
            return new ResultRegisterProcessor<TCommand, TSuccessResult, TReturn>
                (command, resultParsers, commandProcessor);
        }
    }
}

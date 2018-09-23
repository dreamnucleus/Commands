using System;
using System.Collections.Generic;
using DreamNucleus.Commands.Builder;
using DreamNucleus.Commands.Results;

namespace DreamNucleus.Commands
{
    public class ResultProcessor<TReturn>
    {
        private readonly IReadOnlyDictionary<Type, Func<object, TReturn>> _resultParsers;

        private readonly ICommandProcessor _commandProcessor;

        public ResultProcessor(ICommandsBuilder commandsBuilder, ILifetimeScopeService lifetimeScopeService)
            : this(new Dictionary<Type, Func<object, TReturn>>(), commandsBuilder, lifetimeScopeService)
        {
        }

        public ResultProcessor(Dictionary<Type, Func<object, TReturn>> resultParsers, 
            ICommandsBuilder commandsBuilder, ILifetimeScopeService lifetimeScopeService)
        {
            _resultParsers = resultParsers;
            _commandProcessor = new CommandProcessor(commandsBuilder, lifetimeScopeService);
        }

        // can i make one which only returns the success
        public ResultRegisterProcessor<TCommand, TSuccessResult, TReturn> For<TCommand, TSuccessResult>(ISuccessResult<TCommand, TSuccessResult> command)
            where TCommand : IAsyncCommand
        {
            return new ResultRegisterProcessor<TCommand, TSuccessResult, TReturn>
                (command, _resultParsers, _commandProcessor);
        }
    }
}

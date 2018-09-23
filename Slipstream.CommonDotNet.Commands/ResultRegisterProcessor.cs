using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Threading.Tasks;
using Slipstream.CommonDotNet.Commands.Builder;
using Slipstream.CommonDotNet.Commands.Results;

namespace Slipstream.CommonDotNet.Commands
{
    public class ResultRegisterProcessor<TCommand, TSuccessResult, TReturn>
        where TCommand : IAsyncCommand
    {
        private readonly ISuccessResult<TCommand, TSuccessResult> _command;

        private readonly IReadOnlyDictionary<Type, Func<object, TReturn>> _defaultResultParsers;
        private readonly Dictionary<Type, Func<object, TReturn>> _resultParsers = new Dictionary<Type, Func<object, TReturn>>();


        private readonly ICommandProcessor _commandProcessor;

        public ResultRegisterProcessor(ISuccessResult<TCommand, TSuccessResult> command, IReadOnlyDictionary<Type, Func<object, TReturn>> defaultResultParsers,
            ICommandProcessor commandProcessor)
        {
            Contract.Requires(command != null);
            Contract.Requires(defaultResultParsers != null);
            Contract.Requires(commandProcessor != null);

            _command = command;
            _defaultResultParsers = defaultResultParsers;
            _commandProcessor = commandProcessor;
        }


        public ResultParser<TCommand, TSuccessResult, TReturn, TWhen> When<TWhen>(Func<TCommand, TWhen> action)
        {
            return new ResultParser<TCommand, TSuccessResult, TReturn, TWhen>(this, func =>
            {
                _resultParsers.Add(typeof(TWhen), func);
            });
        }

        public ResultParser<TCommand, TSuccessResult, TReturn, TException> Catch<TException>()
            where TException : Exception
        {
            return new ResultParser<TCommand, TSuccessResult, TReturn, TException>(this, func =>
            {
                _resultParsers.Add(typeof(TException), func);
            });
        }

        // TODO: this should be done in another class
        public async Task<TReturn> ExecuteAsync()
        {
            // TODO: should this be reused?
            try
            {
                return ProcessResult(await _commandProcessor.ProcessAsync(_command));
            }
            catch (Exception exception)
            {
                return ProcessResult(exception);
            }
        }

        // TODO: can we do an exception processor
        // TODO: if exception is not found maybe we should throw that exception? makes more sense
        private TReturn ProcessResult(object result)
        {
            if (_resultParsers.ContainsKey(result.GetType()))
            {
                return _resultParsers[result.GetType()](result);
            }
            else if (_defaultResultParsers.ContainsKey(result.GetType()))
            {
                return _defaultResultParsers[result.GetType()](result);
            }
            else if (result is Exception exception)
            {
                throw exception;
            }
            else
            {
                throw new ResultNotRegisteredException(_command.GetType(), result.GetType());
            }
        }

        public async Task<TSuccessResult> ExecuteSuccessAsync()
        {
            return await _commandProcessor.ProcessAsync(_command);
        }
    }
}

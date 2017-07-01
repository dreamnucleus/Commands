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

        private readonly ISuccessResult<TCommand, TSuccessResult> command;

        private readonly IReadOnlyDictionary<Type, Func<object, TReturn>> defualtResultParsers;
        private readonly Dictionary<Type, Func<object, TReturn>> resultParsers = new Dictionary<Type, Func<object, TReturn>>();


        private readonly ICommandProcessor commandProcessor;

        public ResultRegisterProcessor(ISuccessResult<TCommand, TSuccessResult> command, IReadOnlyDictionary<Type, Func<object, TReturn>> defualtResultParsers,
            ICommandProcessor commandProcessor)
        {
            Contract.Requires(command != null);
            Contract.Requires(defualtResultParsers != null);
            Contract.Requires(commandProcessor != null);

            this.command = command;
            this.defualtResultParsers = defualtResultParsers;
            this.commandProcessor = commandProcessor;
        }


        public ResultParser<TCommand, TSuccessResult, TReturn, TWhen> When<TWhen>(Func<TCommand, TWhen> action)
        {
            return new ResultParser<TCommand, TSuccessResult, TReturn, TWhen>(this, func =>
            {
                resultParsers.Add(typeof(TWhen), func);
            });
        }

        // TODO: could add in Catch<TException>.Return(e => e...)
        //public ResultParser<TCommand, TSuccessResult, TReturn, TE> Catch<TWhen>(Func<TCommand, TWhen> action)
        //    where TWhen : Exception
        //{
        //    return new ResultParser<TCommand, TSuccessResult, TReturn, TWhen>(this, func =>
        //    {
        //        resultParsers.Add(typeof(TWhen), func);
        //    });
        //}

        // TODO: this should be done in another class
        public async Task<TReturn> ExecuteAsync()
        {
            // TODO: should this be reused?
            try
            {
                return ProcessResult(await commandProcessor.ProcessAsync(command));
            }
            catch (Exception exception)
            {
                return ProcessResult(exception);
            }
        }

        // TODO: can we do an exception processor
        private TReturn ProcessResult(object result)
        {
            if (resultParsers.ContainsKey(result.GetType()))
            {
                return resultParsers[result.GetType()](result);
            }
            else if (defualtResultParsers.ContainsKey(result.GetType()))
            {
                return defualtResultParsers[result.GetType()](result);
            }
            else
            {
                // TODO: not registered exception
                throw new ResultNotRegisteredException(command.GetType(), result.GetType());
            }
        }

        public async Task<TSuccessResult> ExecuteSuccessAsync()
        {
            return await commandProcessor.ProcessAsync(command);
        }
    }
}

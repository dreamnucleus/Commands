using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Slipstream.CommonDotNet.Commands.Results;

namespace Slipstream.CommonDotNet.Commands
{
    public class ResultParser<TCommand, TSuccessResult, TReturn, TWhen>
        where TCommand : IAsyncCommand
    {
        private readonly ResultRegisterProcessor<TCommand, TSuccessResult, TReturn> resultRegisterProcessor;
        private readonly Action<Func<object, TReturn>> registerResult;

        public ResultParser(ResultRegisterProcessor<TCommand, TSuccessResult, TReturn> resultRegisterProcessor, Action<Func<object, TReturn>> registerResult)
        {
            this.resultRegisterProcessor = resultRegisterProcessor;
            this.registerResult = registerResult;
        }

        public ResultRegisterProcessor<TCommand, TSuccessResult, TReturn> Return(Func<TWhen, TReturn> resultParser)
        {
            registerResult(result => resultParser((TWhen) result));
            return resultRegisterProcessor;
        }
    }

    public class ResultParser<TReturn, TWhen>
    {
        private readonly ResultRegister<TReturn> resultRegisterProcessor;
        private readonly Action<Func<object, TReturn>> registerResult;

        public ResultParser(ResultRegister<TReturn> resultRegisterProcessor, Action<Func<object, TReturn>> registerResult)
        {
            this.resultRegisterProcessor = resultRegisterProcessor;
            this.registerResult = registerResult;
        }

        public ResultRegister<TReturn> Return(Func<TWhen, TReturn> resultParser)
        {
            registerResult(result => resultParser((TWhen)result));
            return resultRegisterProcessor;
        }
    }
}

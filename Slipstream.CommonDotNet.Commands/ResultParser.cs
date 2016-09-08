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
        where TSuccessResult : IResult
        where TWhen : IResult
    {
        private readonly ResultRegister<TCommand, TSuccessResult, TReturn> resultRegister;
        private readonly Action<Func<IResult, TReturn>> registerResult;

        public ResultParser(ResultRegister<TCommand, TSuccessResult, TReturn> resultRegister, Action<Func<IResult, TReturn>> registerResult)
        {
            this.resultRegister = resultRegister;
            this.registerResult = registerResult;
        }


        public ResultRegister<TCommand, TSuccessResult, TReturn> Return(Func<TWhen, TReturn> resultParser)
        {
            registerResult(result => resultParser((TWhen) result));
            return resultRegister;
        }
    }
}

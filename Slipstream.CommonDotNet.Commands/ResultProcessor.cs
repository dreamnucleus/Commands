using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Slipstream.CommonDotNet.Commands.Results;

namespace Slipstream.CommonDotNet.Commands
{
    public class ResultProcessor<TReturn> : IDisposable
    {
        private readonly IReadOnlyDictionary<Type, Func<IResult, TReturn>> resultParsers;
        private readonly ILifetimeScopeService lifetimeScopeService;

        public ResultProcessor(ILifetimeScopeService lifetimeScopeService)
            : this(new Dictionary<Type, Func<IResult, TReturn>>(), lifetimeScopeService)
        {
        }

        public ResultProcessor(Dictionary<Type, Func<IResult, TReturn>> resultParsers, ILifetimeScopeService lifetimeScopeService)
        {
            this.resultParsers = resultParsers;
            this.lifetimeScopeService = lifetimeScopeService;
        }

        // can i make one which only returns the success
        public ResultRegisterProcessor<TCommand, TSuccessResult, TReturn> For<TCommand, TSuccessResult>(ISuccessResult<TCommand, TSuccessResult> command)
            where TCommand : IAsyncCommand
            where TSuccessResult : IResult
        {
            return new ResultRegisterProcessor<TCommand, TSuccessResult, TReturn>(command, resultParsers, lifetimeScopeService);
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}

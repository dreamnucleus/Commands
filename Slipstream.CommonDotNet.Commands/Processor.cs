using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Slipstream.CommonDotNet.Commands.Results;

namespace Slipstream.CommonDotNet.Commands
{
    public class Processor<TReturn> : IDisposable
    {
        private readonly ILifetimeScopeService lifetimeScopeService;

        public Processor(ILifetimeScopeService lifetimeScopeService)
        {
            this.lifetimeScopeService = lifetimeScopeService;
        }

        // can i make one which only returns the success
        public ResultRegister<TCommand, TSuccessResult, TReturn> For<TCommand, TSuccessResult>(ISuccessResult<TCommand, TSuccessResult> command)
            where TCommand : IAsyncCommand
            where TSuccessResult : IResult
        {
            return new ResultRegister<TCommand, TSuccessResult, TReturn>(command, lifetimeScopeService);
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}

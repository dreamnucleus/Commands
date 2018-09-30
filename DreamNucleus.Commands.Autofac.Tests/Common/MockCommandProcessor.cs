using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DreamNucleus.Commands.Results;

namespace DreamNucleus.Commands.Autofac.Tests.Common
{
    public class MockCommandProcessor : ICommandProcessor
    {
        public Task<TSuccessResult> ProcessAsync<TCommand, TSuccessResult>(ISuccessResult<TCommand, TSuccessResult> command) where TCommand : IAsyncCommand
        {
            throw new NotImplementedException();
        }

        public Task<CommandProcessorSuccessResult<TSuccessResult>> ProcessResultAsync<TCommand, TSuccessResult>(ISuccessResult<TCommand, TSuccessResult> command) where TCommand : IAsyncCommand
        {
            throw new NotImplementedException();
        }
    }
}

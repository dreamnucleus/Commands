using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Slipstream.CommonDotNet.Commands.Results;

namespace Slipstream.CommonDotNet.Commands
{
    // TODO: IDisposable
    public interface ICommandProcessor : IDisposable
    {
        //Task ProcessAsync(IAsyncCommand command);

        Task<IResult> ProcessAsync<TCommand, TSuccessResult>(ISuccessResult<TCommand, TSuccessResult> command)
            where TCommand : IAsyncCommand
            where TSuccessResult : IResult;

        Task<CommandProcessorSuccessResult<TSuccessResult>> ProcessSuccessAsync<TCommand, TSuccessResult>(ISuccessResult<TCommand, TSuccessResult> command)
            where TCommand : IAsyncCommand
            where TSuccessResult : IResult;
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Slipstream.CommonDotNet.Commands.Results;

namespace Slipstream.CommonDotNet.Commands
{
    public interface IAsyncCommandHandler<in TCommand, TSuccessResult>
        where TCommand : ISuccessResult<TCommand, TSuccessResult>
    {
        Task<TSuccessResult> ExecuteAsync(TCommand command);
    }

}

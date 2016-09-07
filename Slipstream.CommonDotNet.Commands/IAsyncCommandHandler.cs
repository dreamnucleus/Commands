using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slipstream.CommonDotNet.Commands
{
    public interface IAsyncCommandHandler<in TCommand>
        where TCommand : IAsyncCommand
    {
        Task<IResult> ExecuteAsync(TCommand command);
    }

}

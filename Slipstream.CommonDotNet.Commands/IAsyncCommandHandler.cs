using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Slipstream.CommonDotNet.Commands.Results;

namespace Slipstream.CommonDotNet.Commands
{
    public interface IAsyncCommandHandler<in TCommand>
        where TCommand : IAsyncCommand
    {
        Task<object> ExecuteAsync(TCommand command);
    }

}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slipstream.CommonDotNet.Commands.Notifications
{
    public interface IExceptionNotification<in TCommand> : IUseCommandsBuilder
    {
        Task OnExecptionAsync(TCommand command, Exception exception);
    }
}

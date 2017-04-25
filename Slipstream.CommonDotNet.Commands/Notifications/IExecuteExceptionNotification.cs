using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slipstream.CommonDotNet.Commands.Notifications
{
    public interface IExecuteExceptionNotification<in TCommand>
    {
        Task OnExecutedAsync(TCommand command, Exception exception);

    }
}

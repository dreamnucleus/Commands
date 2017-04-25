using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Slipstream.CommonDotNet.Commands.Results;

namespace Slipstream.CommonDotNet.Commands.Notifications
{
    public interface IExecutedNotification<in TCommand, in TResult>
        where TCommand : ISuccessResult<TCommand, TResult>
    {
        Task OnExecutedAsync(TCommand command, TResult result);
    }
}

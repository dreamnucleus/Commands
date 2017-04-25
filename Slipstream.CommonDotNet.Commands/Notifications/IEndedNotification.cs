using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Slipstream.CommonDotNet.Commands.Results;

namespace Slipstream.CommonDotNet.Commands.Notifications
{
    public interface IEndedNotification<in TCommand, in TResult>
        where TCommand : ISuccessResult<TCommand, TResult>
    {
        Task OnEndedAsync(TCommand command, TResult result);

    }
}

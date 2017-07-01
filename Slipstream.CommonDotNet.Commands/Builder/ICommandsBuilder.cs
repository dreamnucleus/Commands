using Slipstream.CommonDotNet.Commands.Pipelines;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Slipstream.CommonDotNet.Commands.Notifications;

namespace Slipstream.CommonDotNet.Commands.Builder
{
    // TODO: put 
    public interface ICommandsBuilder
    {
        IReadOnlyCollection<Type> Pipelines { get; }

        IReadOnlyDictionary<Type, IReadOnlyCollection<Type>> ExecutingNotifications { get; }
        IReadOnlyDictionary<Type, IReadOnlyCollection<Type>> ExecutedNotifications { get; }
        IReadOnlyDictionary<Type, IReadOnlyCollection<Type>> ExceptionNotifications { get; }


        ICommandsBuilder Use<TItem>()
            where TItem : IUseCommandsBuilder;


    }
}

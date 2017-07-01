using Slipstream.CommonDotNet.Commands.Pipelines;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Slipstream.CommonDotNet.Commands.Notifications;


namespace Slipstream.CommonDotNet.Commands.Builder
{
    public class CommandsBuilder : ICommandsBuilder
    {
        private readonly List<Type> piplines = new List<Type>();
        private readonly Dictionary<Type, IReadOnlyCollection<Type>> executingNotifications = new Dictionary<Type, IReadOnlyCollection<Type>>();
        private readonly Dictionary<Type, IReadOnlyCollection<Type>> executedNotifications = new Dictionary<Type, IReadOnlyCollection<Type>>();
        private readonly Dictionary<Type, IReadOnlyCollection<Type>> exceptionNotifications = new Dictionary<Type, IReadOnlyCollection<Type>>();

        public IReadOnlyCollection<Type> Pipelines => piplines;
        public IReadOnlyDictionary<Type, IReadOnlyCollection<Type>> ExecutingNotifications => executingNotifications;
        public IReadOnlyDictionary<Type, IReadOnlyCollection<Type>> ExecutedNotifications => executedNotifications;
        public IReadOnlyDictionary<Type, IReadOnlyCollection<Type>> ExceptionNotifications => exceptionNotifications;

        public virtual ICommandsBuilder Use<TItem>()
            where TItem : IUseCommandsBuilder
        {
            if (typeof(TItem).GetTypeInfo().IsSubclassOf(typeof(Pipeline)))
            {
                piplines.Add(typeof(TItem));
            }
            // TODO: don't repeat
            else if (typeof(TItem).GetTypeInfo().GetInterfaces()
                .Any(i => i.IsConstructedGenericType && i.GetGenericTypeDefinition() == typeof(IExecutingNotification<>)))
            {
                var commandType = typeof(TItem).GetTypeInfo().GetInterfaces()
                    .Single(i => i.IsConstructedGenericType &&
                                 i.GetGenericTypeDefinition() == typeof(IExecutingNotification<>)).GenericTypeArguments.First();

                if (!executingNotifications.ContainsKey(commandType))
                {
                    executingNotifications.Add(commandType, new List<Type>
                    {
                        typeof(TItem)
                    });
                }
                else
                {
                    ((List<Type>)executingNotifications[commandType]).Add(typeof(TItem));
                }
            }
            else if (typeof(TItem).GetTypeInfo().GetInterfaces()
                .Any(i => i.IsConstructedGenericType && i.GetGenericTypeDefinition() == typeof(IExecutedNotification<,>)))
            {
                var commandType = typeof(TItem).GetTypeInfo().GetInterfaces()
                    .Single(i => i.IsConstructedGenericType &&
                                 i.GetGenericTypeDefinition() == typeof(IExecutedNotification<,>)).GenericTypeArguments.First();

                if (!executedNotifications.ContainsKey(commandType))
                {
                    executedNotifications.Add(commandType, new List<Type>
                    {
                        typeof(TItem)
                    });
                }
                else
                {
                    ((List<Type>)executedNotifications[commandType]).Add(typeof(TItem));
                }
            }
            else if (typeof(TItem).GetTypeInfo().GetInterfaces()
                .Any(i => i.IsConstructedGenericType && i.GetGenericTypeDefinition() == typeof(IExceptionNotification<>)))
            {
                var commandType = typeof(TItem).GetTypeInfo().GetInterfaces()
                    .Single(i => i.IsConstructedGenericType &&
                                 i.GetGenericTypeDefinition() == typeof(IExceptionNotification<>)).GenericTypeArguments.First();

                if (!exceptionNotifications.ContainsKey(commandType))
                {
                    exceptionNotifications.Add(commandType, new List<Type>
                    {
                        typeof(TItem)
                    });
                }
                else
                {
                    ((List<Type>)exceptionNotifications[commandType]).Add(typeof(TItem));
                }
            }
            else
            {
                throw new ArgumentException($"CommandsBuilder cannot use {typeof(TItem).Name}");
            }
            return this;
        }
    }
}

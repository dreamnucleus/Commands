using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DreamNucleus.Commands.Notifications;
using DreamNucleus.Commands.Pipelines;

namespace DreamNucleus.Commands.Builder
{
    public class CommandsBuilder : ICommandsBuilder
    {
        private readonly List<Type> _pipelines = new List<Type>();
        private readonly Dictionary<Type, IReadOnlyCollection<Type>> _executingNotifications = new Dictionary<Type, IReadOnlyCollection<Type>>();
        private readonly Dictionary<Type, IReadOnlyCollection<Type>> _executedNotifications = new Dictionary<Type, IReadOnlyCollection<Type>>();
        private readonly Dictionary<Type, IReadOnlyCollection<Type>> _exceptionNotifications = new Dictionary<Type, IReadOnlyCollection<Type>>();

        public IReadOnlyCollection<Type> Pipelines => _pipelines;
        public IReadOnlyDictionary<Type, IReadOnlyCollection<Type>> ExecutingNotifications => _executingNotifications;
        public IReadOnlyDictionary<Type, IReadOnlyCollection<Type>> ExecutedNotifications => _executedNotifications;
        public IReadOnlyDictionary<Type, IReadOnlyCollection<Type>> ExceptionNotifications => _exceptionNotifications;

        public virtual ICommandsBuilder Use<TItem>()
            where TItem : IUseCommandsBuilder
        {
            if (typeof(TItem).GetTypeInfo().IsSubclassOf(typeof(Pipeline)))
            {
                _pipelines.Add(typeof(TItem));
            }
            // TODO: don't repeat
            else if (typeof(TItem).GetTypeInfo().GetInterfaces()
                .Any(i => i.IsConstructedGenericType && i.GetGenericTypeDefinition() == typeof(IExecutingNotification<>)))
            {
                var commandType = typeof(TItem).GetTypeInfo().GetInterfaces()
                    .Single(i => i.IsConstructedGenericType &&
                                 i.GetGenericTypeDefinition() == typeof(IExecutingNotification<>)).GenericTypeArguments.First();

                if (!_executingNotifications.ContainsKey(commandType))
                {
                    _executingNotifications.Add(commandType, new List<Type>
                    {
                        typeof(TItem)
                    });
                }
                else
                {
                    ((List<Type>)_executingNotifications[commandType]).Add(typeof(TItem));
                }
            }
            else if (typeof(TItem).GetTypeInfo().GetInterfaces()
                .Any(i => i.IsConstructedGenericType && i.GetGenericTypeDefinition() == typeof(IExecutedNotification<,>)))
            {
                var commandType = typeof(TItem).GetTypeInfo().GetInterfaces()
                    .Single(i => i.IsConstructedGenericType &&
                                 i.GetGenericTypeDefinition() == typeof(IExecutedNotification<,>)).GenericTypeArguments.First();

                if (!_executedNotifications.ContainsKey(commandType))
                {
                    _executedNotifications.Add(commandType, new List<Type>
                    {
                        typeof(TItem)
                    });
                }
                else
                {
                    ((List<Type>)_executedNotifications[commandType]).Add(typeof(TItem));
                }
            }
            else if (typeof(TItem).GetTypeInfo().GetInterfaces()
                .Any(i => i.IsConstructedGenericType && i.GetGenericTypeDefinition() == typeof(IExceptionNotification<>)))
            {
                var commandType = typeof(TItem).GetTypeInfo().GetInterfaces()
                    .Single(i => i.IsConstructedGenericType &&
                                 i.GetGenericTypeDefinition() == typeof(IExceptionNotification<>)).GenericTypeArguments.First();

                if (!_exceptionNotifications.ContainsKey(commandType))
                {
                    _exceptionNotifications.Add(commandType, new List<Type>
                    {
                        typeof(TItem)
                    });
                }
                else
                {
                    ((List<Type>)_exceptionNotifications[commandType]).Add(typeof(TItem));
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

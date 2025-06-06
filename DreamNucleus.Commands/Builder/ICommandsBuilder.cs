﻿using System;
using System.Collections.Generic;

namespace DreamNucleus.Commands.Builder
{
    // TODO: put 
    public interface ICommandsBuilder
    {
        IReadOnlyCollection<Type> Pipelines { get; }

        IReadOnlyCollection<Type> PrePipelines { get; }
        IReadOnlyCollection<Type> ExecutingPipelines { get; }
        IReadOnlyCollection<Type> ExecutedPipelines { get; }
        IReadOnlyCollection<Type> ExceptionPipelines { get; }
        IReadOnlyCollection<Type> PostPipelines { get; }

        IReadOnlyDictionary<Type, IReadOnlyCollection<Type>> ExecutingNotifications { get; }
        IReadOnlyDictionary<Type, IReadOnlyCollection<Type>> ExecutedNotifications { get; }
        IReadOnlyDictionary<Type, IReadOnlyCollection<Type>> ExceptionNotifications { get; }


        ICommandsBuilder Use<TItem>()
            where TItem : IUseCommandsBuilder;
    }
}

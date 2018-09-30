using System;

namespace DreamNucleus.Commands
{
#pragma warning disable CA1032 // Implement standard exception constructors
    public sealed class ResultNotRegisteredException : Exception
#pragma warning restore CA1032 // Implement standard exception constructors
    {
        internal ResultNotRegisteredException(Type command, Type result)
            : base($"There was no handler registered for the result {result.Name} returned by {command.Name}. " +
                    "Results must be of the same type and not a base type.")
        {
        }
    }
}

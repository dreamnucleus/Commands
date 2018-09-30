using System;

namespace DreamNucleus.Commands
{
#pragma warning disable CA1032 // Implement standard exception constructors
    public class DependencyNotRegisteredException : Exception
#pragma warning restore CA1032 // Implement standard exception constructors
    {
        public DependencyNotRegisteredException(Type type, Exception exception)
            : base($"There was no dependency registered of type {type.Name}.", exception)
        {
        }
    }
}

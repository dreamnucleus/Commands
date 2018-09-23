using System;

namespace DreamNucleus.Commands.Extensions
{
    /// <summary>
    /// This will ensure the command is only run once at a time
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class SemaphoreAttribute : Attribute
    {
        // TODO: count, hash, time...etc rules of the semaphore
    }
}

using System;

// TODO: REMOVE
namespace DreamNucleus.Commands.Extensions.Semaphore
{
    // TODO: count, hash, time...etc rules of the semaphore
    /// <summary>
    /// This will ensure the command is only run once at a time
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class SemaphoreAttribute : Attribute
    {
    }
}

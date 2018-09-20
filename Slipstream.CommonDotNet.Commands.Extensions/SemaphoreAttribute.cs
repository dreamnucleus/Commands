using System;

namespace Slipstream.CommonDotNet.Commands.Extensions
{
    /// <summary>
    /// This will ensure the command is only run once at a time
    /// </summary>
    public class SemaphoreAttribute : Attribute
    {
        // TODO: count, hash, time...etc rules of the semaphore
    }
}

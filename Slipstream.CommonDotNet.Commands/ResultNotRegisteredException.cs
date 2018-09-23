using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Slipstream.CommonDotNet.Commands
{
#pragma warning disable CA1032 // Implement standard exception constructors
    public class ResultNotRegisteredException : Exception
#pragma warning restore CA1032 // Implement standard exception constructors
    {
        internal ResultNotRegisteredException(Type command, Type result)
            : base($"There was no handler registered for the result {result.Name} returned by {command.Name}. " +
                    "Results must be of the same type and not a base type.")
        {
        }
    }
}

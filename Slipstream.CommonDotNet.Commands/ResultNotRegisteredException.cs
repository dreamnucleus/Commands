using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Slipstream.CommonDotNet.Commands
{
    public class ResultNotRegisteredException : Exception
    {
        public ResultNotRegisteredException(Type command, Type result)
            : base($"There was no handler registered for the result {result.Name} returned by {command.Name}." + "" +
                    "Results must be of the same type and not a base type.")
        {
        }
    }
}

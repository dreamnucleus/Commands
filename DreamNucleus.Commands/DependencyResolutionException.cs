using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DreamNucleus.Commands
{
#pragma warning disable CA1032 // Implement standard exception constructors
    public class DependencyResolutionException : Exception
#pragma warning restore CA1032 // Implement standard exception constructors
    {
        public DependencyResolutionException(Type type, Exception exception)
            : base($"There was an error in resolving the type {type.Name}.", exception)
        {
        }
    }
}

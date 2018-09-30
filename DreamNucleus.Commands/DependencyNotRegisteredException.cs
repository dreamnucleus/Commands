using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DreamNucleus.Commands
{
#pragma warning disable CA1032 // Implement standard exception constructors
    public class DependencyNotRegisteredException : Exception
#pragma warning restore CA1032 // Implement standard exception constructors
    {
        public DependencyNotRegisteredException(Type type)
            : base($"There was no dependency registered of type {type.Name}.")
        {
            
        }
    }
}

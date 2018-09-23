using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace Slipstream.CommonDotNet.Commands.Results
{
    [ExcludeFromCodeCoverage]
    public struct Unit : IEquatable<Unit>
    {
        public static Unit Value { get; } = new Unit();

        public bool Equals(Unit other)
        {
            return true;
        }

        public override bool Equals(object obj)
        {
            return obj is Unit;
        }

        public override int GetHashCode()
        {
            return 0;
        }

        // TODO: why? return string.Empty;?
        public override string ToString()
        {
            return "()";
        }

#pragma warning disable CA1801 // Review unused parameters
        public static bool operator ==(Unit first, Unit second)
#pragma warning restore CA1801 // Review unused parameters
        {
            return true;
        }
#pragma warning disable CA1801 // Review unused parameters
        public static bool operator !=(Unit first, Unit second)
#pragma warning restore CA1801 // Review unused parameters
        {
            return false;
        }
    }
}

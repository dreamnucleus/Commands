using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Slipstream.CommonDotNet.Commands.Results
{
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

        public override string ToString()
        {
            // TODO: why? return string.Empty;?
            return "()";
        }

        public static bool operator ==(Unit first, Unit second)
        {
            return true;
        }

        public static bool operator !=(Unit first, Unit second)
        {
            return false;
        }
    }
}

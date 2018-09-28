using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DreamNucleus.Commands.Tests.Common
{
    public class ObjectResult
    {
        public object Result { get; }

        public ObjectResult(object result)
        {
            Result = result;
        }
    }
}

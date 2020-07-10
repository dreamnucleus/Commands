using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DreamNucleus.Commands.Extensions.Redis
{
    internal class ResultTransport
    {
        public Guid Id { get; set; }
        public object Result { get; set; }

        public ResultTransport(Guid id, object result)
        {
            Id = id;
            Result = result;
        }

        public ResultTransport()
        {
        }
    }
}

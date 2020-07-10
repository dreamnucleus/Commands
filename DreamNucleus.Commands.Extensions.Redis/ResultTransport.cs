using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DreamNucleus.Commands.Extensions.Redis
{
    internal class ResultTransport<TResult>
    {
        public Guid Id { get; set; }
        public bool Success { get; set; }
        public TResult Result { get; set; }
        public Exception Exception { get; set; }

        public ResultTransport()
        {
        }
    }
}

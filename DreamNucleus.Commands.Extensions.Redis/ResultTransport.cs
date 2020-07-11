using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DreamNucleus.Commands.Extensions.Redis
{
    // TODO: should this be an interface for extra properties?
    public class ResultTransport<TSuccessResult> : IResultTransport<TSuccessResult>
    {
        public string Id { get; set; }
        public bool Success { get; set; }
        public TSuccessResult Result { get; set; }
        public Exception Exception { get; set; }

        public ResultTransport()
        {
        }
    }
}

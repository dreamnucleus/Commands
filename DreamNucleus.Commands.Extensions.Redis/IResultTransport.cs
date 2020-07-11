using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DreamNucleus.Commands.Extensions.Redis
{
    public interface IResultTransport : ITransport
    {
        bool Success { get; set; }
    }

    public interface IResultTransport<TResult> : IResultTransport
    {
        TResult Result { get; set; }
        Exception Exception { get; set; }
    }
}

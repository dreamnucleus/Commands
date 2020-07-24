using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DreamNucleus.Commands.Extensions.Redis
{
    public interface IResultContainer : IContainer
    {
        bool Success { get; set; }
    }

    public interface IResultContainer<TResult> : IResultContainer
    {
        TResult Result { get; set; }
        Exception Exception { get; set; }
    }
}

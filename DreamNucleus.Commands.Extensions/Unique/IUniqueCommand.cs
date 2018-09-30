using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DreamNucleus.Commands.Extensions.Unique
{
    // TODO: this could jst be a semaphore which doesn't expire
    public interface IUniqueCommand
    {
        string UniqueCommandId { get; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DreamNucleus.Commands.Extensions.Redis
{
    public interface ICommandContainer : IContainer
    {
        object Command { get; set; }
    }
}

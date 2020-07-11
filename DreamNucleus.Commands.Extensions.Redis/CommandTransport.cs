using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DreamNucleus.Commands.Extensions.Redis
{
    // TODO: should this be an interface for extra properties?
    public sealed class CommandTransport
    {
        public Guid Id { get; set; }
        public object Command { get; set; }

        public CommandTransport(Guid id, object command)
        {
            Id = id;
            Command = command;
        }

        public CommandTransport()
        {
        }
    }
}

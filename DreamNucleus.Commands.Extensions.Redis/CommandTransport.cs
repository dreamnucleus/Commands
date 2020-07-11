using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DreamNucleus.Commands.Extensions.Redis
{
    public sealed class CommandTransport : ICommandTransport
    {
        public string Id { get; set; }
        public object Command { get; set; }

        public CommandTransport(string id, object command)
        {
            Id = id;
            Command = command;
        }

        public CommandTransport()
        {
        }
    }
}

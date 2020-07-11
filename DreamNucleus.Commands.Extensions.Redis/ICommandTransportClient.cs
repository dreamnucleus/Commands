using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DreamNucleus.Commands.Extensions.Redis
{
    public interface ICommandTransportClient
    {
        Task SendAsync(CommandTransport commandTransport);

        // wait for response
    }
}

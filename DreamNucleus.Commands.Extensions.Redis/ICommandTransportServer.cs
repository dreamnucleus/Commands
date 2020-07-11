using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DreamNucleus.Commands.Extensions.Redis
{
    public interface ICommandTransportServer
    {
        Task StartAsync();
        Task ListenAsync(Func<ICommandTransport, Task> listenFunc);
        Task SendAsync(IResultTransport resultTransport);
        Task StopAsync();
    }
}

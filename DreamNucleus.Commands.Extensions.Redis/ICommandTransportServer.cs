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
        // TODO: Listen is clunky, should be called StartListen at the very least
        Task ListenAsync(Func<ICommandContainer, Task> listenFunc);
        Task SendAsync(IResultContainer resultTransport);
        Task StopAsync();
    }
}

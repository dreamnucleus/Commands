using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DreamNucleus.Commands.Extensions.Semaphore;

namespace DreamNucleus.Commands.Extensions.Unique
{
    public interface IUniqueManager
    {
        Task<bool> CheckAsync(string uniqueCommandId, CancellationToken cancellationToken);
    }
}

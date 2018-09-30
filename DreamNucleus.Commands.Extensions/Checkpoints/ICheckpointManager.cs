using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DreamNucleus.Commands.Extensions.Checkpoints
{
    public interface ICheckpointManager
    {
        Task<int> GetIndexAsync(string sectionId);
        Task UpdateIndexAsync(string sectionId, int index);

        Task<bool> GetCompleteAsync(string sectionId);
        Task UpdateCompleteAsync(string sectionId);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DreamNucleus.Commands.Extensions.Checkpoints;

namespace DreamNucleus.Commands.Extensions.Tests.Common
{
    public class FakeCheckpointManager : ICheckpointManager
    {
        public Task<int> GetIndexAsync(string sectionId)
        {
            throw new NotImplementedException();
        }

        public Task UpdateIndexAsync(string sectionId, int index)
        {
            throw new NotImplementedException();
        }

        public Task<bool> GetCompleteAsync(string sectionId)
        {
            throw new NotImplementedException();
        }

        public Task UpdateCompleteAsync(string sectionId)
        {
            throw new NotImplementedException();
        }
    }
}

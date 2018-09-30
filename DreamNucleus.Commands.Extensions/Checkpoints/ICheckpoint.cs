using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DreamNucleus.Commands.Extensions.Checkpoints
{
    // TODO: this would need to be a guaranteed command
    // TODO: would using a queue make it easier... for lists maybe...
    // TODO: this get very tricky with code changes...
    // TODO: or should a whole command just be checkpointed and then we just call them.. similar to unique but doesn't throw
    public class Checkpoint
    {
        private readonly ICheckpointManager _checkpointManager;

        public Checkpoint(ICheckpointManager checkpointManager)
        {
            _checkpointManager = checkpointManager;
        }

        public async Task RunAsync(Action action, string sectionId)
        {
            var complete = await _checkpointManager.GetCompleteAsync(sectionId).ConfigureAwait(false);

            if (!complete)
            {
                action();
                await _checkpointManager.UpdateCompleteAsync(sectionId).ConfigureAwait(false);
            }
        }

        public async Task ForEachAsync<T>(IList<T> list, Action<T> action, string sectionId)
        {
            var index = await _checkpointManager.GetIndexAsync(sectionId).ConfigureAwait(false);

            for (; index < list.Count; index++)
            {
                action(list[index]);
                await _checkpointManager.UpdateIndexAsync(sectionId, index).ConfigureAwait(false);
            }
        }
    }
}

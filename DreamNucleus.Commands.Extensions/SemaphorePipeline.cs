using System;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using DreamNucleus.Commands.Pipelines;

namespace DreamNucleus.Commands.Extensions
{
    // TODO: need to keep on renewing the lock for longer running tasks...
    public sealed class SemaphorePipeline : Pipeline
    {
        private readonly ILockManager _lockManager;

        public SemaphorePipeline(ILockManager lockManager)
        {
            _lockManager = lockManager;
        }

        private static bool IsSemaphoreCommand(IAsyncCommand command) => command.GetType().GetTypeInfo().GetCustomAttribute<SemaphoreAttribute>() != null;

        public override async Task ExecutingAsync(IAsyncCommand command)
        {
            // TODO: maybe have a execute if/when in the pipeline?
            if (IsSemaphoreCommand(command))
            {
                var @lock = await _lockManager.AcquireAsync(command.GetType().GetHashCode().ToString(), CancellationToken.None).ConfigureAwait(false);
            }
        }

        public override async Task ExecutedAsync(IAsyncCommand command, object result)
        {
            if (IsSemaphoreCommand(command))
            {
                await _lockManager.ReleaseAsync(command.GetType().GetHashCode().ToString(), "", CancellationToken.None).ConfigureAwait(false);
            }
        }

        public override async Task ExceptionAsync(IAsyncCommand command, Exception exception)
        {
            if (IsSemaphoreCommand(command))
            {
                await _lockManager.ReleaseAsync(command.GetType().GetHashCode().ToString(), "", CancellationToken.None).ConfigureAwait(false);
            }
        }
    }
}

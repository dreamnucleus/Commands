using System;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Slipstream.CommonDotNet.Commands.Extensions.Results;
using Slipstream.CommonDotNet.Commands.Pipelines;

namespace Slipstream.CommonDotNet.Commands.Extensions
{
    // TODO: need to keep on renewing the lock for longer running tasks...
    public class SemaphorePipeline : Pipeline
    {
        private readonly ILockManager _lockManager;

        public SemaphorePipeline(ILockManager lockManager)
        {
            _lockManager = lockManager;
        }

        private bool IsSemaphoreCommand(IAsyncCommand command) => command.GetType().GetTypeInfo().GetCustomAttribute<SemaphoreAttribute>() != null;

        public override async Task ExecutingAsync(IAsyncCommand command)
        {
            // TODO: maybe have a execute if/when in the pipeline?
            if (IsSemaphoreCommand(command))
            {
                var @lock = await _lockManager.AcquireAsync(command.GetType().GetHashCode().ToString(), CancellationToken.None);
            }
        }

        public override async Task ExecutedAsync(IAsyncCommand command, object result)
        {
            if (IsSemaphoreCommand(command))
            {
                await _lockManager.ReleaseAsync(command.GetType().GetHashCode().ToString(), "", CancellationToken.None);
            }
        }

        public override async Task ExceptionAsync(IAsyncCommand command, Exception exception)
        {
            if (IsSemaphoreCommand(command))
            {
                await _lockManager.ReleaseAsync(command.GetType().GetHashCode().ToString(), "", CancellationToken.None);
            }
        }

    }
}

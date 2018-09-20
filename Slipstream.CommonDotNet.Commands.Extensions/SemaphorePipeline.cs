using System;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Slipstream.CommonDotNet.Commands.Pipelines;

namespace Slipstream.CommonDotNet.Commands.Extensions
{
    // TODO: probably should move this to another library when i can think of the name
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
                try
                {
                    var @lock = await _lockManager.AcquireAsync(command.GetType().GetHashCode().ToString(), CancellationToken.None);
                }
                catch (Exception e)
                {
                    throw new Exception();
                }
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

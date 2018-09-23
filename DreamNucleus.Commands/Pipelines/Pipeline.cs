using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace DreamNucleus.Commands.Pipelines
{
    [ExcludeFromCodeCoverage]
    public abstract class Pipeline : IUseCommandsBuilder
    {
        public virtual Task ExecutingAsync(IAsyncCommand command)
        {
            return Task.FromResult(0);
        }

        public virtual Task ExecutedAsync(IAsyncCommand command, Object result)
        {
            return Task.FromResult(0);
        }

        public virtual Task ExceptionAsync(IAsyncCommand command, Exception exception)
        {
            return Task.FromResult(0);
        }
    }
}

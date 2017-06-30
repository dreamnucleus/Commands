using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slipstream.CommonDotNet.Commands.Pipelines
{
    public abstract class Pipeline
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

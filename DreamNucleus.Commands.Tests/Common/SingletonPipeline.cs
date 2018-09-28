using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DreamNucleus.Commands.Pipelines;

namespace DreamNucleus.Commands.Tests.Common
{
    [Singleton]
    public class SingletonPipeline : Pipeline
    {
        public override Task ExecutingAsync(IAsyncCommand command)
        {
            return base.ExecutingAsync(command);
        }

        public override Task ExecutedAsync(IAsyncCommand command, object result)
        {
            return base.ExecutedAsync(command, result);
        }

        public override Task ExceptionAsync(IAsyncCommand command, Exception exception)
        {
            return base.ExceptionAsync(command, exception);
        }
    }
}

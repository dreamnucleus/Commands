using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Slipstream.CommonDotNet.Commands.Pipelines;

namespace Slipstream.CommonDotNet.Commands.Tests
{
    public class TestPipeline : Pipeline
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

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using DreamNucleus.Commands.Pipelines;

namespace DreamNucleus.Commands.Extensions.Retry
{
    public class RetryPipeline : Pipeline
    {
        private readonly ICommandProcessor _commandProcessor;

        // HACK:
        private readonly ConcurrentDictionary<object, int> _retries = new ConcurrentDictionary<object, int>();

        public RetryPipeline(ICommandProcessor commandProcessor)
        {
            _commandProcessor = commandProcessor;
        }

        public override Task ExceptionAsync(IAsyncCommand command, Exception exception)
        {
            var retryAttribute = (RetryAttribute)Attribute.GetCustomAttribute(command.GetType(), typeof(RetryAttribute));

            if (retryAttribute != null)
            {
                _retries.TryAdd(command, 0);

                _retries.TryGetValue(command, out var retries);

                if (retries <= retryAttribute.Retries)
                {
                    _retries.TryUpdate(command, retries + 1, retries);

                }
            }
        }
    }
}

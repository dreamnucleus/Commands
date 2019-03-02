using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using DreamNucleus.Commands.Pipelines;
using DreamNucleus.Commands.Results;

// TODO: REMOVE
namespace DreamNucleus.Commands.Extensions.Retry
{
    public class RetryExceptionPipeline : ExceptionPipeline
    {
        // HACK: just playing
        private readonly ConcurrentDictionary<object, int> _retries = new ConcurrentDictionary<object, int>();

        private readonly ICommandProcessor _commandProcessor;

        public RetryExceptionPipeline(ICommandProcessor commandProcessor, ExceptionPipeline nextExceptionPipeline)
            : base(nextExceptionPipeline)
        {
            _commandProcessor = commandProcessor;
        }

        public override async Task<object> ExceptionAsync<TCommand, TSuccessResult>(ISuccessResult<TCommand, TSuccessResult> command, Exception exception)
        {
            var retryAttribute = (RetryAttribute)Attribute.GetCustomAttribute(command.GetType(), typeof(RetryAttribute));

            if (retryAttribute != null)
            {
                _retries.TryAdd(command, 0);

                _retries.TryGetValue(command, out var retries);

                if (retries < retryAttribute.Retries)
                {
                    _retries.TryUpdate(command, retries + 1, retries);

                    return await _commandProcessor.ProcessAsync(command).ConfigureAwait(false);
                }
            }

            return await base.ExceptionAsync(command, exception).ConfigureAwait(false);
        }
    }
}

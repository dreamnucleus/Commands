using System;
using System.Threading.Tasks;

namespace DreamNucleus.Commands.Pipelines
{
    public abstract class OutgoingPipeline
    {
        private readonly OutgoingPipeline _nextOutgoingPipeline;

        public OutgoingPipeline(OutgoingPipeline nextOutgoingPipeline)
        {
            _nextOutgoingPipeline = nextOutgoingPipeline;
        }

        public virtual Task<Object> ExecutedAsync(IAsyncCommand command, Object result)
        {
            return _nextOutgoingPipeline.ExecutedAsync(command, result);
        }

        public virtual Task<Object> ExceptionAsync(IAsyncCommand command, Exception exception)
        {
            return _nextOutgoingPipeline.ExceptionAsync(command, exception);
        }
    }

    public abstract class RetryOutgoingPipeline : OutgoingPipeline
    {
        private readonly ICommandProcessor _commandProcessor;
        private readonly OutgoingPipeline _nextOutgoingPipeline;

        public RetryOutgoingPipeline(ICommandProcessor commandProcessor, OutgoingPipeline nextOutgoingPipeline)
            : base(nextOutgoingPipeline)
        {
            _commandProcessor = commandProcessor;
            _nextOutgoingPipeline = nextOutgoingPipeline;
        }

        //public virtual async Task<Object> ExceptionAsync(IAsyncCommand command, Exception exception)
        //{
        //    return await _commandProcessor.ProcessAsync(command);
        //}
    }
}

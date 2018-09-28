using System;
using System.Threading.Tasks;
using DreamNucleus.Commands.Results;

namespace DreamNucleus.Commands.Pipelines
{
    public abstract class OutgoingPipeline
    {
        private readonly OutgoingPipeline _nextOutgoingPipeline;

        public OutgoingPipeline(OutgoingPipeline nextOutgoingPipeline)
        {
            _nextOutgoingPipeline = nextOutgoingPipeline;
        }

        public virtual Task<TSuccessResult> ExecutedAsync<TCommand, TSuccessResult>(ISuccessResult<TCommand, TSuccessResult> command, TSuccessResult result)
            where TCommand : IAsyncCommand
        {
            return _nextOutgoingPipeline.ExecutedAsync(command, result);
        }

        public virtual Task<Object> ExceptionAsync<TCommand, TSuccessResult>(ISuccessResult<TCommand, TSuccessResult> command, Exception exception)
            where TCommand : IAsyncCommand
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

        public override async Task<object> ExceptionAsync<TCommand, TSuccessResult>(ISuccessResult<TCommand, TSuccessResult> command, Exception exception)
        {
            var random = new Random();
            if (random.Next(0, 3) == 2)
            {
                return await _commandProcessor.ProcessAsync(command).ConfigureAwait(false);
            }
            else
            {
                return ExceptionAsync(command, exception);
            }
        }
    }
}

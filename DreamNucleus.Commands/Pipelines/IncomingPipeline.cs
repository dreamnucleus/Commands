using System;
using System.Threading.Tasks;
using DreamNucleus.Commands.Results;

namespace DreamNucleus.Commands.Pipelines
{
    public abstract class IncomingPipeline
    {
        private readonly IncomingPipeline _nextIncomingPipeline;

        public IncomingPipeline(IncomingPipeline nextIncomingPipeline)
        {
            _nextIncomingPipeline = nextIncomingPipeline;
        }

        public virtual Task<TSuccessResult> ExecutingAsync<TCommand, TSuccessResult>(ISuccessResult<TCommand, TSuccessResult> command)
            where TCommand : IAsyncCommand
        {
            return _nextIncomingPipeline.ExecutingAsync(command);
        }
    }


    public class CacheIncomingPipeline : IncomingPipeline
    {
        private readonly IncomingPipeline _nextIncomingPipeline;

        public CacheIncomingPipeline(IncomingPipeline nextIncomingPipeline)
            : base(nextIncomingPipeline)
        {
            _nextIncomingPipeline = nextIncomingPipeline;
        }

        public override Task<TSuccessResult> ExecutingAsync<TCommand, TSuccessResult>(ISuccessResult<TCommand, TSuccessResult> command)
        {
            return Task.FromResult(default(TSuccessResult)); // don't need next one...
        }
    }
}

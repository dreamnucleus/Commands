using System;
using System.Threading.Tasks;

namespace DreamNucleus.Commands.Pipelines
{
    public abstract class IncomingPipeline
    {
        private readonly IncomingPipeline _nextIncomingPipeline;

        public IncomingPipeline(IncomingPipeline nextIncomingPipeline)
        {
            _nextIncomingPipeline = nextIncomingPipeline;
        }

        public virtual Task<Object> ExecutingAsync(IAsyncCommand command)
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

        public virtual Task<Object> ExecutingAsync(IAsyncCommand command)
        {
            return Task.FromResult(new Object()); // don't need next one...
        }
    }
}

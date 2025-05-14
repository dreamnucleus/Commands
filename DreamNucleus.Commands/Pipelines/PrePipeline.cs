using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using DreamNucleus.Commands.Results;

namespace DreamNucleus.Commands.Pipelines
{
    [ExcludeFromCodeCoverage]
    public abstract class PrePipeline : IUseCommandsBuilder
    {
        private readonly PrePipeline _nextPrePipeline;

        protected PrePipeline(PrePipeline nextPrePipeline)
        {
            _nextPrePipeline = nextPrePipeline;
        }

        public virtual Task PreAsync<TCommand, TSuccessResult>(ISuccessResult<TCommand, TSuccessResult> command)
            where TCommand : IAsyncCommand
        {
            return _nextPrePipeline.PreAsync(command);
        }
    }

    internal sealed class FinalPrePipeline : PrePipeline
    {
        public FinalPrePipeline()
            : base(null)
        {
        }

        public override Task PreAsync<TCommand, TSuccessResult>(ISuccessResult<TCommand, TSuccessResult> command)
        {
            return Task.CompletedTask;
        }
    }
}

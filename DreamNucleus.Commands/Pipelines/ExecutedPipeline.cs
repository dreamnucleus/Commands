using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using DreamNucleus.Commands.Results;

namespace DreamNucleus.Commands.Pipelines
{
    [ExcludeFromCodeCoverage]
    public abstract class ExecutedPipeline : IUseCommandsBuilder
    {
        private readonly ExecutedPipeline _nextExecutedPipeline;

        protected ExecutedPipeline(ExecutedPipeline nextExecutedPipeline)
        {
            _nextExecutedPipeline = nextExecutedPipeline;
        }

        public virtual Task<TSuccessResult> ExecutedAsync<TCommand, TSuccessResult>(ISuccessResult<TCommand, TSuccessResult> command, TSuccessResult result)
            where TCommand : IAsyncCommand
        {
            return _nextExecutedPipeline.ExecutedAsync(command, result);
        }
    }

    internal sealed class FinalExecutedPipeline : ExecutedPipeline
    {
        public FinalExecutedPipeline()
            : base(null)
        {
        }

        public override Task<TSuccessResult> ExecutedAsync<TCommand, TSuccessResult>(ISuccessResult<TCommand, TSuccessResult> command, TSuccessResult result)
        {
            return Task.FromResult(result);
        }
    }
}

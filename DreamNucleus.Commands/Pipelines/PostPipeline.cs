using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using DreamNucleus.Commands.Results;

namespace DreamNucleus.Commands.Pipelines
{
    [ExcludeFromCodeCoverage]
    public abstract class PostPipeline : IUseCommandsBuilder
    {
        private readonly PostPipeline _nextPostPipeline;

        protected PostPipeline(PostPipeline nextPostPipeline)
        {
            _nextPostPipeline = nextPostPipeline;
        }

        public virtual Task PostAsync<TCommand, TSuccessResult>(ISuccessResult<TCommand, TSuccessResult> command)
            where TCommand : IAsyncCommand
        {
            return _nextPostPipeline.PostAsync(command);
        }
    }

    internal sealed class FinalPostPipeline : PostPipeline
    {
        public FinalPostPipeline()
            : base(null)
        {
        }

        public override Task PostAsync<TCommand, TSuccessResult>(ISuccessResult<TCommand, TSuccessResult> command)
        {
            return Task.CompletedTask;
        }
    }
}

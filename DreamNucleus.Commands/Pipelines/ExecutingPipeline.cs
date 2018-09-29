using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using DreamNucleus.Commands.Results;

namespace DreamNucleus.Commands.Pipelines
{
    [ExcludeFromCodeCoverage]
    public abstract class ExecutingPipeline : IUseCommandsBuilder
    {
        private readonly ExecutingPipeline _nextExecutingPipeline;

        protected ExecutingPipeline(ExecutingPipeline nextExecutingPipeline)
        {
            _nextExecutingPipeline = nextExecutingPipeline;
        }

        public virtual Task<TSuccessResult> ExecutingAsync<TCommand, TSuccessResult>(ISuccessResult<TCommand, TSuccessResult> command)
            where TCommand : IAsyncCommand
        {
            return _nextExecutingPipeline.ExecutingAsync(command);
        }
    }

    internal sealed class FinalExecutingPipeline : ExecutingPipeline
    {
        private readonly InternalCommandProcessor _commandProcessor;

        internal FinalExecutingPipeline(InternalCommandProcessor commandProcessor)
            : base(null)
        {
            _commandProcessor = commandProcessor;
        }

        public override Task<TSuccessResult> ExecutingAsync<TCommand, TSuccessResult>(ISuccessResult<TCommand, TSuccessResult> command)
        {
            return _commandProcessor.ExecuteAsync(command);
        }
    }
}

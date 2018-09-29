using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using DreamNucleus.Commands.Results;

namespace DreamNucleus.Commands.Pipelines
{
    [ExcludeFromCodeCoverage]
    public abstract class ExceptionPipeline : IUseCommandsBuilder
    {
        private readonly ExceptionPipeline _nextExceptionPipeline;

        protected ExceptionPipeline(ExceptionPipeline nextExceptionPipeline)
        {
            _nextExceptionPipeline = nextExceptionPipeline;
        }

        public virtual Task<object> ExceptionAsync<TCommand, TSuccessResult>(ISuccessResult<TCommand, TSuccessResult> command, Exception exception)
            where TCommand : IAsyncCommand
        {
            return _nextExceptionPipeline.ExceptionAsync(command, exception);
        }
    }

    internal sealed class FinalExceptionPipeline : ExceptionPipeline
    {
        public FinalExceptionPipeline()
            : base(null)
        {
        }

        public override Task<object> ExceptionAsync<TCommand, TSuccessResult>(ISuccessResult<TCommand, TSuccessResult> command, Exception exception)
        {
            return Task.FromResult((object)exception);
        }
    }
}

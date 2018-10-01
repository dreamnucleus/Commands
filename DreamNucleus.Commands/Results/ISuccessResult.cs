using System.Diagnostics.CodeAnalysis;

namespace DreamNucleus.Commands.Results
{
    public interface ISuccessResult<TCommand, TResult> : IAsyncCommand
        where TCommand : IAsyncCommand
    {
    }

    public interface ISuccessResult<TCommand> : ISuccessResult<TCommand, Unit>
        where TCommand : IAsyncCommand
    {
    }

    [ExcludeFromCodeCoverage]
    public static class SuccessResultExtensions
    {
        public static TResult Success<TCommand, TResult>(this ISuccessResult<TCommand, TResult> result)
            where TCommand : IAsyncCommand
        {
            return default;
        }
    }
}

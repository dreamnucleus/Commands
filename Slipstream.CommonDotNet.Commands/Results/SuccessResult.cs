using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slipstream.CommonDotNet.Commands.Results
{
    public interface ISuccessResult<TCommand, TResult> : IAsyncCommand
    where TCommand : IAsyncCommand
    {
    }

    public interface ISuccessResult<TCommand> : ISuccessResult<TCommand, Unit>
        where TCommand : IAsyncCommand
    {
    }


    public static class SuccessResultExtensions
    {
        public static TResult Success<TCommand, TResult>(this ISuccessResult<TCommand, TResult> result)
            where TCommand : IAsyncCommand
        {
            return default(TResult);
        }

    }
}

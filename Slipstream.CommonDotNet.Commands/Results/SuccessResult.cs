using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slipstream.CommonDotNet.Commands.Results
{
    //public interface ISuccessResult<TCommand> : IAsyncCommand
    //{
    //}

    public interface ISuccessResult<TCommand, TResult> : IAsyncCommand
    {
    }

    // TODO: does there have to be a result, can I just return an void
    //public class SuccessResult : IResult
    //{
    //}

    public static class SuccessResultExtensions
    {
        public static TResult Success<TCommand, TResult>(this ISuccessResult<TCommand, TResult> result)
        {
            return default(TResult);
        }

        //public static SuccessResult Success<TCommand>(this ISuccessResult<TCommand> result)
        //{
        //    return new SuccessResult();
        //}
    }
}

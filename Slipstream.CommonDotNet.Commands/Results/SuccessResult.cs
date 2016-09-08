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
    // TODO: is there a way to make TResult not have to be a IResult so I can send back an int... maybe I can do something strange like Result.From(int)....
    public interface ISuccessResult<TCommand, TResult> : IAsyncCommand
        where TCommand : IAsyncCommand
        where TResult : IResult
    {
    }

    // TODO: does there have to be a result, can I just return an void
    //public class SuccessResult : IResult
    //{
    //}

    public static class SuccessResultExtensions
    {
        public static TResult Success<TCommand, TResult>(this ISuccessResult<TCommand, TResult> result)
            where TCommand : IAsyncCommand
            where TResult : IResult
        {
            return default(TResult);
        }

        //public static SuccessResult Success<TCommand>(this ISuccessResult<TCommand> result)
        //{
        //    return new SuccessResult();
        //}
    }
}

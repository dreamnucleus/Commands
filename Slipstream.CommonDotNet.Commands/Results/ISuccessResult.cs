using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Slipstream.CommonDotNet.Commands;
using Slipstream.CommonDotNet.Commands.Results;

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

    //public interface ISuccessResult<TCommand> : IAsyncCommand
    //    where TCommand : IAsyncCommand
    //{
    //}
    [ExcludeFromCodeCoverage]
    public static class SuccessResultExtensions
    {
        public static TResult Success<TCommand, TResult>(this ISuccessResult<TCommand, TResult> result)
            where TCommand : IAsyncCommand
        {
            return default(TResult);
        }

        //public static void Success<TCommand>(this ISuccessResult<TCommand> result)
        //   where TCommand : IAsyncCommand
        //{
        //}
    }
}

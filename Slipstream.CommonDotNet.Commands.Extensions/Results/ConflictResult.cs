using System;
using Slipstream.CommonDotNet.Commands.Results;

namespace Slipstream.CommonDotNet.Commands.Extensions.Results
{
    public interface IConflictResult : IErrorResult<ConflictException>
    {
    }

    public class ConflictException : Exception
    {
    }

    public static class ConflictResultExtensions
    {
        public static ConflictException Conflict(this IConflictResult result)
        {
            return default(ConflictException);
        }
    }
}

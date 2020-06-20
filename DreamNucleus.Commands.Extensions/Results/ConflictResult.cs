using System;
using System.Diagnostics.CodeAnalysis;
using DreamNucleus.Commands.Results;

namespace DreamNucleus.Commands.Extensions.Results
{
    [SuppressMessage("Design", "CA1040:Avoid empty interfaces")]
    public interface IConflictResult : IErrorResult<ConflictException>
    {
    }

    public class ConflictException : Exception
    {
        public ConflictException(string message)
            : base(message)
        {
        }

        public ConflictException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        public ConflictException()
        {
        }
    }

    [ExcludeFromCodeCoverage]
    public static class ConflictResultExtensions
    {
        public static ConflictException Conflict(this IConflictResult result)
        {
            return default;
        }
    }
}

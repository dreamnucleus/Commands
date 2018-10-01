using System;
using System.Diagnostics.CodeAnalysis;
using DreamNucleus.Commands.Results;

namespace DreamNucleus.Commands.Extensions.Results
{
    public interface IConflictResult : IErrorResult<ConflictException>
    {
    }

    public class ConflictException : Exception
    {
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

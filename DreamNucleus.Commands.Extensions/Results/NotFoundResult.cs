using System;
using System.Diagnostics.CodeAnalysis;
using DreamNucleus.Commands.Results;

namespace DreamNucleus.Commands.Extensions.Results
{
    [SuppressMessage("Design", "CA1040:Avoid empty interfaces")]
    public interface INotFoundResult : IErrorResult<NotFoundException>
    {
    }

    public class NotFoundException : Exception
    {
        public NotFoundException(string message)
            : base(message)
        {
        }

        public NotFoundException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        public NotFoundException()
        {
        }
    }

    [ExcludeFromCodeCoverage]
    public static class NotFoundResultExtensions
    {
        public static NotFoundException NotFound(this INotFoundResult result)
        {
            return default;
        }
    }
}

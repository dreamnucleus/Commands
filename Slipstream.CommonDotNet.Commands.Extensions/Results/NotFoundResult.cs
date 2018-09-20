using System;
using System.Diagnostics.CodeAnalysis;
using Slipstream.CommonDotNet.Commands.Results;

namespace Slipstream.CommonDotNet.Commands.Extensions.Results
{
    public interface INotFoundResult : IErrorResult<NotFoundException>
    {
    }

    public class NotFoundException : Exception
    {
    }

    [ExcludeFromCodeCoverage]
    public static class NotFoundResultExtensions
    {
        public static NotFoundException NotFound(this INotFoundResult result)
        {
            return default(NotFoundException);
        }
    }
}

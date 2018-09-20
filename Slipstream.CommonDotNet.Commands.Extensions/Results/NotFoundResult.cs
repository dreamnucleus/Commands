using System;
using Slipstream.CommonDotNet.Commands.Results;

namespace Slipstream.CommonDotNet.Commands.Extensions.Results
{
    public interface INotFoundResult : IErrorResult<NotFoundException>
    {
    }

    public class NotFoundException : Exception
    {
    }

    public static class NotFoundResultExtensions
    {
        public static NotFoundException NotFound(this INotFoundResult result)
        {
            return default(NotFoundException);
        }
    }
}

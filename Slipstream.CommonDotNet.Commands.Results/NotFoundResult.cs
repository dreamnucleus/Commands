using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slipstream.CommonDotNet.Commands.Results
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

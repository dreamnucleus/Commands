using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slipstream.CommonDotNet.Commands.Results
{
    public interface INotFoundResult
    {
    }

    public class NotFoundResult : IResult
    {
    }

    public static class NotFoundResultExtensions
    {
        public static NotFoundResult NotFound(this INotFoundResult result)
        {
            return default(NotFoundResult);
        }
    }
}

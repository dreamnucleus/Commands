using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slipstream.CommonDotNet.Commands.Results
{
    public interface IConflictResult
    {
    }

    public class ConflictResult : IResult
    {
    }

    public static class ConflictResultExtensions
    {
        public static ConflictResult NotFound(this IConflictResult result)
        {
            return default(ConflictResult);
        }
    }
}

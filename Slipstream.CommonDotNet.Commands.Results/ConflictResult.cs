using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slipstream.CommonDotNet.Commands.Results
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

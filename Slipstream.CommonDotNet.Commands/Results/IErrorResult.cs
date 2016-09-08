using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slipstream.CommonDotNet.Commands.Results
{
    public interface IErrorResult<TException> : IResult
        where TException : Exception
    {
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Slipstream.CommonDotNet.Commands.Results;

namespace Slipstream.CommonDotNet.Commands
{
    public class CommandProcessorSuccessResult<TSuccessResult>
        where TSuccessResult : IResult
    {
        public bool Success { get; }
        public bool NotSuccess => !Success;

        public IResult Result { get; }

        public TSuccessResult SuccessResult
        {
            get
            {
                if (Success)
                {
                    return (TSuccessResult)Result;
                }
                else
                {
                    // TODO: what exception to use here?
                    throw new NotSupportedException();
                }
            }
        }

        public CommandProcessorSuccessResult(IResult result)
        {
            Result = result;
            Success = result is TSuccessResult;
        }

    }
}

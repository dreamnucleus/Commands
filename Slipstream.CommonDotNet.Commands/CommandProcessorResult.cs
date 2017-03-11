using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Slipstream.CommonDotNet.Commands.Results;

namespace Slipstream.CommonDotNet.Commands
{
    public class CommandProcessorSuccessResult<TSuccessResult>
    {
        public bool Success { get; }

        public readonly TSuccessResult result;
        public TSuccessResult Result
        {
            get
            {
                if (Success)
                {
                    return Result;
                }
                else
                {
                    // TODO: what exception to use here?
                    throw new NotSupportedException();
                }
            }
        }

        public readonly Exception exception;
        public Exception Exception
        {
            get
            {
                if (!Success)
                {
                    return Exception;
                }
                else
                {
                    // TODO: what exception to use here?
                    throw new NotSupportedException();
                }
            }
        }

        public CommandProcessorSuccessResult(TSuccessResult result)
        {
            this.result = result;
            Success = true;
        }

        public CommandProcessorSuccessResult(Exception exception)
        {
            this.exception = exception;
            Success = false;
        }
    }
}

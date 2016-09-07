using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slipstream.CommonDotNet.Commands.Playground
{
    public class HttpResult
    {
        public int StatusCode { get; }

        public HttpResult(int statusCode)
        {
            StatusCode = statusCode;
        }
    }
}

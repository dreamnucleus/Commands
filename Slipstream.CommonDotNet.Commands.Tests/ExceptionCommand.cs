using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Slipstream.CommonDotNet.Commands.Results;

namespace Slipstream.CommonDotNet.Commands.Tests
{
    public class ExceptionCommand : ISuccessResult<ExceptionCommand, Unit>
    {
    }

    public class ExceptionCommandHandler : IAsyncCommandHandler<ExceptionCommand, Unit>
    {
        public Task<Unit> ExecuteAsync(ExceptionCommand command)
        {
            throw new Exception();
        }
    }
}

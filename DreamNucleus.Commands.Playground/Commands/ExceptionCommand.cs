using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DreamNucleus.Commands.Results;

namespace DreamNucleus.Commands.Playground.Commands
{
    public class ExceptionCommand : ISuccessResult<ExceptionCommand, Unit>
    {
    }

    public class ExceptionCommandHandler : IAsyncCommandHandler<ExceptionCommand, Unit>
    {
        public Task<Unit> ExecuteAsync(ExceptionCommand command)
        {
            throw new Exception("6c32bde5-69d0-4a54-b95d-7af3435e572b");
        }
    }
}

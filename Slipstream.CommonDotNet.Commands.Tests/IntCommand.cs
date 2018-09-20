using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Slipstream.CommonDotNet.Commands.Results;

namespace Slipstream.CommonDotNet.Commands.Tests
{
    public class IntCommand : ISuccessResult<IntCommand, int>
    {
        public int Input { get; }

        public IntCommand(int input)
        {
            Input = input;
        }
    }

    public class IntCommandHandler : IAsyncCommandHandler<IntCommand, int>
    {
        public Task<int> ExecuteAsync(IntCommand command)
        {
            return Task.FromResult(command.Input);
        }
    }
}

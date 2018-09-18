using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Slipstream.CommonDotNet.Commands.Results;

namespace Slipstream.CommonDotNet.Commands.Tests
{
    public class TestCommand : ISuccessResult<TestCommand, int>
    {
        public int Input { get; }

        public TestCommand(int input)
        {
            Input = input;
        }
    }

    public class TestCommandHandler : IAsyncCommandHandler<TestCommand, int>
    {
        public Task<int> ExecuteAsync(TestCommand command)
        {
            return Task.FromResult(command.Input);
        }
    }
}

using Slipstream.CommonDotNet.Commands.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slipstream.CommonDotNet.Commands.Playground
{
    public class TestCommand : ISuccessResult<TestCommand, TestData>, INotFoundResult
    {
    }

    public class TestCommandHandler : IAsyncCommandHandler<TestCommand>
    {
        public async Task<IResult> ExecuteAsync(TestCommand command)
        {
            await Task.Delay(1);

            if (new Random().Next(0, 20) > 15)
            {
                return new NotFoundResult();
            }
            else
            {
                return new TestData();
            }
        }
    }

    public class TestData : IResult
    {
        public int Code { get; set; } = 200;
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Slipstream.CommonDotNet.Commands.Results;

namespace Slipstream.CommonDotNet.Commands.Playground
{
    public class FakeCommand : ISuccessResult<FakeCommand, FakeData>, INotFoundResult, IConflictResult
    {
        public FakeCommand(int number)
        {
            Number = number;
        }

        public int Number { get; set; }
    }

    public class FakeData : IResult
    {
        public int Id { get; set; }
    }

    public class FakeCommandHandler : IAsyncCommandHandler<FakeCommand>
    {
        public FakeCommandHandler()
        {
        }

        public async Task<IResult> ExecuteAsync(FakeCommand command)
        {
            await Task.Delay(12);
            await Task.Delay(12);

            if (command.Number == 0)
            {
                return new FakeData();
            }
            else if (command.Number == -1)
            {
                return TestException.Create();
            }
            else
            {
                return command.Number == 2 ? Test() : new NotFoundException();
            }
        }


        private IResult Test()
        {
            return new ConflictException();
        }
    }
}

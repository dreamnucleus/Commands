using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Slipstream.CommonDotNet.Commands.Results;

namespace Slipstream.CommonDotNet.Commands.Playground
{
    public abstract class MultipleCommand : ISuccessResult<MultipleCommand, MultipleData>
    {
        public int Id { get; set; }
    }

    public class MultipleData : IResult
    {
        public int Result { get; set; }
    }

    public class MultipleOneCommand : MultipleCommand
    {
        public MultipleOneCommand()
        {
            Id = 1;
        }
    }

    public class MultipleTwoCommand : MultipleCommand
    {
        public MultipleTwoCommand()
        {
            Id = 2;
        }
    }

    public class MultipleCommandHandler : IAsyncCommandHandler<MultipleCommand>
    {
        public Task<IResult> ExecuteAsync(MultipleCommand command)
        {
            return Task.FromResult<IResult>(new MultipleData
            {
                Result = command.Id * 10
            });

        }
    }
}

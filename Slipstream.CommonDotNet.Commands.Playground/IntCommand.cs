using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Slipstream.CommonDotNet.Commands.Results;

namespace Slipstream.CommonDotNet.Commands.Playground
{
    public class IntCommand : ISuccessResult<IntCommand, int>
    {
        public int Id { get; set; }
    }

    public class IntCommandHandler : IAsyncCommandHandler<IntCommand, int>
    {
        public async Task<int> ExecuteAsync(IntCommand command)
        {
            //return Task.FromResult<object>(command.Id);
            await Task.Delay(1);
            return 1;
        }
    }


    public class NoneCommand : ISuccessResult<NoneCommand>
    {
        public int Id { get; set; }
    }

    public class NoneCommandHandler : IAsyncCommandHandler<NoneCommand, Unit>
    {
        public async Task<Unit> ExecuteAsync(NoneCommand command)
        {
            await Task.Delay(1);

            return Unit.Value;
        }
    }
}

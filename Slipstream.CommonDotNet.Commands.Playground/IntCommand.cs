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

    public class IntCommandHandler : IAsyncCommandHandler<IntCommand>
    {
        public async Task<object> ExecuteAsync(IntCommand command)
        {
            //return Task.FromResult<object>(command.Id);
            await Task.Delay(1);
            return 1;
        }
    }
}

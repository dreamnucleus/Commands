using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DreamNucleus.Commands.Results;

namespace DreamNucleus.Commands.Tests.Common
{
    public class LongCommand : ISuccessResult<LongCommand, long>
    {
        public long Input { get; }

        public LongCommand(long input)
        {
            Input = input;
        }
    }

    public class LongCommandHandler : IAsyncCommandHandler<LongCommand, long>
    {
        public Task<long> ExecuteAsync(LongCommand command)
        {
            return Task.FromResult(command.Input);
        }
    }
}

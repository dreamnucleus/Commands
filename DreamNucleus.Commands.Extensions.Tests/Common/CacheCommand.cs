using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DreamNucleus.Commands.Extensions.Cache;
using DreamNucleus.Commands.Results;

namespace DreamNucleus.Commands.Extensions.Tests.Common
{
    [Cache]
    public class CacheCommand : ISuccessResult<CacheCommand, int>
    {
        public int Input { get; }

        public CacheCommand(int input)
        {
            Input = input;
        }
    }

    public class CacheCommandHandler : IAsyncCommandHandler<CacheCommand, int>
    {
        public Task<int> ExecuteAsync(CacheCommand command)
        {
            return Task.FromResult(command.Input);
        }
    }
}

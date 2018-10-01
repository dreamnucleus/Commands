﻿using System.Threading.Tasks;
using DreamNucleus.Commands.Extensions.Cache;
using DreamNucleus.Commands.Results;

// TODO: REMOVE
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

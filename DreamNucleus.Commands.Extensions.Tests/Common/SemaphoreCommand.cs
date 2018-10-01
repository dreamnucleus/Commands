﻿using System;
using System.Threading.Tasks;
using DreamNucleus.Commands.Extensions.Semaphore;
using DreamNucleus.Commands.Results;

// TODO: REMOVE
namespace DreamNucleus.Commands.Extensions.Tests.Common
{
    [Semaphore]
    public class SemaphoreCommand : ISuccessResult<SemaphoreCommand, Unit>
    {
        public Func<Task> Func { get; }

        public SemaphoreCommand(Func<Task> func)
        {
            Func = func;
        }
    }

    public class SemaphoreCommandHandler : IAsyncCommandHandler<SemaphoreCommand, Unit>
    {
        public async Task<Unit> ExecuteAsync(SemaphoreCommand command)
        {
            await command.Func();
            return Unit.Value;
        }
    }
}

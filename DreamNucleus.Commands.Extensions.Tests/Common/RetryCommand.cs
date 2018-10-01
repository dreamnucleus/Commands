using System;
using System.Threading.Tasks;
using DreamNucleus.Commands.Extensions.Retry;
using DreamNucleus.Commands.Results;

// TODO: REMOVE
namespace DreamNucleus.Commands.Extensions.Tests.Common
{
    [Retry(retries: 1)]
    public class RetryCommand : ISuccessResult<RetryCommand, Unit>
    {
        public int MaxTries { get; }
        public int Tries { get; set; } = 0;

        public RetryCommand(int maxTries)
        {
            MaxTries = maxTries;
        }
    }

    public class RetryCommandHandler : IAsyncCommandHandler<RetryCommand, Unit>
    {
        public Task<Unit> ExecuteAsync(RetryCommand command)
        {
            if (command.Tries <= command.MaxTries)
            {
                command.Tries++;
                throw new Exception();
            }

            return Task.FromResult(Unit.Value);
        }
    }
}

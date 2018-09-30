using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DreamNucleus.Commands.Extensions.Checkpoints;
using DreamNucleus.Commands.Extensions.Tests.Common;
using DreamNucleus.Commands.Results;

namespace DreamNucleus.Commands.Extensions.Tests.Common
{
    public class CheckpointCommand : ISuccessResult<CheckpointCommand, Unit>
    {
        public List<string> Emails { get; }

        public CheckpointCommand(List<string> emails)
        {
            Emails = emails;
        }
    }

    public class CheckpointCommandHandler : IAsyncCommandHandler<CheckpointCommand, Unit>
    {
        private readonly Checkpoint _checkpoint;

        public CheckpointCommandHandler(Checkpoint checkpoint)
        {
            _checkpoint = checkpoint;
        }

        public async Task<Unit> ExecuteAsync(CheckpointCommand command)
        {
            Task SendAsync(string email)
            {
                return Task.CompletedTask;
            }

            await _checkpoint.RunAsync(async () => await SendAsync("admin@test.com"), "unique-send-email");

            //foreach (var email in command.Emails)
            //{
            //    await SendAsync(email);
            //    await _checkpointManager.CheckPointAsync();
            //}

            await _checkpoint.ForEachAsync(command.Emails, async e => await SendAsync(e), "unique-send-emails");

            return Unit.Value;
        }
    }
}

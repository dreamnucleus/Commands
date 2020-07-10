using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DreamNucleus.Commands.Results;

namespace DreamNucleus.Commands.Playground
{
    public class LongCommand : ISuccessResult<LongCommand, long>
    {
        public int Id { get; set; }

        public LongCommand(int id)
        {
            Id = id;
        }

        public LongCommand()
        {
        }
    }

    public class LongCommandHandler : IAsyncCommandHandler<LongCommand, long>
    {
        public async Task<long> ExecuteAsync(LongCommand command)
        {
            await Task.Delay(1);
            return 1;
        }
    }
}

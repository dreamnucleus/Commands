using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DreamNucleus.Commands.Results;

namespace DreamNucleus.Commands.Playground.Commands
{
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

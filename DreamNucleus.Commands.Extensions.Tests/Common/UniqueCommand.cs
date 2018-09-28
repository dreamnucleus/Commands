using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DreamNucleus.Commands.Extensions.Unique;
using DreamNucleus.Commands.Results;

namespace DreamNucleus.Commands.Extensions.Tests.Common
{
    public class UniqueCommand : ISuccessResult<UniqueCommand, Unit>, IUniqueCommand
    {
        public string UniqueCommandId { get; }

        public UniqueCommand(string uniqueCommandId)
        {
            UniqueCommandId = uniqueCommandId;
        }
    }

    public class UniqueCommandHandler : IAsyncCommandHandler<UniqueCommand, Unit>
    {
        public async Task<Unit> ExecuteAsync(UniqueCommand command)
        {
            return Unit.Value;
        }
    }
}

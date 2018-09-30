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
        public Task<Unit> ExecuteAsync(UniqueCommand command)
        {
            return Task.FromResult(Unit.Value);
        }
    }
}

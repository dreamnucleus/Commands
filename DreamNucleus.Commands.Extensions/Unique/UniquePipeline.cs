using System.Threading;
using System.Threading.Tasks;
using DreamNucleus.Commands.Extensions.Results;
using DreamNucleus.Commands.Pipelines;

namespace DreamNucleus.Commands.Extensions.Unique
{
    public class UniquePipeline : Pipeline
    {
        private readonly IUniqueManager _uniqueManager;

        public UniquePipeline(IUniqueManager uniqueManager)
        {
            _uniqueManager = uniqueManager;
        }

        public override async Task ExecutingAsync(IAsyncCommand command)
        {
            // ReSharper disable once SuspiciousTypeConversion.Global
            if (command is IUniqueCommand uniqueCommand)
            {
                if (!await _uniqueManager.CheckAsync(uniqueCommand.UniqueCommandId, CancellationToken.None).ConfigureAwait(false))
                {
                    throw new ConflictException();
                }
            }
        }
    }
}

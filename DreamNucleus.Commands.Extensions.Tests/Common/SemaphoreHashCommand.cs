using System;
using System.Globalization;
using System.Threading.Tasks;
using DreamNucleus.Commands.Extensions.Semaphore;
using DreamNucleus.Commands.Results;

namespace DreamNucleus.Commands.Extensions.Tests.Common
{
    [Semaphore]
    public class SemaphoreHashCommand : ISuccessResult<SemaphoreHashCommand, int>, ISemaphoreHash
    {
        public int Id { get; }
        public Func<Task> Func { get; }

        public SemaphoreHashCommand(int id, Func<Task> func)
        {
            Id = id;
            Func = func;
        }

        public string SemaphoreHash()
        {
            return $"{GetType().GetHashCode().ToString(CultureInfo.InvariantCulture)}-{Id}";
        }
    }

    public class SemaphoreHashCommandHandler : IAsyncCommandHandler<SemaphoreHashCommand, int>
    {
        public async Task<int> ExecuteAsync(SemaphoreHashCommand command)
        {
            await command.Func();
            return command.Id;
        }
    }
}

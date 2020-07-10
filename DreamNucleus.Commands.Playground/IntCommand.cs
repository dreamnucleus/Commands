using System.Threading.Tasks;
using DreamNucleus.Commands.Results;

namespace DreamNucleus.Commands.Playground
{
    public class IntCommand : ISuccessResult<IntCommand, int>
    {
        public int Id { get; set; }

        public IntCommand(int id)
        {
            Id = id;
        }

        public IntCommand()
        {
        }
    }

    public class IntCommandHandler : IAsyncCommandHandler<IntCommand, int>
    {
        public async Task<int> ExecuteAsync(IntCommand command)
        {
            //return Task.FromResult<object>(command.Id);
            await Task.Delay(1);
            return 1;
        }
    }
}

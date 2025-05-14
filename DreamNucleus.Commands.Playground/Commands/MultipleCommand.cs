using System.Threading.Tasks;
using DreamNucleus.Commands.Results;

namespace DreamNucleus.Commands.Playground.Commands
{
    public abstract class MultipleCommand : ISuccessResult<MultipleCommand, MultipleData>
    {
        public int Id { get; set; }
    }

    public class MultipleData
    {
        public int Result { get; set; }
    }

    public class MultipleOneCommand : MultipleCommand
    {
        public MultipleOneCommand()
        {
            Id = 1;
        }
    }

    public class MultipleTwoCommand : MultipleCommand
    {
        public MultipleTwoCommand()
        {
            Id = 2;
        }
    }

    public class MultipleCommandHandler : IAsyncCommandHandler<MultipleCommand, MultipleData>
    {
        public Task<MultipleData> ExecuteAsync(MultipleCommand command)
        {
            return Task.FromResult(new MultipleData
            {
                Result = command.Id * 10
            });

        }
    }
}

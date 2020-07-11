using System.Threading.Tasks;
using DreamNucleus.Commands.Results;

namespace DreamNucleus.Commands.Tests.Common
{
    public class ExceptionCommand : ISuccessResult<ExceptionCommand, Unit>
    {
    }

    public class ExceptionCommandHandler : IAsyncCommandHandler<ExceptionCommand, Unit>
    {
        public Task<Unit> ExecuteAsync(ExceptionCommand command)
        {
            throw new TestException();
        }
    }
}

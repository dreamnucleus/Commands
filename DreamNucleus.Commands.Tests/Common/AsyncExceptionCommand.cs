using System.Threading.Tasks;
using DreamNucleus.Commands.Results;

namespace DreamNucleus.Commands.Tests.Common
{
    public class AsyncExceptionCommand : ISuccessResult<AsyncExceptionCommand, Unit>
    {
    }

    public class AsyncExceptionCommandHandler : IAsyncCommandHandler<AsyncExceptionCommand, Unit>
    {
        public async Task<Unit> ExecuteAsync(AsyncExceptionCommand command)
        {
            await Task.Delay(1);
            throw new TestException();
        }
    }
}

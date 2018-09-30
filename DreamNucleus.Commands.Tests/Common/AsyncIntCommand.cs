using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DreamNucleus.Commands.Results;

namespace DreamNucleus.Commands.Tests.Common
{
    public class AsyncIntCommand : ISuccessResult<AsyncIntCommand, int>
    {
        public int Input { get; }

        public AsyncIntCommand(int input)
        {
            Input = input;
        }
    }

    public class AsyncIntCommandHandler : IAsyncCommandHandler<AsyncIntCommand, int>
    {
        public async Task<int> ExecuteAsync(AsyncIntCommand command)
        {
            await Task.Delay(1);
            return command.Input;
        }
    }
}

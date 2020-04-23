using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DreamNucleus.Commands.Results;

namespace DreamNucleus.Commands.Tests.Common
{
    public class GenericCommand<TReturn> : ISuccessResult<GenericCommand<TReturn>, TReturn>
    {
        public TReturn Input { get; set; }

        public GenericCommand(TReturn input)
        {
            Input = input;
        }
    }

    public class GenericCommandHandler<TReturn> : IAsyncCommandHandler<GenericCommand<TReturn>, TReturn>
    {
        public GenericCommandHandler()
        {
        }

        public Task<TReturn> ExecuteAsync(GenericCommand<TReturn> command)
        {
            return Task.FromResult(command.Input);
        }
    }
}

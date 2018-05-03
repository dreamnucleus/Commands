using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Slipstream.CommonDotNet.Commands.Results;

namespace Slipstream.CommonDotNet.Commands.Playground
{
    public class GenericCommand<TEnumeration> : ISuccessResult<GenericCommand<TEnumeration>, TEnumeration>
        where TEnumeration : class, new()
    {
    }

    public class GenericCommandHandler<TEnumeration> : IAsyncCommandHandler<GenericCommand<TEnumeration>, TEnumeration>
        where TEnumeration : class, new()
    {
        public Task<TEnumeration> ExecuteAsync(GenericCommand<TEnumeration> genericCommand)
        {
            return Task.FromResult(new TEnumeration());
        }
    }
}

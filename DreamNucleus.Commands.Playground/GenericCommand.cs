using System.Threading.Tasks;
using DreamNucleus.Commands.Results;

namespace DreamNucleus.Commands.Playground
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

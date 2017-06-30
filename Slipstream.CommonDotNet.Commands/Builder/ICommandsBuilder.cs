using Slipstream.CommonDotNet.Commands.Pipelines;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slipstream.CommonDotNet.Commands.Builder
{
    // TODO: put 
    public interface ICommandsBuilder
    {
        IReadOnlyCollection<Type> Pipelines { get; }

        ICommandsBuilder Use<TPipeline>()
            where TPipeline : Pipeline;
    }
}

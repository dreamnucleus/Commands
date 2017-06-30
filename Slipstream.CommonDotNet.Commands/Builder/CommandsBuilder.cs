using Slipstream.CommonDotNet.Commands.Pipelines;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Slipstream.CommonDotNet.Commands.Builder
{
    public class CommandsBuilder : ICommandsBuilder
    {
        private readonly List<Type> piplines = new List<Type>();
        public IReadOnlyCollection<Type> Pipelines => piplines;

        public virtual ICommandsBuilder Use<TPipeline>() where TPipeline : Pipeline
        {
            piplines.Add(typeof(TPipeline));
            return this;
        }
    }
}

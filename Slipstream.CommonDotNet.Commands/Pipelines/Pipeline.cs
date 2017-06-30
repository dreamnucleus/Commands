using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slipstream.CommonDotNet.Commands.Pipelines
{
    public abstract class Pipeline
    {

        public virtual Task Incoming()
        {
            return Task.FromResult(0);
        }

        public virtual Task Outgoing()
        {
            return Task.FromResult(0);
        }
    }
}

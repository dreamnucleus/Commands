using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DreamNucleus.Commands.Results;

namespace DreamNucleus.Commands.Tests.Common
{
    public class ExecutingPipeline : Pipelines.ExecutingPipeline
    {
        public ExecutingPipeline(Pipelines.ExecutingPipeline nextExecutingPipeline)
            : base(nextExecutingPipeline)
        {
        }

        public override Task<TSuccessResult> ExecutingAsync<TCommand, TSuccessResult>(ISuccessResult<TCommand, TSuccessResult> command)
        {
            Console.WriteLine("ExecutingAsync");
            return base.ExecutingAsync(command);
        }
    }
}

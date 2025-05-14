using System;
using System.Threading.Tasks;
using DreamNucleus.Commands.Results;

namespace DreamNucleus.Commands.Tests.Common
{
    public class PrePipeline : Pipelines.PrePipeline
    {
        public PrePipeline(Pipelines.PrePipeline nextPrePipeline)
            : base(nextPrePipeline)
        {
        }

        public override Task PreAsync<TCommand, TSuccessResult>(ISuccessResult<TCommand, TSuccessResult> command)
        {
            Console.WriteLine("PreAsync");
            return base.PreAsync(command);
        }
    }
}

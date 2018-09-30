using System;
using System.Threading.Tasks;
using DreamNucleus.Commands.Results;

namespace DreamNucleus.Commands.Tests.Common
{
    public class ExecutedPipeline : Pipelines.ExecutedPipeline
    {
        public ExecutedPipeline(Pipelines.ExecutedPipeline nextExecutedPipeline)
            : base(nextExecutedPipeline)
        {
        }

        public override Task<TSuccessResult> ExecutedAsync<TCommand, TSuccessResult>(ISuccessResult<TCommand, TSuccessResult> command, TSuccessResult result)
        {
            Console.WriteLine("ExecutedAsync");
            return base.ExecutedAsync(command, result);
        }
    }
}

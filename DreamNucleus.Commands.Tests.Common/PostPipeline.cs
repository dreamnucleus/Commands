using System;
using System.Threading.Tasks;
using DreamNucleus.Commands.Results;

namespace DreamNucleus.Commands.Tests.Common
{
    public class PostPipeline : Pipelines.PostPipeline
    {
        public PostPipeline(Pipelines.PostPipeline nextPostPipeline)
            : base(nextPostPipeline)
        {
        }

        public override Task PostAsync<TCommand, TSuccessResult>(ISuccessResult<TCommand, TSuccessResult> command)
        {
            Console.WriteLine("PostAsync");
            return base.PostAsync(command);
        }
    }
}

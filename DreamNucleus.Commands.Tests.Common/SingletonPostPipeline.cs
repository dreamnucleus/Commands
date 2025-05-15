using System;
using System.Threading.Tasks;
using DreamNucleus.Commands.Results;

namespace DreamNucleus.Commands.Tests.Common
{
    public class SingletonPostPipeline : Pipelines.PostPipeline
    {
        public SingletonPostPipeline(Pipelines.PostPipeline nextPostPipeline)
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DreamNucleus.Commands.Results;

namespace DreamNucleus.Commands.Tests.Common
{
    public class ExceptionPipeline : Pipelines.ExceptionPipeline
    {
        public ExceptionPipeline(Pipelines.ExceptionPipeline nextExceptionPipeline)
            : base(nextExceptionPipeline)
        {
        }

        public override Task<object> ExceptionAsync<TCommand, TSuccessResult>(ISuccessResult<TCommand, TSuccessResult> command, Exception exception)
        {
            Console.WriteLine("ExceptionAsync");
            return base.ExceptionAsync(command, exception);
        }
    }
}

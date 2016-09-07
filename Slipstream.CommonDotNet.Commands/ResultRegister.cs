using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Slipstream.CommonDotNet.Commands.Results;

namespace Slipstream.CommonDotNet.Commands
{
    public class ResultRegister<TCommand, TSuccessResult, TReturn>
    {
        // TODO: defaults can be passed and then overridden
        private readonly Dictionary<Type, Func<IResult, TReturn>> resultParsers = new Dictionary<Type, Func<IResult, TReturn>>();

        private readonly ISuccessResult<TCommand, TSuccessResult> command;
        private readonly ILifetimeScopeDependencyService dependencyService;

        public ResultRegister(ISuccessResult<TCommand, TSuccessResult> command, ILifetimeScopeDependencyService dependencyService)
        {
            this.command = command;
            this.dependencyService = dependencyService;
        }


        public ResultParser<TCommand, TSuccessResult, TReturn, TWhen> When<TWhen>(Func<TCommand, TWhen> action)
            where TWhen : IResult
        {
            return new ResultParser<TCommand, TSuccessResult, TReturn, TWhen>(this, func =>
            {
                resultParsers.Add(typeof(TWhen), func);
            });
        }
        
        // TODO: this should be done in another class
        public async Task<TReturn> ExecuteAsync()
        {
            var processor = new CommandProcessor(dependencyService);

            var result = await processor.ProcessAsync(command);

            return resultParsers[result.GetType()](result);
        }

        public Task<TSuccessResult> ExecuteSuccessAsync()
        {
            throw new NotImplementedException();
        }
    }
}

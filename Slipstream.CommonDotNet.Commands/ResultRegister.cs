using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Slipstream.CommonDotNet.Commands.Results;

namespace Slipstream.CommonDotNet.Commands
{
    public class ResultRegister<TReturn>
    {
        private readonly Dictionary<Type, Func<IResult, TReturn>> resultParsers = new Dictionary<Type, Func<IResult, TReturn>>();

        public ResultRegister()
        {
        }

        public ResultParser<TReturn, TWhen> When<TWhen>()
            where TWhen : IResult
        {
            return new ResultParser<TReturn, TWhen>(this, func =>
            {
                resultParsers.Add(typeof(TWhen), func);
            });
        }

        public Dictionary<Type, Func<IResult, TReturn>> Emit()
        {
            return resultParsers;
        }
    }
}

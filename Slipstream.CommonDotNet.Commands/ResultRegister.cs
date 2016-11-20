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
        private readonly Dictionary<Type, Func<object, TReturn>> resultParsers = new Dictionary<Type, Func<object, TReturn>>();

        public ResultRegister()
        {
        }

        public ResultParser<TReturn, TWhen> When<TWhen>()
        {
            return new ResultParser<TReturn, TWhen>(this, func =>
            {
                resultParsers.Add(typeof(TWhen), func);
            });
        }

        // TODO: rename
        public Dictionary<Type, Func<object, TReturn>> Emit()
        {
            return resultParsers;
        }
    }
}

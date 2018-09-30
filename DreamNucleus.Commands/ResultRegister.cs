using System;
using System.Collections.Generic;

namespace DreamNucleus.Commands
{
    public class ResultRegister<TReturn>
    {
        private readonly Dictionary<Type, Func<object, TReturn>> _resultParsers = new Dictionary<Type, Func<object, TReturn>>();

        public ResultRegister()
        {
        }

        public ResultParser<TReturn, TWhen> When<TWhen>()
        {
            return new ResultParser<TReturn, TWhen>(this, func =>
            {
                _resultParsers.Add(typeof(TWhen), func);
            });
        }

        public Dictionary<Type, Func<object, TReturn>> Emit()
        {
            return _resultParsers;
        }
    }
}

using System;

namespace DreamNucleus.Commands
{
    public class ResultParser<TCommand, TSuccessResult, TReturn, TWhen>
        where TCommand : IAsyncCommand
    {
        private readonly ResultRegisterProcessor<TCommand, TSuccessResult, TReturn> _resultRegisterProcessor;
        private readonly Action<Func<object, TReturn>> _registerResult;

        public ResultParser(ResultRegisterProcessor<TCommand, TSuccessResult, TReturn> resultRegisterProcessor, Action<Func<object, TReturn>> registerResult)
        {
            _resultRegisterProcessor = resultRegisterProcessor;
            _registerResult = registerResult;
        }

        public ResultRegisterProcessor<TCommand, TSuccessResult, TReturn> Return(Func<TWhen, TReturn> resultParser)
        {
            _registerResult(result => resultParser((TWhen) result));
            return _resultRegisterProcessor;
        }
    }

    public class ResultParser<TReturn, TWhen>
    {
        private readonly ResultRegister<TReturn> _resultRegisterProcessor;
        private readonly Action<Func<object, TReturn>> _registerResult;

        public ResultParser(ResultRegister<TReturn> resultRegisterProcessor, Action<Func<object, TReturn>> registerResult)
        {
            _resultRegisterProcessor = resultRegisterProcessor;
            _registerResult = registerResult;
        }

        public ResultRegister<TReturn> Return(Func<TWhen, TReturn> resultParser)
        {
            _registerResult(result => resultParser((TWhen)result));
            return _resultRegisterProcessor;
        }
    }
}

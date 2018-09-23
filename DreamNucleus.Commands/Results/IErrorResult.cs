using System;

namespace DreamNucleus.Commands.Results
{
    public interface IErrorResult<TException>
        where TException : Exception
    {
    }
}

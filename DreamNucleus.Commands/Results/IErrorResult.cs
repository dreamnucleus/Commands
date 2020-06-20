using System;
using System.Diagnostics.CodeAnalysis;

namespace DreamNucleus.Commands.Results
{
    [SuppressMessage("Design", "CA1040:Avoid empty interfaces")]
    public interface IErrorResult<TException>
        where TException : Exception
    {
    }
}

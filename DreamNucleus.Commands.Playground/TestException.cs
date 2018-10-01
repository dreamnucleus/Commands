using System;
using DreamNucleus.Commands.Results;

namespace DreamNucleus.Commands.Playground
{
    public interface ITestResult : IErrorResult<TestException>
    {
    }

    public class TestException : Exception
    {
        public static TestException Create()
        {
            return new TestException();
        }

        public object Test()
        {
            return new object();
        }
    }

    public static class TestResultExtensions
    {
        public static TestException Conflict(this ITestResult result)
        {
            return default;
        }
    }
}

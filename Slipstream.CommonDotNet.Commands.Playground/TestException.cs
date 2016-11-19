using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Slipstream.CommonDotNet.Commands.Results;

namespace Slipstream.CommonDotNet.Commands.Playground
{
    public interface ITestResult : IErrorResult<TestException>
    {
    }

    public class TestException : Exception, IResult
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
            return default(TestException);
        }
    }
}

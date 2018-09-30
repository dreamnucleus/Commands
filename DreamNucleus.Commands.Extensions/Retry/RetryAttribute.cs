using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DreamNucleus.Commands.Extensions.Retry
{
    // TODO: for what errors, delay, circuit breaker..etc
    [AttributeUsage(AttributeTargets.Class)]
    public class RetryAttribute : Attribute
    {
        public int Retries { get; }

        public RetryAttribute(int retries)
        {
            Retries = retries;
        }
    }
}

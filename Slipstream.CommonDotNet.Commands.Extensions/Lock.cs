using System;

namespace Slipstream.CommonDotNet.Commands.Extensions
{
    public class Lock
    {
        public string LeaseId { get; }
        public TimeSpan LeaseTime { get; }
        public DateTimeOffset Created { get; set; }

        public Lock(string leaseId, TimeSpan leaseTime, DateTimeOffset created)
        {
            LeaseId = leaseId;
            LeaseTime = leaseTime;
            Created = created;
        }
    }
}

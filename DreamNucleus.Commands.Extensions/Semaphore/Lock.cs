using System;

namespace DreamNucleus.Commands.Extensions.Semaphore
{
    public sealed class Lock
    {
        public string LeaseId { get; }
        public TimeSpan LeaseTime { get; }
        public DateTimeOffset Created { get; }

        public Lock(string leaseId, TimeSpan leaseTime, DateTimeOffset created)
        {
            LeaseId = leaseId;
            LeaseTime = leaseTime;
            Created = created;
        }
    }
}

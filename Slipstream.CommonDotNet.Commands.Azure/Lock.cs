using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slipstream.CommonDotNet.Commands.Azure
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

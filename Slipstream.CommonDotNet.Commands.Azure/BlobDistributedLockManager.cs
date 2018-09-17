using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace Slipstream.CommonDotNet.Commands.Azure
{
    public class BlobDistributedLockManager : ILockManager
    {
        public TimeSpan LeaseTime { get; }

        private readonly CloudBlobContainer cloudBlobContainer;

        public BlobDistributedLockManager(CloudBlobContainer cloudBlobContainer, TimeSpan leaseTime)
        {
            if (cloudBlobContainer == null)
            {
                throw new ArgumentNullException(nameof(cloudBlobContainer));
            }
            if (leaseTime < TimeSpan.FromSeconds(15) || leaseTime > TimeSpan.FromSeconds(60))
            {
                throw new ArgumentOutOfRangeException(nameof(leaseTime), "Lease time must be between 15 and 60 seconds");
            }

            LeaseTime = leaseTime;

            this.cloudBlobContainer = cloudBlobContainer;
        }

        public async Task<Lock> AcquireAsync(string resourceId, CancellationToken cancellationToken)
        {
            var cloudBlockBlob = cloudBlobContainer.GetBlockBlobReference(resourceId);
            if (!await cloudBlockBlob.ExistsAsync(null, null, cancellationToken))
            {
                await cloudBlockBlob.UploadFromByteArrayAsync(new byte[0], 0, 0, null, null, null, cancellationToken);
            }
            var leaseId = await cloudBlockBlob.AcquireLeaseAsync(LeaseTime, null, null, null, null, cancellationToken);

            return new Lock(leaseId, LeaseTime, DateTimeOffset.UtcNow);
        }

        public async Task<Lock> RenewAsync(string resourceId, string leaseId, CancellationToken cancellationToken)
        {
            var cloudBlockBlob = cloudBlobContainer.GetBlockBlobReference(resourceId);
            await cloudBlockBlob.RenewLeaseAsync(AccessCondition.GenerateLeaseCondition(leaseId), null, null, cancellationToken);

            return new Lock(leaseId, LeaseTime, DateTimeOffset.UtcNow);
        }

        public async Task ReleaseAsync(string resourceId, string leaseId, CancellationToken cancellationToken)
        {
            var cloudBlockBlob = cloudBlobContainer.GetBlockBlobReference(resourceId);
            await cloudBlockBlob.ReleaseLeaseAsync(AccessCondition.GenerateLeaseCondition(leaseId), null, null, cancellationToken);
        }
    }
}

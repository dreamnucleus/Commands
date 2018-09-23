using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace DreamNucleus.Commands.Extensions.Azure
{
    public sealed class BlobDistributedLockManager : ILockManager
    {
        public TimeSpan LeaseTime { get; }

        private readonly CloudBlobContainer _cloudBlobContainer;
#if NET45
        private static readonly byte[] EmptyByteArray = new byte[0];
#endif
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

            _cloudBlobContainer = cloudBlobContainer;
            LeaseTime = leaseTime;
        }

        public async Task<Lock> AcquireAsync(string resourceId, CancellationToken cancellationToken)
        {
            var cloudBlockBlob = _cloudBlobContainer.GetBlockBlobReference(resourceId);
            if (!await cloudBlockBlob.ExistsAsync(null, null, cancellationToken).ConfigureAwait(false))
            {
#if NET45
                await cloudBlockBlob.UploadFromByteArrayAsync(EmptyByteArray, 0, 0, null, null, null, cancellationToken).ConfigureAwait(false);
#else
                await cloudBlockBlob.UploadFromByteArrayAsync(Array.Empty<byte>(), 0, 0, null, null, null, cancellationToken).ConfigureAwait(false);
#endif
            }
            var leaseId = await cloudBlockBlob.AcquireLeaseAsync(LeaseTime, null, null, null, null, cancellationToken).ConfigureAwait(false);

            return new Lock(leaseId, LeaseTime, DateTimeOffset.UtcNow);
        }

        public async Task<Lock> RenewAsync(string resourceId, string leaseId, CancellationToken cancellationToken)
        {
            var cloudBlockBlob = _cloudBlobContainer.GetBlockBlobReference(resourceId);
            await cloudBlockBlob.RenewLeaseAsync(AccessCondition.GenerateLeaseCondition(leaseId), null, null, cancellationToken).ConfigureAwait(false);

            return new Lock(leaseId, LeaseTime, DateTimeOffset.UtcNow);
        }

        public async Task ReleaseAsync(string resourceId, string leaseId, CancellationToken cancellationToken)
        {
            var cloudBlockBlob = _cloudBlobContainer.GetBlockBlobReference(resourceId);
            await cloudBlockBlob.ReleaseLeaseAsync(AccessCondition.GenerateLeaseCondition(leaseId), null, null, cancellationToken).ConfigureAwait(false);
        }
    }
}

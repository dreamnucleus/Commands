using System;
using System.Threading.Tasks;
using DreamNucleus.Commands.Pipelines;
using DreamNucleus.Commands.Results;

// TODO: REMOVE
namespace DreamNucleus.Commands.Extensions.Cache
{
    public class CacheExecutedPipeline : ExecutedPipeline
    {
        private readonly ICacheManager _cacheManager;

        public CacheExecutedPipeline(ICacheManager cacheManager, ExecutedPipeline nextExecutedPipeline)
            : base(nextExecutedPipeline)
        {
            _cacheManager = cacheManager;
        }

        public override async Task<TSuccessResult> ExecutedAsync<TCommand, TSuccessResult>(ISuccessResult<TCommand, TSuccessResult> command, TSuccessResult result)
        {
            var cacheAttribute = (CacheAttribute)Attribute.GetCustomAttribute(command.GetType(), typeof(CacheAttribute));

            if (cacheAttribute != null)
            {
                var cacheId = command.GetType().GetHashCode(); // HACK: just playing
                if (!await _cacheManager.ContainsAsync(cacheId).ConfigureAwait(false))
                {
                    await _cacheManager.AddAsync(cacheId, result).ConfigureAwait(false);
                }
            }

            return await base.ExecutedAsync(command, result).ConfigureAwait(false);
        }
    }
}

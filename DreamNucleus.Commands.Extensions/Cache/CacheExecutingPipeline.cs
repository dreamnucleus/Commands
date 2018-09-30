using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using DreamNucleus.Commands.Pipelines;
using DreamNucleus.Commands.Results;

namespace DreamNucleus.Commands.Extensions.Cache
{
    public class CacheExecutingPipeline : ExecutingPipeline
    {
        private readonly ICacheManager _cacheManager;

        public CacheExecutingPipeline(ICacheManager cacheManager, ExecutingPipeline nextIncomingPipeline)
            : base(nextIncomingPipeline)
        {
            _cacheManager = cacheManager;
        }

        public override async Task<TSuccessResult> ExecutingAsync<TCommand, TSuccessResult>(ISuccessResult<TCommand, TSuccessResult> command)
        {
            var cacheAttribute = (CacheAttribute)Attribute.GetCustomAttribute(command.GetType(), typeof(CacheAttribute));

            if (cacheAttribute != null)
            {
                var cacheId = command.GetType().GetHashCode(); // HACK: just playing
                if (await _cacheManager.ContainsAsync(cacheId).ConfigureAwait(false))
                {
                    return (TSuccessResult)await _cacheManager.GetAsync(cacheId).ConfigureAwait(false);
                }
            }

            return await base.ExecutingAsync(command).ConfigureAwait(false);
        }

    }
}

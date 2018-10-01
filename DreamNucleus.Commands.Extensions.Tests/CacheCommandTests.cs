using System.Threading.Tasks;
using Autofac;
using DreamNucleus.Commands.Extensions.Cache;
using DreamNucleus.Commands.Extensions.Tests.Common;
using DreamNucleus.Commands.Results;
using Xunit;

// TODO: REMOVE
namespace DreamNucleus.Commands.Extensions.Tests
{
    public class CacheCommandTests
    {
        private static ICommandProcessor CommandProcessor()
        {
            return Helpers.CreateDefaultCommandProcessor(
                containerBuilder =>
                {
                    containerBuilder.RegisterType<FakeCacheManager>().As<ICacheManager>().SingleInstance();
                    containerBuilder.RegisterType<CacheCommandHandler>().As<IAsyncCommandHandler<CacheCommand, int>>();
                },
                commandsBuilder =>
                {
                    commandsBuilder.Use<CacheExecutingPipeline>();
                    commandsBuilder.Use<CacheExecutedPipeline>();
                });
        }

        [Fact]
        public async Task ProcessAsync_TwoDifferentReturnCommands_ReturnsCachedResult()
        {
            var commandProcessor = CommandProcessor();

            const int id1 = 1;
            const int id2 = 2;

            var result1 = await commandProcessor.ProcessAsync(new CacheCommand(id1));
            var result2 = await commandProcessor.ProcessAsync(new CacheCommand(id2));

            Assert.Equal(id1, result1);
            Assert.Equal(id1, result2);
        }
    }
}

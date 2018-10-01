using System.Threading.Tasks;
using DreamNucleus.Commands.Extensions.Tests.Common;
using Xunit;

// TODO: REMOVE
namespace DreamNucleus.Commands.Extensions.Tests
{
    public class CacheCommandTests
    {
        [Fact]
        public async Task ProcessAsync_TwoDifferentReturnCommands_ReturnsCachedResult()
        {
            var commandProcessor = Helpers.CreateDefaultCommandProcessor();

            const int id1 = 1;
            const int id2 = 2;

            var result1 = await commandProcessor.ProcessAsync(new CacheCommand(id1));
            var result2 = await commandProcessor.ProcessAsync(new CacheCommand(id2));

            Assert.Equal(id1, result1);
            Assert.Equal(id1, result2);
        }
    }
}

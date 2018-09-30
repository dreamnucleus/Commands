using System.Threading.Tasks;
using DreamNucleus.Commands.Extensions.Results;
using DreamNucleus.Commands.Extensions.Tests.Common;
using Xunit;

namespace DreamNucleus.Commands.Extensions.Tests
{
    public class UniqueCommandTests
    {
        [Fact]
        public async Task ProcessAsync_TwoUniqueCommands_NoException()
        {
            var commandProcessor = Helpers.CreateDefaultCommandProcessor();

            const string id1 = "1";
            const string id2 = "2";

            await commandProcessor.ProcessAsync(new UniqueCommand(id1));
            await commandProcessor.ProcessAsync(new UniqueCommand(id2));
            Assert.True(true);
        }

        [Fact]
        public async Task ProcessAsync_TwoNonUniqueCommands_ThrowsException()
        {
            var commandProcessor = Helpers.CreateDefaultCommandProcessor();

            const string id = "1";

            await commandProcessor.ProcessAsync(new UniqueCommand(id));
            await Assert.ThrowsAsync<ConflictException>(async () => await commandProcessor.ProcessAsync(new UniqueCommand(id)));
        }
    }
}

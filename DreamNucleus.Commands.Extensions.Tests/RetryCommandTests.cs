using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DreamNucleus.Commands.Extensions.Tests.Common;
using DreamNucleus.Commands.Results;
using Xunit;

namespace DreamNucleus.Commands.Extensions.Tests
{
    public class RetryCommandTests
    {
        [Fact]
        public async Task ProcessAsync_CommandRetriesOnce_ReturnsResult()
        {
            var commandProcessor = Helpers.CreateDefaultCommandProcessor();

            var result = await commandProcessor.ProcessAsync(new RetryCommand(1));

            Assert.Equal(Unit.Value, result);
        }

        [Fact]
        public async Task ProcessAsync_CommandRetriesOnceOnly_ThrowsException()
        {
            var commandProcessor = Helpers.CreateDefaultCommandProcessor();

            await Assert.ThrowsAsync<Exception>(async () => await commandProcessor.ProcessAsync(new RetryCommand(2)));
        }
    }
}

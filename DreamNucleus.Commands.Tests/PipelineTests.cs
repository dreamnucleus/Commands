using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DreamNucleus.Commands.Tests.Common;
using Xunit;

namespace DreamNucleus.Commands.Tests
{
    public class PipelineTests
    {
        [Fact]
        public async Task ProcessAsync_AsyncIntReturn_ReturnsInt()
        {
            var commandProcessor = Helpers.CreateDefaultCommandProcessor();

            const int input = 2;
            var result = await commandProcessor.ProcessAsync(new AsyncIntCommand(input));
            Assert.Equal(input, result);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DreamNucleus.Commands.Results;
using DreamNucleus.Commands.Tests.Common;
using Xunit;

namespace DreamNucleus.Commands.Tests
{
    // TODO: defaults
    public class ResultProcessorTests
    {
        [Fact]
        public async Task ExecuteAsync_AsyncIntReturn_ReturnsInt()
        {
            var resultProcessor = Helpers.CreateDefaultResultProcessor();

            const int input = 2;
            var result = await resultProcessor.For(new AsyncIntCommand(input))
                .When(o => o.Success()).Return(r => new ObjectResult(r))
                .ExecuteAsync();
            Assert.Equal(input, (int)result.Result);
        }
    }
}

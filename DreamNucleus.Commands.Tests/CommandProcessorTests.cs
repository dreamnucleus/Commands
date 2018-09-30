using System;
using System.Threading.Tasks;
using Autofac;
using DreamNucleus.Commands.Autofac;
using DreamNucleus.Commands.Results;
using DreamNucleus.Commands.Tests.Common;
using Xunit;

namespace DreamNucleus.Commands.Tests
{
    public class CommandProcessorTests
    {
        // ProcessAsync

        [Fact]
        public async Task ProcessAsync_AsyncIntReturn_ReturnsInt()
        {
            var commandProcessor = Helpers.CreateDefaultCommandProcessor();

            const int input = 2;
            var result = await commandProcessor.ProcessAsync(new AsyncIntCommand(input));
            Assert.Equal(input, result);
        }

        [Fact]
        public async Task ProcessAsync_IntReturn_ReturnsInt()
        {
            var commandProcessor = Helpers.CreateDefaultCommandProcessor();

            const int input = 2;
            var result = await commandProcessor.ProcessAsync(new IntCommand(input));
            Assert.Equal(input, result);
        }

        [Fact]
        public async Task ProcessAsync_AsyncException_ThrowsException()
        {
            var commandProcessor = Helpers.CreateDefaultCommandProcessor();

            await Assert.ThrowsAsync<TestException>(async () => await commandProcessor.ProcessAsync(new AsyncExceptionCommand()));
        }

        [Fact]
        public async Task ProcessAsync_Exception_ThrowsException()
        {
            var commandProcessor = Helpers.CreateDefaultCommandProcessor();

            await Assert.ThrowsAsync<TestException>(async () => await commandProcessor.ProcessAsync(new ExceptionCommand()));
        }

        [Fact]
        public async Task ProcessAsync_Exception_ThrowsExceptionWithCorrectStackTrace()
        {
            var commandProcessor = Helpers.CreateDefaultCommandProcessor();

            try
            {
                await commandProcessor.ProcessAsync(new ExceptionCommand());
            }
            catch (TestException testException)
            {
                Assert.Contains(Constants.ExceptionCommandStackTrace, testException.StackTrace);
            }
        }


        // ProcessResultAsync

        [Fact]
        public async Task ProcessResultAsync_AsyncIntReturn_ReturnsInt()
        {
            var commandProcessor = Helpers.CreateDefaultCommandProcessor();

            const int input = 2;
            var result = await commandProcessor.ProcessResultAsync(new AsyncIntCommand(input));
            Assert.True(result.Success);
            Assert.Equal(input, result.Result);
        }

        [Fact]
        public async Task ProcessResultAsync_IntReturn_ReturnsInt()
        {
            var commandProcessor = Helpers.CreateDefaultCommandProcessor();

            const int input = 2;
            var result = await commandProcessor.ProcessResultAsync(new IntCommand(input));
            Assert.True(result.Success);
            Assert.Equal(input, result.Result);
        }

        [Fact]
        public async Task ProcessResultAsync_Success_ExceptionThrowsException()
        {
            var commandProcessor = Helpers.CreateDefaultCommandProcessor();

            const int input = 2;
            var result = await commandProcessor.ProcessResultAsync(new IntCommand(input));
            Assert.True(result.Success);
            Assert.Throws<NotSupportedException>(() => result.Exception);
        }

        [Fact]
        public async Task ProcessResultAsync_AsyncException_ReturnsException()
        {
            var commandProcessor = Helpers.CreateDefaultCommandProcessor();

            var result = await commandProcessor.ProcessResultAsync(new AsyncExceptionCommand());
            Assert.False(result.Success);
            Assert.NotNull(result.Exception);
            Assert.IsType<TestException>(result.Exception);
        }

        [Fact]
        public async Task ProcessResultAsync_Exception_ReturnsException()
        {
            var commandProcessor = Helpers.CreateDefaultCommandProcessor();

            var result = await commandProcessor.ProcessResultAsync(new ExceptionCommand());
            Assert.False(result.Success);
            Assert.NotNull(result.Exception);
            Assert.IsType<TestException>(result.Exception);
        }

        [Fact]
        public async Task ProcessResultAsync_Exception_ReturnsExceptionWithCorrectStackTrace()
        {
            var commandProcessor = Helpers.CreateDefaultCommandProcessor();

            var result = await commandProcessor.ProcessResultAsync(new ExceptionCommand());
            Assert.Contains(Constants.ExceptionCommandStackTrace, result.Exception.StackTrace);
        }

        [Fact]
        public async Task ProcessResultAsync_NotSuccess_ResultThrowsException()
        {
            var commandProcessor = Helpers.CreateDefaultCommandProcessor();

            var result = await commandProcessor.ProcessResultAsync(new ExceptionCommand());
            Assert.False(result.Success);
            Assert.Throws<NotSupportedException>(() => result.Result);
        }
    }
}

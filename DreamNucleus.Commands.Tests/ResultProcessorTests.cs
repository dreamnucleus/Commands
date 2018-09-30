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
        // ExecuteAsync

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

        [Fact]
        public async Task ExecuteAsync_InlineCatchException_ReturnsException()
        {
            var resultProcessor = Helpers.CreateDefaultResultProcessor();

            var result = await resultProcessor.For(new ExceptionCommand())
                .Catch<TestException>().Return(r => new ObjectResult(r))
                .ExecuteAsync();

            Assert.Equal(typeof(TestException), result.Result.GetType());
        }

        [Fact]
        public async Task ExecuteAsync_InlineCatchException_ReturnsExceptionWithCorrectStackTrace()
        {
            var resultProcessor = Helpers.CreateDefaultResultProcessor();

            var result = await resultProcessor.For(new ExceptionCommand())
                .Catch<TestException>().Return(r => new ObjectResult(r))
                .ExecuteAsync();

            Assert.Contains(Constants.ExceptionCommandStackTrace, ((TestException)result.Result).StackTrace);
        }

        [Fact]
        public async Task ExecuteAsync_DefaultCatchException_ReturnsException()
        {
            var resultProcessor = Helpers.CreateDefaultResultProcessor(resultRegister =>
                {
                    resultRegister.When<TestException>().Return(r => new ObjectResult(r));
                });

            var result = await resultProcessor.For(new ExceptionCommand())
                .ExecuteAsync();

            Assert.Equal(typeof(TestException), result.Result.GetType());
        }

        [Fact]
        public async Task ExecuteAsync_DefaultCatchException_ReturnsExceptionWithCorrectStackTrace()
        {
            var resultProcessor = Helpers.CreateDefaultResultProcessor(resultRegister =>
            {
                resultRegister.When<TestException>().Return(r => new ObjectResult(r));
            });

            var result = await resultProcessor.For(new ExceptionCommand())
                .ExecuteAsync();

            Assert.Contains(Constants.ExceptionCommandStackTrace, ((TestException)result.Result).StackTrace);
        }

        [Fact]
        public async Task ExecuteAsync_NoCatchException_ThrowsException()
        {
            var resultProcessor = Helpers.CreateDefaultResultProcessor();

            await Assert.ThrowsAsync<TestException>(async () => await resultProcessor.For(new ExceptionCommand()).ExecuteAsync());
        }

        [Fact]
        public async Task ExecuteAsync_NoCatchException_ThrowsExceptionWithCorrectStackTrace()
        {
            var resultProcessor = Helpers.CreateDefaultResultProcessor();

            try
            {
                await resultProcessor.For(new ExceptionCommand()).ExecuteAsync();
            }
            catch (TestException testException)
            {
                Assert.Contains(Constants.ExceptionCommandStackTrace, testException.StackTrace);
            }
        }

        [Fact]
        public async Task ExecuteAsync_ResultNotRegistered_ThrowsResultNotRegisteredException()
        {
            var resultProcessor = Helpers.CreateDefaultResultProcessor();

            await Assert.ThrowsAsync<ResultNotRegisteredException>(async () => await resultProcessor.For(new IntCommand(1)).ExecuteAsync());
        }

        // ExecuteSuccessAsync

        [Fact]
        public async Task ExecuteSuccessAsync_AsyncIntReturn_ReturnsInt()
        {
            var resultProcessor = Helpers.CreateDefaultResultProcessor();

            const int input = 2;
            var result = await resultProcessor.For(new AsyncIntCommand(input))
                .ExecuteSuccessAsync();

            Assert.Equal(input, result);
        }

        [Fact]
        public async Task ExecuteSuccessAsync_Exception_ThrowsException()
        {
            var resultProcessor = Helpers.CreateDefaultResultProcessor();

            await Assert.ThrowsAsync<TestException>(async () => await resultProcessor.For(new ExceptionCommand()).ExecuteSuccessAsync());
        }


        [Fact]
        public async Task ExecuteSuccessAsync_Exception_ThrowsExceptionWithCorrectStackTrace()
        {
            var resultProcessor = Helpers.CreateDefaultResultProcessor();

            try
            {
                await resultProcessor.For(new ExceptionCommand())
                    .ExecuteSuccessAsync();
            }
            catch (TestException testException)
            {
                Assert.Contains(Constants.ExceptionCommandStackTrace, testException.StackTrace);
            }
        }
    }
}

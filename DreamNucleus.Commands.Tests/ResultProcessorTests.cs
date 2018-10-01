using System.Threading.Tasks;
using Autofac;
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
            var resultProcessor = Helpers.CreateDefaultResultProcessor(
                containerBuilder =>
                {
                    containerBuilder.RegisterType<AsyncIntCommandHandler>().As<IAsyncCommandHandler<AsyncIntCommand, int>>();
                },
                autofacCommandsBuilder =>
                {
                },
                resultRegister =>
                {
                });

            const int input = 2;
            var result = await resultProcessor.For(new AsyncIntCommand(input))
                .When(o => o.Success()).Return(r => new ObjectResult(r))
                .ExecuteAsync();

            Assert.Equal(input, (int)result.Result);
        }

        [Fact]
        public async Task ExecuteAsync_InlineCatchException_ReturnsException()
        {
            var resultProcessor = Helpers.CreateDefaultResultProcessor(
                containerBuilder =>
                {
                    containerBuilder.RegisterType<ExceptionCommandHandler>().As<IAsyncCommandHandler<ExceptionCommand, Unit>>();
                },
                autofacCommandsBuilder =>
                {
                },
                resultRegister =>
                {
                });

            var result = await resultProcessor.For(new ExceptionCommand())
                .Catch<TestException>().Return(r => new ObjectResult(r))
                .ExecuteAsync();

            Assert.Equal(typeof(TestException), result.Result.GetType());
        }

        [Fact]
        public async Task ExecuteAsync_InlineCatchException_ReturnsExceptionWithCorrectStackTrace()
        {
            var resultProcessor = Helpers.CreateDefaultResultProcessor(
                containerBuilder =>
                {
                    containerBuilder.RegisterType<ExceptionCommandHandler>().As<IAsyncCommandHandler<ExceptionCommand, Unit>>();
                },
                autofacCommandsBuilder =>
                {
                },
                resultRegister =>
                {
                });

            var result = await resultProcessor.For(new ExceptionCommand())
                .Catch<TestException>().Return(r => new ObjectResult(r))
                .ExecuteAsync();

            Assert.Contains(Constants.ExceptionCommandStackTrace, ((TestException)result.Result).StackTrace);
        }

        [Fact]
        public async Task ExecuteAsync_DefaultCatchException_ReturnsException()
        {
            var resultProcessor = Helpers.CreateDefaultResultProcessor(
                containerBuilder =>
                {
                    containerBuilder.RegisterType<ExceptionCommandHandler>().As<IAsyncCommandHandler<ExceptionCommand, Unit>>();
                },
                autofacCommandsBuilder =>
                {
                },
                resultRegister =>
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
            var resultProcessor = Helpers.CreateDefaultResultProcessor(
                containerBuilder =>
                {
                    containerBuilder.RegisterType<ExceptionCommandHandler>().As<IAsyncCommandHandler<ExceptionCommand, Unit>>();
                },
                autofacCommandsBuilder =>
                {
                },
                resultRegister =>
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
            var resultProcessor = Helpers.CreateDefaultResultProcessor(
                containerBuilder =>
                {
                    containerBuilder.RegisterType<ExceptionCommandHandler>().As<IAsyncCommandHandler<ExceptionCommand, Unit>>();
                },
                autofacCommandsBuilder =>
                {
                },
                resultRegister =>
                {
                });

            await Assert.ThrowsAsync<TestException>(async () => await resultProcessor.For(new ExceptionCommand()).ExecuteAsync());
        }

        [Fact]
        public async Task ExecuteAsync_NoCatchException_ThrowsExceptionWithCorrectStackTrace()
        {
            var resultProcessor = Helpers.CreateDefaultResultProcessor(
                containerBuilder =>
                {
                    containerBuilder.RegisterType<ExceptionCommandHandler>().As<IAsyncCommandHandler<ExceptionCommand, Unit>>();
                },
                autofacCommandsBuilder =>
                {
                },
                resultRegister =>
                {
                });

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
            var resultProcessor = Helpers.CreateDefaultResultProcessor(
                containerBuilder =>
                {
                    containerBuilder.RegisterType<IntCommandHandler>().As<IAsyncCommandHandler<IntCommand, int>>();
                },
                autofacCommandsBuilder =>
                {
                },
                resultRegister =>
                {
                });

            await Assert.ThrowsAsync<ResultNotRegisteredException>(async () => await resultProcessor.For(new IntCommand(1)).ExecuteAsync());
        }

        // ExecuteSuccessAsync

        [Fact]
        public async Task ExecuteSuccessAsync_AsyncIntReturn_ReturnsInt()
        {
            var resultProcessor = Helpers.CreateDefaultResultProcessor(
                containerBuilder =>
                {
                    containerBuilder.RegisterType<AsyncIntCommandHandler>().As<IAsyncCommandHandler<AsyncIntCommand, int>>();
                },
                autofacCommandsBuilder =>
                {
                },
                resultRegister =>
                {
                });

            const int input = 2;
            var result = await resultProcessor.For(new AsyncIntCommand(input))
                .ExecuteSuccessAsync();

            Assert.Equal(input, result);
        }

        [Fact]
        public async Task ExecuteSuccessAsync_Exception_ThrowsException()
        {
            var resultProcessor = Helpers.CreateDefaultResultProcessor(
                containerBuilder =>
                {
                    containerBuilder.RegisterType<ExceptionCommandHandler>().As<IAsyncCommandHandler<ExceptionCommand, Unit>>();
                },
                autofacCommandsBuilder =>
                {
                },
                resultRegister =>
                {
                });

            await Assert.ThrowsAsync<TestException>(async () => await resultProcessor.For(new ExceptionCommand()).ExecuteSuccessAsync());
        }


        [Fact]
        public async Task ExecuteSuccessAsync_Exception_ThrowsExceptionWithCorrectStackTrace()
        {
            var resultProcessor = Helpers.CreateDefaultResultProcessor(
                containerBuilder =>
                {
                    containerBuilder.RegisterType<ExceptionCommandHandler>().As<IAsyncCommandHandler<ExceptionCommand, Unit>>();
                },
                autofacCommandsBuilder =>
                {
                },
                resultRegister =>
                {
                });

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

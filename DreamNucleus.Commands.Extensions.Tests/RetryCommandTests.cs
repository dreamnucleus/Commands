using System;
using System.Threading.Tasks;
using Autofac;
using DreamNucleus.Commands.Extensions.Retry;
using DreamNucleus.Commands.Extensions.Tests.Common;
using DreamNucleus.Commands.Results;
using Xunit;

// TODO: REMOVE
namespace DreamNucleus.Commands.Extensions.Tests
{
    public class RetryCommandTests
    {
        [Fact]
        public async Task ProcessAsync_CommandRetriesOnce_ReturnsResult()
        {
            var commandProcessor = Helpers.CreateDefaultCommandProcessor(
                containerBuilder =>
                {
                    containerBuilder.RegisterType<RetryCommandHandler>().As<IAsyncCommandHandler<RetryCommand, Unit>>();
                },
                commandsBuilder =>
                {
                    commandsBuilder.Use<RetryExceptionPipeline>();
                });

            var result = await commandProcessor.ProcessAsync(new RetryCommand(1));

            Assert.Equal(Unit.Value, result);
        }

        [Fact]
        public async Task ProcessAsync_CommandRetriesOnceOnly_ThrowsException()
        {
            var commandProcessor = Helpers.CreateDefaultCommandProcessor(
                containerBuilder =>
                {
                    containerBuilder.RegisterType<RetryCommandHandler>().As<IAsyncCommandHandler<RetryCommand, Unit>>();
                },
                commandsBuilder =>
                {
                    commandsBuilder.Use<RetryExceptionPipeline>();
                });

            await Assert.ThrowsAsync<Exception>(async () => await commandProcessor.ProcessAsync(new RetryCommand(6)));
        }
    }
}

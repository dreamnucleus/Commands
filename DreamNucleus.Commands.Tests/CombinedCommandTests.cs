using System.Threading.Tasks;
using Autofac;
using DreamNucleus.Commands.Tests.Common;
using Xunit;

namespace DreamNucleus.Commands.Tests
{
    // TODO: joining commands and testing the lifetime
    public class CombinedCommandTests
    {
        [Fact]
        public async Task ProcessAsync_CombinedReturn_ReturnsInt()
        {
            var commandProcessor = Helpers.CreateDefaultCommandProcessor(
                containerBuilder =>
                {
                    containerBuilder.RegisterType<IntCommandHandler>().As<IAsyncCommandHandler<IntCommand, int>>();
                    containerBuilder.RegisterType<AsyncIntCommandHandler>().As<IAsyncCommandHandler<AsyncIntCommand, int>>();
                    containerBuilder.RegisterType<CombinedCommandHandler>().As<IAsyncCommandHandler<CombinedCommand, int>>();
                },
                autofacCommandsBuilder =>
                {
                    autofacCommandsBuilder.Use<PrePipeline>();
                    autofacCommandsBuilder.Use<SingletonPostPipeline>();
                });

            const int input = 2;
            var result = await commandProcessor.ProcessAsync(new CombinedCommand(input));
            Assert.Equal(input + input, result);
        }
    }
}

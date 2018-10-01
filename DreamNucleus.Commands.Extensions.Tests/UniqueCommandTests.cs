using System.Threading.Tasks;
using Autofac;
using DreamNucleus.Commands.Extensions.Results;
using DreamNucleus.Commands.Extensions.Tests.Common;
using DreamNucleus.Commands.Extensions.Unique;
using DreamNucleus.Commands.Results;
using Xunit;

// TODO: REMOVE
namespace DreamNucleus.Commands.Extensions.Tests
{
    public class UniqueCommandTests
    {
        private static ICommandProcessor CommandProcessor()
        {
            return Helpers.CreateDefaultCommandProcessor(
                containerBuilder =>
                {
                    containerBuilder.RegisterType<FakeUniqueManager>().As<IUniqueManager>().SingleInstance();
                    containerBuilder.RegisterType<UniqueCommandHandler>().As<IAsyncCommandHandler<UniqueCommand, Unit>>();
                },
                commandsBuilder =>
                {
                    commandsBuilder.Use<UniquePipeline>();
                });
        }

        [Fact]
        public async Task ProcessAsync_TwoUniqueCommands_NoException()
        {
            var commandProcessor = CommandProcessor();

            const string id1 = "1";
            const string id2 = "2";

            await commandProcessor.ProcessAsync(new UniqueCommand(id1));
            await commandProcessor.ProcessAsync(new UniqueCommand(id2));
            Assert.True(true);
        }

        [Fact]
        public async Task ProcessAsync_TwoNonUniqueCommands_ThrowsException()
        {
            var commandProcessor = CommandProcessor();

            const string id = "1";

            await commandProcessor.ProcessAsync(new UniqueCommand(id));
            await Assert.ThrowsAsync<ConflictException>(async () => await commandProcessor.ProcessAsync(new UniqueCommand(id)));
        }
    }
}

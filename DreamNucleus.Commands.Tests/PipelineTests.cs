using System.Threading.Tasks;
using Autofac;
using DreamNucleus.Commands.Tests.Common;
using Xunit;

namespace DreamNucleus.Commands.Tests
{
    // TODO: order of exec
    public class PipelineTests
    {
        [Fact]
        public async Task Todo()
        {
            var commandProcessor = Helpers.CreateDefaultCommandProcessor(
                containerBuilder =>
                {
                    containerBuilder.RegisterType<AsyncIntCommandHandler>().As<IAsyncCommandHandler<AsyncIntCommand, int>>();
                },
                autofacCommandsBuilder =>
                {
                    autofacCommandsBuilder.Use<SingletonPipeline>();
                    autofacCommandsBuilder.Use<RepeatPipeline>();

                    autofacCommandsBuilder.Use<IntCommandExecutingNotification>();
                    autofacCommandsBuilder.Use<IntCommandExecutedNotification>();
                    autofacCommandsBuilder.Use<ExceptionCommandExceptionNotification>();

                    autofacCommandsBuilder.Use<ExecutingPipeline>();
                    autofacCommandsBuilder.Use<ExecutedPipeline>();
                    autofacCommandsBuilder.Use<ExceptionPipeline>();
                });

            const int input = 2;
            var result = await commandProcessor.ProcessAsync(new AsyncIntCommand(input));
            Assert.Equal(input, result);
        }
    }
}

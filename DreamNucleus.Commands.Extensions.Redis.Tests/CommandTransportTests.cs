using System.Threading.Tasks;
using Autofac;
using DreamNucleus.Commands.Extensions.Redis.Tests.Common;
using DreamNucleus.Commands.Results;
using DreamNucleus.Commands.Tests.Common;
using Xunit;

namespace DreamNucleus.Commands.Extensions.Redis.Tests
{
    public class CommandTransportTests
    {
        //[Fact]
        //public async Task ProcessAsync_AsyncIntReturn_ReturnsInt()
        //{
        //    var commandProcessor = Helpers.CreateDefaultCommandProcessor(
        //        containerBuilder =>
        //        {
        //            containerBuilder.RegisterType<AsyncIntCommandHandler>().As<IAsyncCommandHandler<AsyncIntCommand, int>>();
        //        },
        //        autofacCommandsBuilder =>
        //        {
        //        });

        //    var connectionMultiplexer = await ConnectionMultiplexer.ConnectAsync("localhost,allowAdmin=true");
        //    //var redisServer = connectionMultiplexer.GetServer(connectionMultiplexer.GetEndPoints().Single());
        //    //redisServer.FlushAllDatabases();
        //    var redisCommandTransportClient = new RedisCommandTransportClient(connectionMultiplexer, "test~commands1", "test~results1");
        //    var redisCommandTransportServer = new RedisCommandTransportServer(connectionMultiplexer, "test~commands1", "group", "consumer_1");

        //    var commandProcessorClient = new CommandProcessorClient(redisCommandTransportClient);
        //    var commandProcessorServer = new CommandProcessorServer(commandProcessor, redisCommandTransportServer);

        //    _ = commandProcessorServer.StartAsync();

        //    const int input = 2;
        //    var result = await commandProcessorClient.ProcessAsync(new AsyncIntCommand(input));
        //    Assert.Equal(input, result);
        //}


        [Fact]
        public async Task ProcessAsync_AsyncIntReturn_ReturnsInt()
        {
            var commandProcessor = Helpers.CreateDefaultCommandProcessor(
                containerBuilder =>
                {
                    containerBuilder.RegisterType<AsyncIntCommandHandler>().As<IAsyncCommandHandler<AsyncIntCommand, int>>();
                },
                autofacCommandsBuilder =>
                {
                });

            var inMemoryTransport = new InMemoryTransport();
            var commandProcessorClient = new CommandProcessorClient(new InMemoryCommandTransportClient(inMemoryTransport));
            var commandProcessorServer = new CommandProcessorServer(commandProcessor, new InMemoryCommandTransportServer(inMemoryTransport));

            await commandProcessorServer.StartAsync();

            const int input = 2;
            var result = await commandProcessorClient.ProcessAsync(new AsyncIntCommand(input));
            Assert.Equal(input, result);

            await commandProcessorServer.StopAsync();
        }

        [Fact]
        public async Task ProcessAsync_GenericReturn_ReturnsGeneric()
        {
            var commandProcessor = Helpers.CreateDefaultCommandProcessor(
                containerBuilder =>
                {
                    containerBuilder.RegisterType<GenericCommandHandler<string>>().As<IAsyncCommandHandler<GenericCommand<string>, string>>();
                },
                autofacCommandsBuilder =>
                {
                });

            var inMemoryTransport = new InMemoryTransport();
            var commandProcessorClient = new CommandProcessorClient(new InMemoryCommandTransportClient(inMemoryTransport));
            var commandProcessorServer = new CommandProcessorServer(commandProcessor, new InMemoryCommandTransportServer(inMemoryTransport));

            await commandProcessorServer.StartAsync();

            const string input = "2";
            var result = await commandProcessorClient.ProcessAsync(new GenericCommand<string>(input));
            Assert.Equal(input, result);

            await commandProcessorServer.StopAsync();
        }

        [Fact]
        public async Task ProcessAsync_AsyncException_ThrowsException()
        {
            var commandProcessor = Helpers.CreateDefaultCommandProcessor(
                containerBuilder =>
                {
                    containerBuilder.RegisterType<AsyncExceptionCommandHandler>().As<IAsyncCommandHandler<AsyncExceptionCommand, Unit>>();
                },
                autofacCommandsBuilder =>
                {
                });

            var inMemoryTransport = new InMemoryTransport();
            var commandProcessorClient = new CommandProcessorClient(new InMemoryCommandTransportClient(inMemoryTransport));
            var commandProcessorServer = new CommandProcessorServer(commandProcessor, new InMemoryCommandTransportServer(inMemoryTransport));

            await commandProcessorServer.StartAsync();

            await Assert.ThrowsAsync<TestException>(async () => await commandProcessorClient.ProcessAsync(new AsyncExceptionCommand()));
        }

        [Fact]
        public async Task ProcessAsync_MultipleCommands_MultipleReturns()
        {
            var commandProcessor = Helpers.CreateDefaultCommandProcessor(
                containerBuilder =>
                {
                    containerBuilder.RegisterType<AsyncIntCommandHandler>().As<IAsyncCommandHandler<AsyncIntCommand, int>>();
                    containerBuilder.RegisterType<LongCommandHandler>().As<IAsyncCommandHandler<LongCommand, long>>();
                },
                autofacCommandsBuilder =>
                {
                });

            var inMemoryTransport = new InMemoryTransport();
            var commandProcessorClient = new CommandProcessorClient(new InMemoryCommandTransportClient(inMemoryTransport));
            var commandProcessorServer = new CommandProcessorServer(commandProcessor, new InMemoryCommandTransportServer(inMemoryTransport));

            await commandProcessorServer.StartAsync();

            const long input1 = 5;
            var result1 = await commandProcessorClient.ProcessAsync(new LongCommand(input1));
            Assert.Equal(input1, result1);

            const int input2 = 2;
            var result2 = await commandProcessorClient.ProcessAsync(new AsyncIntCommand(input2));
            Assert.Equal(input2, result2);

            await commandProcessorServer.StopAsync();
        }
    }
}

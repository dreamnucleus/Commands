using System;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using DreamNucleus.Commands.Results;
using DreamNucleus.Commands.Tests.Common;
using StackExchange.Redis;
using Xunit;

namespace DreamNucleus.Commands.Extensions.Redis.Tests
{
    public class UnitTests
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

        //[Fact]
        //public async Task ProcessAsync_GenericReturn_ReturnsGeneric()
        //{
        //    var commandProcessor = Helpers.CreateDefaultCommandProcessor(
        //        containerBuilder =>
        //        {
        //            containerBuilder.RegisterType<GenericCommandHandler<string>>().As<IAsyncCommandHandler<GenericCommand<string>, string>>();
        //        },
        //        autofacCommandsBuilder =>
        //        {
        //        });

        //    var connectionMultiplexer = await ConnectionMultiplexer.ConnectAsync("localhost,allowAdmin=true");

        //    var client = new RedisCommandProcessorClient(connectionMultiplexer, "test~commands2", "test~results2");
        //    var server = new RedisCommandProcessorServer(commandProcessor, connectionMultiplexer, "test~commands2", "group", "consumer_1");

        //    _ = server.Start();

        //    const string input = "2";
        //    var result = await client.ProcessAsync(new GenericCommand<string>(input));
        //    Assert.Equal(input, result);
        //}

        //[Fact]
        //public async Task ProcessAsync_AsyncException_ThrowsException()
        //{
        //    var commandProcessor = Helpers.CreateDefaultCommandProcessor(
        //        containerBuilder =>
        //        {
        //            //containerBuilder.RegisterType<AsyncIntCommandHandler>().As<IAsyncCommandHandler<AsyncIntCommand, int>>();
        //            //containerBuilder.RegisterType<GenericCommandHandler<string>>().As<IAsyncCommandHandler<GenericCommand<string>, string>>();
        //            containerBuilder.RegisterType<AsyncExceptionCommandHandler>().As<IAsyncCommandHandler<AsyncExceptionCommand, Unit>>();
        //        },
        //        autofacCommandsBuilder =>
        //        {
        //        });

        //    var connectionMultiplexer = await ConnectionMultiplexer.ConnectAsync("localhost,allowAdmin=true");

        //    var client = new RedisCommandProcessorClient(connectionMultiplexer, "test~commands3", "test~results3");
        //    var server = new RedisCommandProcessorServer(commandProcessor, connectionMultiplexer, "test~commands3", "group", "consumer_1");

        //    _ = server.Start();

        //    await Assert.ThrowsAsync<TestException>(async () => await client.ProcessAsync(new AsyncExceptionCommand()));
        //}

        //[Fact]
        //public async Task ProcessAsync_MultipleCommands_MultipleReturns()
        //{
        //    var commandProcessor = Helpers.CreateDefaultCommandProcessor(
        //        containerBuilder =>
        //        {
        //            containerBuilder.RegisterType<AsyncIntCommandHandler>().As<IAsyncCommandHandler<AsyncIntCommand, int>>();
        //            containerBuilder.RegisterType<LongCommandHandler>().As<IAsyncCommandHandler<LongCommand, long>>();
        //        },
        //        autofacCommandsBuilder =>
        //        {
        //        });

        //    var connectionMultiplexer = await ConnectionMultiplexer.ConnectAsync("localhost,allowAdmin=true");

        //    var client = new RedisCommandProcessorClient(connectionMultiplexer, "test~commands4", "test~results4");
        //    var server = new RedisCommandProcessorServer(commandProcessor, connectionMultiplexer, "test~commands4", "group", "consumer_1");

        //    _ = server.Start();

        //    const long input1 = 5;
        //    var result1 = await client.ProcessAsync(new LongCommand(input1));
        //    Assert.Equal(input1, result1);

        //    const int input2 = 2;
        //    var result2 = await client.ProcessAsync(new AsyncIntCommand(input2));
        //    Assert.Equal(input2, result2);
        //}
    }
}

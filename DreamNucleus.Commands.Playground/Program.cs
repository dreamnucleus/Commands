﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Autofac;
using DreamNucleus.Commands.Autofac;
using DreamNucleus.Commands.Extensions.Redis;
using DreamNucleus.Commands.Extensions.Results;
using DreamNucleus.Commands.Results;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace DreamNucleus.Commands.Playground
{
    public class Program
    {
        public static async Task Main(string[] args)
        {

            //using (var context = new BloggingContext())
            //{
            //    context.Database.EnsureDeleted();
            //    context.Database.EnsureCreated();
            //    var blog = new Blog
            //    {
            //        Url = "http://vswebessentials.com/blog"
            //    };
            //    context.Blogs.Add(blog);
            //    context.SaveChanges();
            //}

            var containerBuilder = new ContainerBuilder();

            containerBuilder.RegisterType<BloggingContext>().InstancePerLifetimeScope();

            containerBuilder.RegisterType<TestCommandHandler>().As<IAsyncCommandHandler<TestCommand, List<TestData>>>();
            containerBuilder.RegisterType<GetBlogCommandHandler>().As<IAsyncCommandHandler<GetBlogCommand, BlogData>>();
            containerBuilder.RegisterType<CreatePostCommandHandler>().As<IAsyncCommandHandler<CreatePostCommand, PostData>>();

            containerBuilder.RegisterType<FakeCommandHandler>().As<IAsyncCommandHandler<FakeCommand, FakeData>>();

            containerBuilder.RegisterType<MultipleCommandHandler>().As<IAsyncCommandHandler<MultipleCommand, MultipleData>>();

            containerBuilder.RegisterType<IntCommandHandler>().As<IAsyncCommandHandler<IntCommand, int>>();
            containerBuilder.RegisterType<LongCommandHandler>().As<IAsyncCommandHandler<LongCommand, long>>();
            containerBuilder.RegisterType<NoneCommandHandler>().As<IAsyncCommandHandler<NoneCommand, Unit>>();
            containerBuilder.RegisterType<ExceptionCommandHandler>().As<IAsyncCommandHandler<ExceptionCommand, Unit>>();

            var commandsBuilder = new AutofacCommandsBuilder(containerBuilder);
            commandsBuilder.Use<TestPipeline>();
            commandsBuilder.Use<SecondTestPipeline>();

            commandsBuilder.Use<ExecutingNotification>();
            commandsBuilder.Use<ExecutedNotification>();
            commandsBuilder.Use<ExceptionNotification>();

            containerBuilder.RegisterInstance(commandsBuilder).SingleInstance();
            containerBuilder.RegisterType<AutofacLifetimeScopeService>().As<ILifetimeScopeService>();

            containerBuilder.RegisterType<CommandProcessor>().As<ICommandProcessor>();

            var container = containerBuilder.Build();

            var commandProcessor = container.Resolve<ICommandProcessor>();
            //var commandProcessor = new CommandProcessor(commandsBuilder, new AutofacLifetimeScopeService(container.BeginLifetimeScope()));

            var connectionMultiplexer = await ConnectionMultiplexer.ConnectAsync("localhost,allowAdmin=true");
            var containerSerializer = new NewtonsoftJsonContainerSerializer();

            var commandProcessorClient = new CommandProcessorClient(new RedisCommandTransportClient("test~commands", "test~results", connectionMultiplexer, containerSerializer));
            var commandTransportServer = new RedisCommandTransportServer("test~commands", "group", "consumer_1", connectionMultiplexer, containerSerializer);
            var commandProcessorServer = new CommandProcessorServer(commandProcessor, commandTransportServer);

            await commandTransportServer.StartAsync();

            var intResult = await commandProcessorClient.ProcessAsync(new IntCommand(2));
            var longResult = await commandProcessorClient.ProcessAsync(new LongCommand(2));
            var noneResult = await commandProcessorClient.ProcessAsync(new NoneCommand());

            await Task.Delay(-1);

            try
            {
                var exceptionResult = await commandProcessorClient.ProcessAsync(new ExceptionCommand());
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }

            await Task.Delay(-1);

            var resultRegister = new ResultRegister<HttpResult>();
            resultRegister.When<NotFoundException>().Return(r => new HttpResult(444444444));
            var resultProcessor = new ResultProcessor<HttpResult>(resultRegister.Emit(), commandsBuilder,
                new AutofacLifetimeScopeService(container.BeginLifetimeScope()));


            string jsonString = JsonConvert.SerializeObject(new IntCommand { Id = 2 }, Formatting.Indented, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.All
            });

            var commandObject = JsonConvert.DeserializeObject(jsonString, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.All
            });

            try
            {
                var throws = await resultProcessor.For(new FakeCommand(-1)).ExecuteAsync();
                var test = await commandProcessor.ProcessAsync(new FakeCommand(-1));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            Console.WriteLine();
            Console.WriteLine();
            var test1 = commandProcessor.ProcessAsync(new FakeCommand(10)).Result;
            Console.WriteLine();



            // TODO: need to be able to add in global stuff
            // TODO: ExecuteSuccessAsync could be from another Processor? like a non-generic one


            try
            {
                var throws = commandProcessor.ProcessAsync(new TestCommand()).Result;
            }
            catch (Exception)
            {
                // ignore
            }

            // using default handlers
            var defultHandlers = resultProcessor.For(new TestCommand())
                //.When(o => o.NotFound()).Return(r => new HttpResult(404))
                .When(o => o.Conflict()).Return(r => new HttpResult(409))
                .When(o => o.Success()).Return(r => new HttpResult(200))
                .Catch<Exception>().Return(e => new HttpResult(500))
                .ExecuteAsync().Result;

            // checking lifetime
            for (int i = 0; i < 2; i++)
            {
                var lifetime = resultProcessor.For(new GetBlogCommand(1))
                    .When(o => o.NotFound()).Return(r => new HttpResult(404))
                    .When(o => o.Success()).Return(r => new HttpResult(200))
                    .ExecuteAsync().Result;
            }

            // internal use of command
            var toReturn = resultProcessor.For(new CreatePostCommand(1, "2", "My Blog", "Good day!"))
                .When(o => o.NotFound()).Return(r => new HttpResult(404))
                .When(o => o.Conflict()).Return(r => new HttpResult(409))
                .When(o => o.Success()).Return(r => new HttpResult(200))
                .ExecuteAsync().Result;


            // multiple commands with same handler
            var multipleOne = resultProcessor.For(new MultipleOneCommand())
                .When(o => o.Success()).Return(r => new HttpResult(r.Result))
                .ExecuteAsync().Result;

            var multipleTwo = resultProcessor.For(new MultipleTwoCommand())
                .When(o => o.Success()).Return(r => new HttpResult(r.Result))
                .ExecuteAsync().Result;

            var intReturn = resultProcessor.For(new IntCommand())
                .When(o => o.Success()).Return(r => new HttpResult(r))
                .ExecuteAsync().Result;

            // checking it throws the exceptions
            try
            {
                var throws = commandProcessor.ProcessAsync(new FakeCommand(-1)).Result;
            }
            catch (Exception)
            {
                // ignore
            }
        }
    }

}


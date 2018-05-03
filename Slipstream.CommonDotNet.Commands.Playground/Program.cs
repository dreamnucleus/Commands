using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Autofac;
using Slipstream.CommonDotNet.Commands.Autofac;
using Slipstream.CommonDotNet.Commands.Results;

namespace Slipstream.CommonDotNet.Commands.Playground
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

            containerBuilder.RegisterType<GenericCommandHandler<object>>().As<IAsyncCommandHandler<GenericCommand<object>, object>>();

            containerBuilder.RegisterType<MultipleCommandHandler>().As<IAsyncCommandHandler<MultipleCommand, MultipleData>>();

            containerBuilder.RegisterType<IntCommandHandler>().As<IAsyncCommandHandler<IntCommand, int>>();
            containerBuilder.RegisterType<NoneCommandHandler>().As<IAsyncCommandHandler<NoneCommand, Unit>>();

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
            //try
            //{
            //    var test = await commandProcessor.ProcessAsync(new FakeCommand(10));
            //}
            //catch (Exception e)
            //{
            //    Console.WriteLine(e);
            //}

            try
            {

                var throws = commandProcessor.ProcessAsync(new TestCommand()).Result;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                // ignore
            }

            //var generic = commandProcessor.ProcessAsync(new GenericCommand<object>()).Result;

            //Console.WriteLine();
            //Console.WriteLine();
            //var test1 = commandProcessor.ProcessAsync(new FakeCommand(10)).Result;
            //Console.WriteLine();


            var resultRegister = new ResultRegister<HttpResult>();
            resultRegister.When<NotFoundException>().Return(r => new HttpResult(444444444));

            // TODO: need to be able to add in global stuff
            // TODO: ExecuteSuccessAsync could be from another Processor? like a non-generic one
            var resultProcessor = new ResultProcessor<HttpResult>(resultRegister.Emit(), commandsBuilder,
                new AutofacLifetimeScopeService(container.BeginLifetimeScope()));


            // using default handlers
            var defultHandlers = await resultProcessor.For(new TestCommand())
                //.When(o => o.NotFound()).Return(r => new HttpResult(404))
                .When(o => o.Conflict()).Return(r => new HttpResult(409))
                .When(o => o.Success()).Return(r => new HttpResult(200))
                .ExecuteAsync();

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


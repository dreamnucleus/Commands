using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Autofac;
using Slipstream.CommonDotNet.Commands;
using Slipstream.CommonDotNet.Commands.Autofac;
using Slipstream.CommonDotNet.Commands.Results;

namespace Slipstream.CommonDotNet.Commands.Playground
{
    public class Program
    {
        public static void Main(string[] args)
        {

            using (var db = new BloggingContext())
            {
                db.Database.EnsureDeleted();
                db.Database.EnsureCreated();
            }

            var containerBuilder = new ContainerBuilder();

            containerBuilder.RegisterType<BloggingContext>().InstancePerLifetimeScope();

            containerBuilder.RegisterType<TestCommandHandler>().As<IAsyncCommandHandler<TestCommand>>();
            containerBuilder.RegisterType<GetBlogCommandHandler>().As<IAsyncCommandHandler<GetBlogCommand>>();
            containerBuilder.RegisterType<CreatePostCommandHandler>().As<IAsyncCommandHandler<CreatePostCommand>>();

            containerBuilder.RegisterType<FakeCommandHandler>().As<IAsyncCommandHandler<FakeCommand>>();

            containerBuilder.RegisterType<MultipleCommandHandler>().As<IAsyncCommandHandler<MultipleCommand>>();

            containerBuilder.RegisterType<IntCommandHandler>().As<IAsyncCommandHandler<IntCommand>>();



            var container = containerBuilder.Build();


            var resultRegister = new ResultRegister<HttpResult>();
            resultRegister.When<NotFoundException>().Return(r => new HttpResult(444444444));

            // TODO: need to be able to add in global stuff
            // TODO: ExecuteSuccessAsync could be from another Processor? like a non-generic one
            var resultProcessor = new ResultProcessor<HttpResult>(resultRegister.Emit(),
                new LifetimeScopeService(container.BeginLifetimeScope()));
            var commandProcessor = new CommandProcessor(new LifetimeScopeService(container.BeginLifetimeScope()));


            // using default handlers 
            var defultHandlers = resultProcessor.For(new FakeCommand(1231231))
                //.When(o => o.NotFound()).Return(r => new HttpResult(404))
                .When(o => o.Conflict()).Return(r => new HttpResult(409))
                .When(o => o.Success()).Return(r => new HttpResult(200))
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
                var throws = commandProcessor.ProcessSuccessAsync(new FakeCommand(-1)).Result;
            }
            catch (Exception)
            {
                // ignore
            }
        }
    }

}


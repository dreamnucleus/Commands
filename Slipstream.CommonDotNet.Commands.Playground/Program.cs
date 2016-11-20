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

            //using (var db = new BloggingContext())
            //{
            //    db.Database.EnsureCreated();
            //    db.Blogs.Add(new Blog { Url = "http://blogs.msdn.com/adonet" });
            //    db.SaveChanges();
            //    //Console.WriteLine("{0} records saved to database", count);

            //    //Console.WriteLine();
            //    //Console.WriteLine("All blogs in database:");
            //    //foreach (var blog in db.Blogs)
            //    //{
            //    //    Console.WriteLine(" - {0}", blog.Url);
            //    //}
            //}

            var containerBuilder = new ContainerBuilder();

            containerBuilder.RegisterType<BloggingContext>().InstancePerLifetimeScope();

            containerBuilder.RegisterType<TestCommandHandler>().As<IAsyncCommandHandler<TestCommand>>();
            containerBuilder.RegisterType<GetBlogCommandHandler>().As<IAsyncCommandHandler<GetBlogCommand>>();
            containerBuilder.RegisterType<CreatePostCommandHandler>().As<IAsyncCommandHandler<CreatePostCommand>>();

            containerBuilder.RegisterType<FakeCommandHandler>().As<IAsyncCommandHandler<FakeCommand>>();

            var container = containerBuilder.Build();


            var resultRegister = new ResultRegister<HttpResult>();
            resultRegister.When<NotFoundException>().Return(r => new HttpResult(444444444));

            // TODO: need to be able to add in global stuff
            // TODO: ExecuteSuccessAsync could be from another Processor? like a non-generic one
            var resultProcessor = new ResultProcessor<HttpResult>(resultRegister.Emit(), new LifetimeScopeService(container.BeginLifetimeScope()));
            var commandProcessor = new CommandProcessor(new LifetimeScopeService(container.BeginLifetimeScope()));


            var toReturn = resultProcessor.For(new FakeCommand(1231231))
                //.When(o => o.NotFound()).Return(r => new HttpResult(404))
                .When(o => o.Conflict()).Return(r => new HttpResult(409))
                .When(o => o.Success()).Return(r => new HttpResult(200))
                .ExecuteAsync().Result;

            Console.WriteLine($"Status code: {toReturn.StatusCode}");

            //for (int i = 0; i < 10; i++)
            //{
 

            //    //var getBlog = processor.For(new GetBlogCommand(1))
            //    //    .When(o => o.NotFound()).Return(r => new HttpResult(404))
            //    //    .When(o => o.Success()).Return(r => new HttpResult(200))
            //    //    .ExecuteAsync().Result;

            //    //try
            //    //{
            //    //    var test = commandProcessor.ProcessSuccessAsync(new FakeCommand(-1)).Result;
            //    //    Console.WriteLine(test);
            //    //}
            //    //catch (Exception exception)
            //    //{
            //    //    Console.WriteLine(exception);
            //    //}

            //    //var toReturn = resultProcessor.For(new CreatePostCommand(1, "2", "My Blog", "Good day!"))
            //    //    .When(o => o.NotFound()).Return(r => new HttpResult(404))
            //    //    .When(o => o.Conflict()).Return(r => new HttpResult(409))
            //    //    .When(o => o.Success()).Return(r => new HttpResult(200))
            //    //    .ExecuteAsync().Result;

            //    Console.WriteLine($"{i}: {toReturn.StatusCode}");
            //}


            Console.WriteLine();
        }

    }

}


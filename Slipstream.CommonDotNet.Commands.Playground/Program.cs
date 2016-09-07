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

            var containerBuilder = new ContainerBuilder();
            containerBuilder.RegisterType<TestCommandHandler>().As<IAsyncCommandHandler<TestCommand>>();
            var container = containerBuilder.Build();

            var processor = new Processor<HttpResult>(new LifetimeScopeService(container.BeginLifetimeScope()));

            for (int i = 0; i < 10; i++)
            {
                var toReturn = processor.For(new TestCommand())
                    .When(o => o.Success()).Return(r => new HttpResult(r.Code))
                    .When(o => o.NotFound()).Return(r => new HttpResult(404))
                    .ExecuteAsync().Result;
                Console.WriteLine($"{i}: {toReturn.StatusCode}");
            }


            Console.WriteLine();
        }

    }

}


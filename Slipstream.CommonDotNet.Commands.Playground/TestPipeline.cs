using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Slipstream.CommonDotNet.Commands.Pipelines;

namespace Slipstream.CommonDotNet.Commands.Playground
{
    public class TestPipeline : Pipeline
    {
        private readonly BloggingContext context;

        public TestPipeline(BloggingContext context)
        {
            Console.WriteLine("TestPipeline was created");
            this.context = context;
        }

        public override Task Incoming()
        {
            Console.WriteLine("Incoming with context: " + context.Id);
            return base.Incoming();
        }

        public override Task Outgoing()
        {
            Console.WriteLine("Outgoing with context: " + context.Id);
            return base.Outgoing();
        }
    }

    public class SecondTestPipeline : Pipeline
    {
        private readonly BloggingContext context;

        public SecondTestPipeline(BloggingContext context)
        {
            Console.WriteLine("SecondTestPipeline was created");
            this.context = context;
        }

        public override Task Incoming()
        {
            Console.WriteLine("Incoming (Second) with context: " + context.Id);
            return base.Incoming();
        }

        public override Task Outgoing()
        {
            Console.WriteLine("Outgoing (Second) with context: " + context.Id);
            return base.Outgoing();
        }
    }
}

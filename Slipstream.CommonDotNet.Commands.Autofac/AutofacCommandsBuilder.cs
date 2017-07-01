using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Slipstream.CommonDotNet.Commands.Builder;

namespace Slipstream.CommonDotNet.Commands.Autofac
{
    public class AutofacCommandsBuilder : CommandsBuilder
    {
        private readonly ContainerBuilder containerBuilder;

        public AutofacCommandsBuilder(ContainerBuilder containerBuilder)
        {
            this.containerBuilder = containerBuilder;
        }

        public override ICommandsBuilder Use<TItem>()
        {
            containerBuilder.RegisterType<TItem>().InstancePerLifetimeScope();
            return base.Use<TItem>();
        }
    }
}

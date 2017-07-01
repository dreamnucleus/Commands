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

            containerBuilder.RegisterInstance(this).As<ICommandsBuilder>().SingleInstance();
        }

        public override ICommandsBuilder Use<TItem>()
        {
            containerBuilder.RegisterType<TItem>().InstancePerLifetimeScope();
            return base.Use<TItem>();
        }

        public ICommandsBuilder Use<TItem>(AutofacLifetime autofacLifetime)
            where TItem : IUseCommandsBuilder
        {
            switch (autofacLifetime)
            {
                case AutofacLifetime.Dependancy:
                    containerBuilder.RegisterType<TItem>().InstancePerDependency();
                    break;
                case AutofacLifetime.Pipeline:
                    containerBuilder.RegisterType<TItem>().InstancePerLifetimeScope();
                    break;
                case AutofacLifetime.Singleton:
                    containerBuilder.RegisterType<TItem>().SingleInstance();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(autofacLifetime), autofacLifetime, null);
            }
            
            return base.Use<TItem>();
        }
    }
}

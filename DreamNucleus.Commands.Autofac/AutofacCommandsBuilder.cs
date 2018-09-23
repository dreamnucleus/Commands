using System;
using Autofac;
using DreamNucleus.Commands.Builder;

namespace DreamNucleus.Commands.Autofac
{
    public class AutofacCommandsBuilder : CommandsBuilder
    {
        private readonly ContainerBuilder _containerBuilder;

        public AutofacCommandsBuilder(ContainerBuilder containerBuilder)
        {
            _containerBuilder = containerBuilder;

            containerBuilder.RegisterInstance(this).As<ICommandsBuilder>().SingleInstance();
        }

        public override ICommandsBuilder Use<TItem>()
        {
            _containerBuilder.RegisterType<TItem>().InstancePerLifetimeScope();
            return base.Use<TItem>();
        }

        public ICommandsBuilder Use<TItem>(AutofacLifetime autofacLifetime)
            where TItem : IUseCommandsBuilder
        {
            switch (autofacLifetime)
            {
                case AutofacLifetime.Dependancy:
                    _containerBuilder.RegisterType<TItem>().InstancePerDependency();
                    break;
                case AutofacLifetime.Pipeline:
                    _containerBuilder.RegisterType<TItem>().InstancePerLifetimeScope();
                    break;
                case AutofacLifetime.Singleton:
                    _containerBuilder.RegisterType<TItem>().SingleInstance();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(autofacLifetime), autofacLifetime, null);
            }
            
            return base.Use<TItem>();
        }
    }
}

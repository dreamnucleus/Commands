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
            return Use<TItem>(Lifetime.Pipeline);
        }

        public ICommandsBuilder Use<TItem>(Lifetime lifetime)
            where TItem : IUseCommandsBuilder
        {
            switch (lifetime)
            {
                case Lifetime.Dependency:
                    _containerBuilder.RegisterType<TItem>().InstancePerDependency();
                    break;
                case Lifetime.Pipeline:
                    _containerBuilder.RegisterType<TItem>().InstancePerLifetimeScope();
                    break;
                case Lifetime.Singleton:
                    _containerBuilder.RegisterType<TItem>().SingleInstance();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(lifetime), lifetime, null);
            }
            
            return base.Use<TItem>();
        }
    }
}

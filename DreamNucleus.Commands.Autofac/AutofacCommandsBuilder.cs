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
            return Use<TItem>(InstanceLifetime.Pipeline);
        }

        public ICommandsBuilder Use<TItem>(InstanceLifetime instanceLifetime)
            where TItem : IUseCommandsBuilder
        {
            switch (instanceLifetime)
            {
                case InstanceLifetime.Dependency:
                    _containerBuilder.RegisterType<TItem>().InstancePerDependency();
                    break;
                case InstanceLifetime.Pipeline:
                    _containerBuilder.RegisterType<TItem>().InstancePerLifetimeScope();
                    break;
                case InstanceLifetime.Singleton:
                    _containerBuilder.RegisterType<TItem>().SingleInstance();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(instanceLifetime), instanceLifetime, null);
            }
            
            return base.Use<TItem>();
        }
    }
}

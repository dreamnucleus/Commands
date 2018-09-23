using System;
using Autofac;

namespace DreamNucleus.Commands.Autofac
{
    public sealed class AutofacDependencyService : ILifetimeScopeDependencyService
    {
        private readonly ILifetimeScope _lifetimeScope;

        public AutofacDependencyService(ILifetimeScope lifetimeScope)
        {
            _lifetimeScope = lifetimeScope;
        }

        public bool IsRegistered<T>()
        {
            return _lifetimeScope.IsRegistered<T>();
        }

        public bool IsRegistered(Type type)
        {
            return _lifetimeScope.IsRegistered(type);
        }

        public T Resolve<T>()
        {
            return _lifetimeScope.Resolve<T>();
        }

        public object Resolve(Type type)
        {
            return _lifetimeScope.Resolve(type);
        }

        public void Dispose()
        {
            _lifetimeScope.Dispose();
        }
    }

    public sealed class AutofacLifetimeScopeService : ILifetimeScopeService
    {
        private readonly ILifetimeScope _lifetimeScope;

        public AutofacLifetimeScopeService(ILifetimeScope lifetimeScope)
        {
            _lifetimeScope = lifetimeScope;
        }

        public ILifetimeScopeDependencyService BeginLifetimeScope(ICommandProcessor commandProcessor)
        {
            return new AutofacDependencyService(_lifetimeScope.BeginLifetimeScope(c =>
            {
                c.RegisterInstance(commandProcessor).As<ICommandProcessor>().ExternallyOwned();
            }));
        }
    }
}

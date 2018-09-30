using System;
using Autofac;
using Autofac.Core.Registration;

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
            try
            {
                return _lifetimeScope.Resolve<T>();
            }
            catch (ComponentNotRegisteredException componentNotRegisteredException)
            {
                throw new DependencyNotRegisteredException(typeof(T), componentNotRegisteredException);
            }
            catch (global::Autofac.Core.DependencyResolutionException dependencyResolutionException)
            {
                throw new DependencyResolutionException(typeof(T), dependencyResolutionException);
            }
        }

        public object Resolve(Type type)
        {
            try
            {
                return _lifetimeScope.Resolve(type);
            }
            catch (ComponentNotRegisteredException componentNotRegisteredException)
            {
                throw new DependencyNotRegisteredException(type, componentNotRegisteredException);
            }
            catch (global::Autofac.Core.DependencyResolutionException dependencyResolutionException)
            {
                throw new DependencyResolutionException(type, dependencyResolutionException);
            }
        }

        public T Resolve<T>(Type parameterType, object parameter)
        {
            try
            {
                return _lifetimeScope.Resolve<T>(new TypedParameter(parameterType, parameter));
            }
            catch (ComponentNotRegisteredException componentNotRegisteredException)
            {
                throw new DependencyNotRegisteredException(typeof(T), componentNotRegisteredException);
            }
            catch (global::Autofac.Core.DependencyResolutionException dependencyResolutionException)
            {
                throw new DependencyResolutionException(typeof(T), dependencyResolutionException);
            }
        }

        public object Resolve(Type type, Type parameterType, object parameter)
        {
            try
            {
                return _lifetimeScope.Resolve(type, new TypedParameter(parameterType, parameter));
            }
            catch (ComponentNotRegisteredException componentNotRegisteredException)
            {
                throw new DependencyNotRegisteredException(type, componentNotRegisteredException);
            }
            catch (global::Autofac.Core.DependencyResolutionException dependencyResolutionException)
            {
                throw new DependencyResolutionException(type, dependencyResolutionException);
            }
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

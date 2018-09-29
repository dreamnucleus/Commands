using System;

namespace DreamNucleus.Commands
{
    public interface ILifetimeScopeDependencyService : IDisposable
    {
        bool IsRegistered<T>();
        bool IsRegistered(Type type);
        T Resolve<T>();
        object Resolve(Type type);
        object Resolve(Type type, Type parameterType, object parameter);
    }

    public interface ILifetimeScopeService
    {
        ILifetimeScopeDependencyService BeginLifetimeScope(ICommandProcessor commandProcessor);
    }
}

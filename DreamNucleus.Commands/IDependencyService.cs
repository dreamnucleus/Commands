using System;

namespace DreamNucleus.Commands
{
    public interface ILifetimeScopeDependencyService : IDisposable
    {
        bool IsRegistered<T>();
        bool IsRegistered(Type type);
        T Resolve<T>();
        object Resolve(Type type);
    }

    public interface ILifetimeScopeService
    {
        ILifetimeScopeDependencyService BeginLifetimeScope(ICommandProcessor commandProcessor);
    }
}

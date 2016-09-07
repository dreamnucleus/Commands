using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slipstream.CommonDotNet.Commands
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
        // TODO: remove
        ILifetimeScopeDependencyService BeginLifetimeScope();
        ILifetimeScopeDependencyService BeginLifetimeScope(ICommandProcessor commandProcessor);
    }
}

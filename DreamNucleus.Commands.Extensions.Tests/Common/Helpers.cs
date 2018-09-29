using Autofac;
using DreamNucleus.Commands.Autofac;
using DreamNucleus.Commands.Extensions.Cache;
using DreamNucleus.Commands.Extensions.Retry;
using DreamNucleus.Commands.Extensions.Semaphore;
using DreamNucleus.Commands.Extensions.Unique;
using DreamNucleus.Commands.Results;

namespace DreamNucleus.Commands.Extensions.Tests.Common
{
    public static class Helpers
    {
        public static ICommandProcessor CreateDefaultCommandProcessor()
        {
            var containerBuilder = new ContainerBuilder();

            containerBuilder.RegisterType<FakeLockManager>().As<ILockManager>().SingleInstance();
            containerBuilder.RegisterType<FakeUniqueManager>().As<IUniqueManager>().SingleInstance();
            containerBuilder.RegisterType<FakeCacheManager>().As<ICacheManager>().SingleInstance();

            containerBuilder.RegisterType<SemaphoreCommandHandler>().As<IAsyncCommandHandler<SemaphoreCommand, Unit>>();
            containerBuilder.RegisterType<SemaphoreHashCommandHandler>().As<IAsyncCommandHandler<SemaphoreHashCommand, int>>();
            containerBuilder.RegisterType<UniqueCommandHandler>().As<IAsyncCommandHandler<UniqueCommand, Unit>>();
            containerBuilder.RegisterType<CacheCommandHandler>().As<IAsyncCommandHandler<CacheCommand, int>>();
            containerBuilder.RegisterType<RetryCommandHandler>().As<IAsyncCommandHandler<RetryCommand, Unit>>();

            var commandsBuilder = new AutofacCommandsBuilder(containerBuilder);

            commandsBuilder.Use<SemaphorePipeline>();
            commandsBuilder.Use<UniquePipeline>();

            commandsBuilder.Use<CacheExecutingPipeline>();
            commandsBuilder.Use<CacheExecutedPipeline>();

            commandsBuilder.Use<RetryExceptionPipeline>();

            containerBuilder.RegisterInstance(commandsBuilder).SingleInstance();
            containerBuilder.RegisterType<AutofacLifetimeScopeService>().As<ILifetimeScopeService>();
            containerBuilder.RegisterType<CommandProcessor>().As<ICommandProcessor>();

            var container = containerBuilder.Build();

            return container.Resolve<ICommandProcessor>();
        }
    }
}

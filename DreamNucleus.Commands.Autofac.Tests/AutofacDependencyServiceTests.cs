using Autofac;
using DreamNucleus.Commands.Autofac.Tests.Common;
using Xunit;

namespace DreamNucleus.Commands.Autofac.Tests
{
    public class AutofacDependencyServiceTests
    {
        [Fact]
        public void IsRegistered_Register_ReturnsTrue()
        {
            var lifetimeScopeService = Helpers.CreateLifetimeScopeService(containerBuilderAction: containerBuilder =>
                {
                    containerBuilder.RegisterType<DependencyObject>();
                });

            using (var lifetimeScopeDependencyService = lifetimeScopeService.BeginLifetimeScope(new MockCommandProcessor()))
            {
                Assert.True(lifetimeScopeDependencyService.IsRegistered(typeof(DependencyObject)));
                Assert.True(lifetimeScopeDependencyService.IsRegistered<DependencyObject>());
            }
        }

        [Fact]
        public void IsRegistered_DoNotRegister_ReturnsFalse()
        {
            var lifetimeScopeService = Helpers.CreateLifetimeScopeService();

            using (var lifetimeScopeDependencyService = lifetimeScopeService.BeginLifetimeScope(new MockCommandProcessor()))
            {
                Assert.False(lifetimeScopeDependencyService.IsRegistered(typeof(DependencyObject)));
                Assert.False(lifetimeScopeDependencyService.IsRegistered<DependencyObject>());
            }
        }

        [Fact]
        public void Resolve_Register_ReturnsObject()
        {
            var lifetimeScopeService = Helpers.CreateLifetimeScopeService(containerBuilderAction: containerBuilder =>
            {
                containerBuilder.RegisterType<DependencyObject>();
            });

            using (var lifetimeScopeDependencyService = lifetimeScopeService.BeginLifetimeScope(new MockCommandProcessor()))
            {
                Assert.Equal(typeof(DependencyObject), lifetimeScopeDependencyService.Resolve(typeof(DependencyObject)).GetType());
                Assert.Equal(typeof(DependencyObject), lifetimeScopeDependencyService.Resolve<DependencyObject>().GetType());
            }
        }

        [Fact]
        public void Resolve_RegisterDoNotRegisterDependency_ThrowsDependencyResolutionException()
        {
            var lifetimeScopeService = Helpers.CreateLifetimeScopeService(containerBuilderAction: containerBuilder =>
            {
                containerBuilder.RegisterType<DependentObject>();
            });

            using (var lifetimeScopeDependencyService = lifetimeScopeService.BeginLifetimeScope(new MockCommandProcessor()))
            {
                Assert.Throws<DependencyResolutionException>(() => lifetimeScopeDependencyService.Resolve(typeof(DependentObject)));
                Assert.Throws<DependencyResolutionException>(() => lifetimeScopeDependencyService.Resolve<DependentObject>());
            }
        }

        [Fact]
        public void Resolve_DoNotRegister_ThrowsDependencyNotRegisteredException()
        {
            var lifetimeScopeService = Helpers.CreateLifetimeScopeService();

            using (var lifetimeScopeDependencyService = lifetimeScopeService.BeginLifetimeScope(new MockCommandProcessor()))
            {
                Assert.Throws<DependencyNotRegisteredException>(() => lifetimeScopeDependencyService.Resolve(typeof(DependencyObject)));
                Assert.Throws<DependencyNotRegisteredException>(() => lifetimeScopeDependencyService.Resolve<DependencyObject>());
            }
        }

        [Fact]
        public void ResolveWithParameter_Register_ReturnsObject()
        {
            var lifetimeScopeService = Helpers.CreateLifetimeScopeService(containerBuilderAction: containerBuilder =>
            {
                containerBuilder.RegisterType<DependentObject>();
            });

            using (var lifetimeScopeDependencyService = lifetimeScopeService.BeginLifetimeScope(new MockCommandProcessor()))
            {
                Assert.Equal(typeof(DependentObject), lifetimeScopeDependencyService.Resolve(typeof(DependentObject), typeof(DependencyObject), new DependencyObject()).GetType());
                Assert.Equal(typeof(DependentObject), lifetimeScopeDependencyService.Resolve<DependentObject>(typeof(DependencyObject), new DependencyObject()).GetType());
            }
        }

        [Fact]
        public void ResolveWithParameter_DoNotRegister_ThrowsDependencyNotRegisteredException()
        {
            var lifetimeScopeService = Helpers.CreateLifetimeScopeService();

            using (var lifetimeScopeDependencyService = lifetimeScopeService.BeginLifetimeScope(new MockCommandProcessor()))
            {
                Assert.Throws<DependencyNotRegisteredException>(() => lifetimeScopeDependencyService.Resolve(typeof(DependentObject), typeof(DependencyObject), new DependencyObject()));
                Assert.Throws<DependencyNotRegisteredException>(() => lifetimeScopeDependencyService.Resolve<DependentObject>(typeof(DependencyObject), new DependencyObject()));
            }
        }

        [Fact]
        public void ResolveWithParameter_RegisterIncorrectParameter_ThrowsDependencyResolutionException()
        {
            var lifetimeScopeService = Helpers.CreateLifetimeScopeService(containerBuilderAction: containerBuilder =>
            {
                containerBuilder.RegisterType<DependentObject>();
            });

            using (var lifetimeScopeDependencyService = lifetimeScopeService.BeginLifetimeScope(new MockCommandProcessor()))
            {
                Assert.Throws<DependencyResolutionException>(() => lifetimeScopeDependencyService.Resolve(typeof(DependentObject), typeof(object), new object()));
                Assert.Throws<DependencyResolutionException>(() => lifetimeScopeDependencyService.Resolve<DependentObject>(typeof(object), new object()));
            }
        }
    }
}

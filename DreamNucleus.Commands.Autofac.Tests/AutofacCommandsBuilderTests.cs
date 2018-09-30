using System;
using DreamNucleus.Commands.Autofac.Tests.Common;
using Xunit;

namespace DreamNucleus.Commands.Autofac.Tests
{
    public class AutofacCommandsBuilderTests
    {
        [Fact]
        public void Use_LifetimeDependency_NewInstancePerResolve()
        {
            var lifetimeScopeService = Helpers.CreateLifetimeScopeService(autofacCommandsBuilderAction: autofacCommandsBuilder =>
            {
                autofacCommandsBuilder.Use<DependencyPipeline>(Lifetime.Dependency);
            });

            DependencyPipeline pipeline1;
            DependencyPipeline pipeline2;
            using (var lifetimeScopeDependencyService = lifetimeScopeService.BeginLifetimeScope(new MockCommandProcessor()))
            {
                pipeline1 = lifetimeScopeDependencyService.Resolve<DependencyPipeline>();
                pipeline2 = lifetimeScopeDependencyService.Resolve<DependencyPipeline>();
            }

            Assert.True(pipeline1 != pipeline2);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void Use_LifetimePipeline_SameInstanceInPipeline(bool useDefaultUse)
        {
            var lifetimeScopeService = Helpers.CreateLifetimeScopeService(autofacCommandsBuilderAction: autofacCommandsBuilder =>
            {
                if (useDefaultUse)
                {
                    autofacCommandsBuilder.Use<DependencyPipeline>();
                }
                else
                {
                    autofacCommandsBuilder.Use<DependencyPipeline>(Lifetime.Pipeline);
                }
            });

            DependencyPipeline pipeline1;
            DependencyPipeline pipeline2;
            using (var lifetimeScopeDependencyService = lifetimeScopeService.BeginLifetimeScope(new MockCommandProcessor()))
            {
                pipeline1 = lifetimeScopeDependencyService.Resolve<DependencyPipeline>();
                pipeline2 = lifetimeScopeDependencyService.Resolve<DependencyPipeline>();
            }

            Assert.True(pipeline1 == pipeline2);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void Use_LifetimePipeline_NewInstancePerPipeline(bool useDefaultUse)
        {
            var lifetimeScopeService = Helpers.CreateLifetimeScopeService(autofacCommandsBuilderAction: autofacCommandsBuilder =>
            {
                if (useDefaultUse)
                {
                    autofacCommandsBuilder.Use<DependencyPipeline>();
                }
                else
                {
                    autofacCommandsBuilder.Use<DependencyPipeline>(Lifetime.Pipeline);
                }
            });

            DependencyPipeline pipeline1;
            using (var lifetimeScopeDependencyService = lifetimeScopeService.BeginLifetimeScope(new MockCommandProcessor()))
            {
                pipeline1 = lifetimeScopeDependencyService.Resolve<DependencyPipeline>();
            }

            DependencyPipeline pipeline2;
            using (var lifetimeScopeDependencyService = lifetimeScopeService.BeginLifetimeScope(new MockCommandProcessor()))
            {
                pipeline2 = lifetimeScopeDependencyService.Resolve<DependencyPipeline>();
            }

            Assert.True(pipeline1 != pipeline2);
        }

        [Fact]
        public void Use_LifetimeSingleton_SingleInstance()
        {
            var lifetimeScopeService = Helpers.CreateLifetimeScopeService(autofacCommandsBuilderAction: autofacCommandsBuilder =>
                {
                    autofacCommandsBuilder.Use<DependencyPipeline>(Lifetime.Singleton);
                });

            DependencyPipeline pipeline1;
            DependencyPipeline pipeline2;
            using (var lifetimeScopeDependencyService = lifetimeScopeService.BeginLifetimeScope(new MockCommandProcessor()))
            {
                pipeline1 = lifetimeScopeDependencyService.Resolve<DependencyPipeline>();
                pipeline2 = lifetimeScopeDependencyService.Resolve<DependencyPipeline>();
            }

            DependencyPipeline pipeline3;
            using (var lifetimeScopeDependencyService = lifetimeScopeService.BeginLifetimeScope(new MockCommandProcessor()))
            {
                pipeline3 = lifetimeScopeDependencyService.Resolve<DependencyPipeline>();
            }

            Assert.True(pipeline1 == pipeline2);
            Assert.True(pipeline2 == pipeline3);
        }
    }
}

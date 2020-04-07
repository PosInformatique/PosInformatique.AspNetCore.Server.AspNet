//-----------------------------------------------------------------------
// <copyright file="WebHostBuilderAspNetExtensionsTest.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace PosInformatique.AspNetCore.Server.AspNet.Tests
{
    using System;
    using System.Linq;
    using FluentAssertions;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Hosting.Server;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Options;
    using Moq;
    using Xunit;

    public class WebHostBuilderAspNetExtensionsTest
    {
        [Fact]
        public void UseAspNet()
        {
            var webHostBuildReturned = Mock.Of<IWebHostBuilder>();

            var serviceCollection = new ServiceCollection();

            var builder = new Mock<IWebHostBuilder>(MockBehavior.Strict);
            builder.Setup(b => b.ConfigureServices(It.IsAny<Action<IServiceCollection>>()))
                .Callback((Action<IServiceCollection> configureServices) =>
                {
                    configureServices(serviceCollection);
                })
                .Returns(webHostBuildReturned);

            WebHostBuilderAspNetExtensions.UseAspNet(builder.Object, options => { options.Routes.Add("The route"); }).Should().BeSameAs(webHostBuildReturned);

            var serviceDescriptor = serviceCollection.Single(sd => sd.ServiceType == typeof(IServer));
            serviceDescriptor.ImplementationType.Should().BeSameAs(typeof(AspNetServer));
            serviceDescriptor.Lifetime.Should().Be(ServiceLifetime.Singleton);

            var optionsBuilt = serviceCollection.BuildServiceProvider().GetService<IOptions<AspNetServerOptions>>().Value;
            optionsBuilt.Routes.Should().Equal("The route");
        }

        [Fact]
        public void UseAspNet_WithNullBuilder_ExceptionThrown()
        {
            Action act = () =>
            {
                WebHostBuilderAspNetExtensions.UseAspNet(null, null);
            };

            act.Should().ThrowExactly<ArgumentNullException>()
                .And.ParamName.Should().Be("builder");
        }

        [Fact]
        public void UseAspNet_WithNullOptions_ExceptionThrown()
        {
            Action act = () =>
            {
                WebHostBuilderAspNetExtensions.UseAspNet(Mock.Of<IWebHostBuilder>(), null);
            };

            act.Should().ThrowExactly<ArgumentNullException>()
                .And.ParamName.Should().Be("options");
        }
    }
}

//-----------------------------------------------------------------------
// <copyright file="HttpResponseFeatureTest.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace PosInformatique.AspNetCore.Server.AspNet.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Collections.Specialized;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web;
    using FluentAssertions;
    using Microsoft.AspNetCore.Http;
    using Moq;
    using Xunit;

    public class HttpResponseFeatureTest
    {
        [Fact]
        public void StatusCode_Get()
        {
            var response = new Mock<HttpResponseBase>(MockBehavior.Strict);
            response.Setup(r => r.StatusCode)
                .Returns(1234);

            var responseFeature = new HttpResponseFeature(response.Object);
            responseFeature.StatusCode.Should().Be(1234);
        }

        [Fact]
        public void StatusCode_Set()
        {
            var response = new Mock<HttpResponseBase>(MockBehavior.Strict);
            response.SetupSet(r => r.StatusCode = 1234);

            var responseFeature = new HttpResponseFeature(response.Object);
            responseFeature.StatusCode = 1234;

            response.VerifySet(r => r.StatusCode = 1234);
        }

        [Fact]
        public void ReasonPhrase_Get()
        {
            var response = new Mock<HttpResponseBase>(MockBehavior.Strict);
            response.Setup(r => r.StatusDescription)
                .Returns("The status");

            var responseFeature = new HttpResponseFeature(response.Object);
            responseFeature.ReasonPhrase.Should().Be("The status");
        }

        [Fact]
        public void ReasonPhrase_Set()
        {
            var response = new Mock<HttpResponseBase>(MockBehavior.Strict);
            response.SetupSet(r => r.StatusDescription = "The status");

            var responseFeature = new HttpResponseFeature(response.Object);
            responseFeature.ReasonPhrase = "The status";

            response.VerifySet(r => r.StatusDescription = "The status");
        }

        [Fact]
        public void Headers_Get()
        {
            var headers = new NameValueCollection()
            {
                { "Header#1", "Value #1" },
                { "Header#2", "Value #2" },
            };

            var response = new Mock<HttpResponseBase>(MockBehavior.Strict);
            response.Setup(r => r.Headers)
                .Returns(headers);

            var responseFeature = new HttpResponseFeature(response.Object);
            responseFeature.Headers.Should().BeOfType<HttpResponseHeaderDictionary>();
            responseFeature.Headers["Header#1"].ToString().Should().Be("Value #1");
            responseFeature.Headers["Header#2"].ToString().Should().Be("Value #2");
        }

        [Fact]
        public void Headers_Set()
        {
            var response = new Mock<HttpResponseBase>(MockBehavior.Strict);

            var responseFeature = new HttpResponseFeature(response.Object);

            responseFeature.Invoking(rf => rf.Headers = Mock.Of<IHeaderDictionary>()).Should().ThrowExactly<NotSupportedException>()
                .WithMessage("Unable to change the response headers of the response.\r\nPlease contact the community of the 'PosInformatique/PosInformatique.AspNetCore.Server.AspNet' project for more information (https://github.com/PosInformatique/PosInformatique.AspNetCore.Server.AspNet).");
        }

        [Fact]
        public void Body_Get()
        {
            var stream = Mock.Of<Stream>();

            var response = new Mock<HttpResponseBase>(MockBehavior.Strict);
            response.Setup(r => r.OutputStream)
                .Returns(stream);

            var responseFeature = new HttpResponseFeature(response.Object);
            responseFeature.Body.Should().BeSameAs(stream);
        }

        [Fact]
        public void Body_Set()
        {
            var response = new Mock<HttpResponseBase>(MockBehavior.Strict);

            var responseFeature = new HttpResponseFeature(response.Object);

            responseFeature.Invoking(rf => rf.Body = Mock.Of<Stream>()).Should().ThrowExactly<NotSupportedException>()
                .WithMessage("Unable to change the response stream body of the response.\r\nPlease contact the community of the 'PosInformatique/PosInformatique.AspNetCore.Server.AspNet' project for more information (https://github.com/PosInformatique/PosInformatique.AspNetCore.Server.AspNet).");
        }

        [Fact]
        public async Task StartAsync()
        {
            var callback1Called = false;
            var callback2Called = false;

            Func<object, Task> callback1 = (state) =>
            {
                state.Should().Be(1111);
                callback1Called = true;

                return Task.CompletedTask;
            };

            Func<object, Task> callback2 = (state) =>
            {
                state.Should().Be("2222");
                callback2Called = true;

                return Task.CompletedTask;
            };

            var response = new Mock<HttpResponseBase>(MockBehavior.Strict);

            var responseFeature = new HttpResponseFeature(response.Object);
            responseFeature.OnStarting(callback1, 1111);
            responseFeature.OnStarting(callback2, "2222");

            await responseFeature.StartAsync();

            responseFeature.HasStarted.Should().BeTrue();
            callback1Called.Should().BeTrue();
            callback2Called.Should().BeTrue();
        }

        [Fact]
        public async Task CompleteAsync()
        {
            var callback1Called = false;
            var callback2Called = false;

            Func<object, Task> callback1 = (state) =>
            {
                state.Should().Be(1111);
                callback1Called = true;

                return Task.CompletedTask;
            };

            Func<object, Task> callback2 = (state) =>
            {
                state.Should().Be("2222");
                callback2Called = true;

                return Task.CompletedTask;
            };

            var response = new Mock<HttpResponseBase>(MockBehavior.Strict);

            var responseFeature = new HttpResponseFeature(response.Object);
            responseFeature.OnCompleted(callback1, 1111);
            responseFeature.OnCompleted(callback2, "2222");

            await responseFeature.CompleteAsync();

            callback1Called.Should().BeTrue();
            callback2Called.Should().BeTrue();
        }
    }
}

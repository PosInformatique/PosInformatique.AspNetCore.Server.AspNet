//-----------------------------------------------------------------------
// <copyright file="HttpRequestFeatureCollectionTest.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace PosInformatique.AspNetCore.Server.AspNet.Tests
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Collections.Specialized;
    using System.IO;
    using System.Linq;
    using System.Web;
    using FluentAssertions;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Http.Features;
    using Moq;
    using Xunit;

    public class HttpRequestFeatureCollectionTest
    {
        [Fact]
        public void Constructor()
        {
            var inputStream = Mock.Of<Stream>();
            var headers = new NameValueCollection()
            {
                { "Header#1", "Value #1" },
                { "Header#2", "Value #2" },
            };
            var serverVariables = new NameValueCollection()
            {
                { "SERVER_PROTOCOL", "The server protocol" },
            };

            var response = new Mock<HttpResponseBase>(MockBehavior.Strict);
            var request = new Mock<HttpRequestBase>(MockBehavior.Strict);
            request.SetupGet(r => r.InputStream)
                .Returns(inputStream);
            request.SetupGet(r => r.Headers)
                .Returns(headers);
            request.SetupGet(r => r.HttpMethod)
                .Returns("THE METHOD");
            request.SetupGet(r => r.Path)
                .Returns("The path");
            request.SetupGet(r => r.ApplicationPath)
                .Returns("The application path");
            request.SetupGet(r => r.ServerVariables)
                .Returns(serverVariables);
            request.SetupGet(r => r.Url)
                .Returns(new Uri("special://the_uri/?a=1&b=2"));

            var context = new Mock<HttpContextBase>(MockBehavior.Strict);
            context.Setup(c => c.Request)
                .Returns(request.Object);
            context.Setup(c => c.Response)
                .Returns(response.Object);

            var features = new FeatureCollection();

            var featureCollection = new HttpRequestFeatureCollection(context.Object, features);

            featureCollection.Body.Should().BeSameAs(inputStream);
            featureCollection.Headers.Count.Should().Be(2);
            featureCollection.Headers["Header#1"].ToString().Should().Be("Value #1");
            featureCollection.Headers["Header#2"].ToString().Should().Be("Value #2");
            featureCollection.Method.Should().Be("THE METHOD");
            featureCollection.Path.Should().Be("The path");
            featureCollection.PathBase.Should().Be("The application path");
            featureCollection.Protocol.Should().Be("The server protocol");
            featureCollection.QueryString.Should().Be("?a=1&b=2");
            featureCollection.RawTarget.Should().BeNull();
            featureCollection.Scheme.Should().Be("special");
            featureCollection.TraceIdentifier.Should().HaveLength(Guid.NewGuid().ToString().Length);
            featureCollection.Revision.Should().Be(3);
            featureCollection.Response.Should().NotBeNull();
            featureCollection.IsReadOnly.Should().BeFalse();

            featureCollection[typeof(IHttpRequestFeature)].Should().BeSameAs(featureCollection);
            featureCollection.Get<IHttpRequestFeature>().Should().BeSameAs(featureCollection);

            featureCollection[typeof(IHttpRequestIdentifierFeature)].Should().BeSameAs(featureCollection);
            featureCollection.Get<IHttpRequestIdentifierFeature>().Should().BeSameAs(featureCollection);

            featureCollection[typeof(IHttpResponseFeature)].Should().BeSameAs(featureCollection.Response);
            featureCollection.Get<IHttpResponseFeature>().Should().BeSameAs(featureCollection.Response);
        }

        [Fact]
        public void Protocol()
        {
            var inputStream = Mock.Of<Stream>();
            var headers = new NameValueCollection();
            var serverVariables = new NameValueCollection()
            {
                { "SERVER_PROTOCOL", "The server protocol" },
            };

            var response = new Mock<HttpResponseBase>(MockBehavior.Strict);
            var request = new Mock<HttpRequestBase>(MockBehavior.Strict);
            request.SetupGet(r => r.InputStream)
                .Returns(inputStream);
            request.SetupGet(r => r.Headers)
                .Returns(headers);
            request.SetupGet(r => r.HttpMethod)
                .Returns("THE METHOD");
            request.SetupGet(r => r.Path)
                .Returns("The path");
            request.SetupGet(r => r.ApplicationPath)
                .Returns("The application path");
            request.SetupGet(r => r.ServerVariables)
                .Returns(serverVariables);
            request.SetupGet(r => r.Url)
                .Returns(new Uri("special://the_uri/?a=1&b=2"));

            var context = new Mock<HttpContextBase>(MockBehavior.Strict);
            context.Setup(c => c.Request)
                .Returns(request.Object);
            context.Setup(c => c.Response)
                .Returns(response.Object);

            var features = new FeatureCollection();

            var featureCollection = new HttpRequestFeatureCollection(context.Object, features);

            featureCollection.Protocol = "The new protocol";
            featureCollection.Protocol.Should().Be("The new protocol");
        }

        [Fact]
        public void Scheme()
        {
            var inputStream = Mock.Of<Stream>();
            var headers = new NameValueCollection();
            var serverVariables = new NameValueCollection()
            {
                { "SERVER_PROTOCOL", "The server protocol" },
            };

            var response = new Mock<HttpResponseBase>(MockBehavior.Strict);
            var request = new Mock<HttpRequestBase>(MockBehavior.Strict);
            request.SetupGet(r => r.InputStream)
                .Returns(inputStream);
            request.SetupGet(r => r.Headers)
                .Returns(headers);
            request.SetupGet(r => r.HttpMethod)
                .Returns("THE METHOD");
            request.SetupGet(r => r.Path)
                .Returns("The path");
            request.SetupGet(r => r.ApplicationPath)
                .Returns("The application path");
            request.SetupGet(r => r.ServerVariables)
                .Returns(serverVariables);
            request.SetupGet(r => r.Url)
                .Returns(new Uri("special://the_uri/?a=1&b=2"));

            var context = new Mock<HttpContextBase>(MockBehavior.Strict);
            context.Setup(c => c.Request)
                .Returns(request.Object);
            context.Setup(c => c.Response)
                .Returns(response.Object);

            var features = new FeatureCollection();

            var featureCollection = new HttpRequestFeatureCollection(context.Object, features);

            featureCollection.Scheme = "The new scheme";
            featureCollection.Scheme.Should().Be("The new scheme");
        }

        [Fact]
        public void Method()
        {
            var inputStream = Mock.Of<Stream>();
            var headers = new NameValueCollection();
            var serverVariables = new NameValueCollection()
            {
                { "SERVER_PROTOCOL", "The server protocol" },
            };

            var response = new Mock<HttpResponseBase>(MockBehavior.Strict);
            var request = new Mock<HttpRequestBase>(MockBehavior.Strict);
            request.SetupGet(r => r.InputStream)
                .Returns(inputStream);
            request.SetupGet(r => r.Headers)
                .Returns(headers);
            request.SetupGet(r => r.HttpMethod)
                .Returns("THE METHOD");
            request.SetupGet(r => r.Path)
                .Returns("The path");
            request.SetupGet(r => r.ApplicationPath)
                .Returns("The application path");
            request.SetupGet(r => r.ServerVariables)
                .Returns(serverVariables);
            request.SetupGet(r => r.Url)
                .Returns(new Uri("special://the_uri/?a=1&b=2"));

            var context = new Mock<HttpContextBase>(MockBehavior.Strict);
            context.Setup(c => c.Request)
                .Returns(request.Object);
            context.Setup(c => c.Response)
                .Returns(response.Object);

            var features = new FeatureCollection();

            var featureCollection = new HttpRequestFeatureCollection(context.Object, features);

            featureCollection.Method = "The new method";
            featureCollection.Method.Should().Be("The new method");
        }

        [Fact]
        public void PathBase()
        {
            var inputStream = Mock.Of<Stream>();
            var headers = new NameValueCollection();
            var serverVariables = new NameValueCollection()
            {
                { "SERVER_PROTOCOL", "The server protocol" },
            };

            var response = new Mock<HttpResponseBase>(MockBehavior.Strict);
            var request = new Mock<HttpRequestBase>(MockBehavior.Strict);
            request.SetupGet(r => r.InputStream)
                .Returns(inputStream);
            request.SetupGet(r => r.Headers)
                .Returns(headers);
            request.SetupGet(r => r.HttpMethod)
                .Returns("THE METHOD");
            request.SetupGet(r => r.Path)
                .Returns("The path");
            request.SetupGet(r => r.ApplicationPath)
                .Returns("The application path");
            request.SetupGet(r => r.ServerVariables)
                .Returns(serverVariables);
            request.SetupGet(r => r.Url)
                .Returns(new Uri("special://the_uri/?a=1&b=2"));

            var context = new Mock<HttpContextBase>(MockBehavior.Strict);
            context.Setup(c => c.Request)
                .Returns(request.Object);
            context.Setup(c => c.Response)
                .Returns(response.Object);

            var features = new FeatureCollection();

            var featureCollection = new HttpRequestFeatureCollection(context.Object, features);

            featureCollection.PathBase = "The new path base";
            featureCollection.PathBase.Should().Be("The new path base");
        }

        [Fact]
        public void Path()
        {
            var inputStream = Mock.Of<Stream>();
            var headers = new NameValueCollection();
            var serverVariables = new NameValueCollection()
            {
                { "SERVER_PROTOCOL", "The server protocol" },
            };

            var response = new Mock<HttpResponseBase>(MockBehavior.Strict);
            var request = new Mock<HttpRequestBase>(MockBehavior.Strict);
            request.SetupGet(r => r.InputStream)
                .Returns(inputStream);
            request.SetupGet(r => r.Headers)
                .Returns(headers);
            request.SetupGet(r => r.HttpMethod)
                .Returns("THE METHOD");
            request.SetupGet(r => r.Path)
                .Returns("The path");
            request.SetupGet(r => r.ApplicationPath)
                .Returns("The application path");
            request.SetupGet(r => r.ServerVariables)
                .Returns(serverVariables);
            request.SetupGet(r => r.Url)
                .Returns(new Uri("special://the_uri/?a=1&b=2"));

            var context = new Mock<HttpContextBase>(MockBehavior.Strict);
            context.Setup(c => c.Request)
                .Returns(request.Object);
            context.Setup(c => c.Response)
                .Returns(response.Object);

            var features = new FeatureCollection();

            var featureCollection = new HttpRequestFeatureCollection(context.Object, features);

            featureCollection.Path = "The new path";
            featureCollection.Path.Should().Be("The new path");
        }

        [Fact]
        public void QueryString()
        {
            var inputStream = Mock.Of<Stream>();
            var headers = new NameValueCollection();
            var serverVariables = new NameValueCollection()
            {
                { "SERVER_PROTOCOL", "The server protocol" },
            };

            var response = new Mock<HttpResponseBase>(MockBehavior.Strict);
            var request = new Mock<HttpRequestBase>(MockBehavior.Strict);
            request.SetupGet(r => r.InputStream)
                .Returns(inputStream);
            request.SetupGet(r => r.Headers)
                .Returns(headers);
            request.SetupGet(r => r.HttpMethod)
                .Returns("THE METHOD");
            request.SetupGet(r => r.Path)
                .Returns("The path");
            request.SetupGet(r => r.ApplicationPath)
                .Returns("The application path");
            request.SetupGet(r => r.ServerVariables)
                .Returns(serverVariables);
            request.SetupGet(r => r.Url)
                .Returns(new Uri("special://the_uri/?a=1&b=2"));

            var context = new Mock<HttpContextBase>(MockBehavior.Strict);
            context.Setup(c => c.Request)
                .Returns(request.Object);
            context.Setup(c => c.Response)
                .Returns(response.Object);

            var features = new FeatureCollection();

            var featureCollection = new HttpRequestFeatureCollection(context.Object, features);

            featureCollection.QueryString = "The new query string";
            featureCollection.QueryString.Should().Be("The new query string");
        }

        [Fact]
        public void RawTarget()
        {
            var inputStream = Mock.Of<Stream>();
            var headers = new NameValueCollection();
            var serverVariables = new NameValueCollection()
            {
                { "SERVER_PROTOCOL", "The server protocol" },
            };

            var response = new Mock<HttpResponseBase>(MockBehavior.Strict);
            var request = new Mock<HttpRequestBase>(MockBehavior.Strict);
            request.SetupGet(r => r.InputStream)
                .Returns(inputStream);
            request.SetupGet(r => r.Headers)
                .Returns(headers);
            request.SetupGet(r => r.HttpMethod)
                .Returns("THE METHOD");
            request.SetupGet(r => r.Path)
                .Returns("The path");
            request.SetupGet(r => r.ApplicationPath)
                .Returns("The application path");
            request.SetupGet(r => r.ServerVariables)
                .Returns(serverVariables);
            request.SetupGet(r => r.Url)
                .Returns(new Uri("special://the_uri/?a=1&b=2"));

            var context = new Mock<HttpContextBase>(MockBehavior.Strict);
            context.Setup(c => c.Request)
                .Returns(request.Object);
            context.Setup(c => c.Response)
                .Returns(response.Object);

            var features = new FeatureCollection();

            var featureCollection = new HttpRequestFeatureCollection(context.Object, features);

            featureCollection.RawTarget = "The new raw target";
            featureCollection.RawTarget.Should().Be("The new raw target");
        }

        [Fact]
        public void Headers()
        {
            var inputStream = Mock.Of<Stream>();
            var headers = new NameValueCollection();
            var serverVariables = new NameValueCollection()
            {
                { "SERVER_PROTOCOL", "The server protocol" },
            };

            var response = new Mock<HttpResponseBase>(MockBehavior.Strict);
            var request = new Mock<HttpRequestBase>(MockBehavior.Strict);
            request.SetupGet(r => r.InputStream)
                .Returns(inputStream);
            request.SetupGet(r => r.Headers)
                .Returns(headers);
            request.SetupGet(r => r.HttpMethod)
                .Returns("THE METHOD");
            request.SetupGet(r => r.Path)
                .Returns("The path");
            request.SetupGet(r => r.ApplicationPath)
                .Returns("The application path");
            request.SetupGet(r => r.ServerVariables)
                .Returns(serverVariables);
            request.SetupGet(r => r.Url)
                .Returns(new Uri("special://the_uri/?a=1&b=2"));

            var context = new Mock<HttpContextBase>(MockBehavior.Strict);
            context.Setup(c => c.Request)
                .Returns(request.Object);
            context.Setup(c => c.Response)
                .Returns(response.Object);

            var features = new FeatureCollection();

            var featureCollection = new HttpRequestFeatureCollection(context.Object, features);

            var newHeaders = Mock.Of<IHeaderDictionary>();

            featureCollection.Headers = newHeaders;
            featureCollection.Headers.Should().BeSameAs(newHeaders);
        }

        [Fact]
        public void TraceIdentifier()
        {
            var inputStream = Mock.Of<Stream>();
            var headers = new NameValueCollection();
            var serverVariables = new NameValueCollection()
            {
                { "SERVER_PROTOCOL", "The server protocol" },
            };

            var response = new Mock<HttpResponseBase>(MockBehavior.Strict);
            var request = new Mock<HttpRequestBase>(MockBehavior.Strict);
            request.SetupGet(r => r.InputStream)
                .Returns(inputStream);
            request.SetupGet(r => r.Headers)
                .Returns(headers);
            request.SetupGet(r => r.HttpMethod)
                .Returns("THE METHOD");
            request.SetupGet(r => r.Path)
                .Returns("The path");
            request.SetupGet(r => r.ApplicationPath)
                .Returns("The application path");
            request.SetupGet(r => r.ServerVariables)
                .Returns(serverVariables);
            request.SetupGet(r => r.Url)
                .Returns(new Uri("special://the_uri/?a=1&b=2"));

            var context = new Mock<HttpContextBase>(MockBehavior.Strict);
            context.Setup(c => c.Request)
                .Returns(request.Object);
            context.Setup(c => c.Response)
                .Returns(response.Object);

            var features = new FeatureCollection();

            var featureCollection = new HttpRequestFeatureCollection(context.Object, features);

            featureCollection.TraceIdentifier = "The trace identifier";
            featureCollection.TraceIdentifier.Should().Be("The trace identifier");
        }

        [Fact]
        public void Body()
        {
            var inputStream = Mock.Of<Stream>();
            var headers = new NameValueCollection();
            var serverVariables = new NameValueCollection()
            {
                { "SERVER_PROTOCOL", "The server protocol" },
            };

            var response = new Mock<HttpResponseBase>(MockBehavior.Strict);
            var request = new Mock<HttpRequestBase>(MockBehavior.Strict);
            request.SetupGet(r => r.InputStream)
                .Returns(inputStream);
            request.SetupGet(r => r.Headers)
                .Returns(headers);
            request.SetupGet(r => r.HttpMethod)
                .Returns("THE METHOD");
            request.SetupGet(r => r.Path)
                .Returns("The path");
            request.SetupGet(r => r.ApplicationPath)
                .Returns("The application path");
            request.SetupGet(r => r.ServerVariables)
                .Returns(serverVariables);
            request.SetupGet(r => r.Url)
                .Returns(new Uri("special://the_uri/?a=1&b=2"));

            var context = new Mock<HttpContextBase>(MockBehavior.Strict);
            context.Setup(c => c.Request)
                .Returns(request.Object);
            context.Setup(c => c.Response)
                .Returns(response.Object);

            var features = new FeatureCollection();

            var featureCollection = new HttpRequestFeatureCollection(context.Object, features);

            var body = Mock.Of<Stream>();

            featureCollection.Body = body;
            featureCollection.Body.Should().BeSameAs(body);
        }

        [Fact]
        public void GetEnumerator()
        {
            var inputStream = Mock.Of<Stream>();
            var headers = new NameValueCollection();
            var serverVariables = new NameValueCollection()
            {
                { "SERVER_PROTOCOL", "The server protocol" },
            };

            var response = new Mock<HttpResponseBase>(MockBehavior.Strict);
            var request = new Mock<HttpRequestBase>(MockBehavior.Strict);
            request.SetupGet(r => r.InputStream)
                .Returns(inputStream);
            request.SetupGet(r => r.Headers)
                .Returns(headers);
            request.SetupGet(r => r.HttpMethod)
                .Returns("THE METHOD");
            request.SetupGet(r => r.Path)
                .Returns("The path");
            request.SetupGet(r => r.ApplicationPath)
                .Returns("The application path");
            request.SetupGet(r => r.ServerVariables)
                .Returns(serverVariables);
            request.SetupGet(r => r.Url)
                .Returns(new Uri("special://the_uri/?a=1&b=2"));

            var context = new Mock<HttpContextBase>(MockBehavior.Strict);
            context.Setup(c => c.Request)
                .Returns(request.Object);
            context.Setup(c => c.Response)
                .Returns(response.Object);

            var features = new FeatureCollection();

            var featureCollection = new HttpRequestFeatureCollection(context.Object, features);

            var enumerator = featureCollection.GetEnumerator();

            enumerator.MoveNext().Should().BeTrue();
            enumerator.Current.Key.Should().BeSameAs(typeof(IHttpRequestFeature));
            enumerator.Current.Value.Should().BeSameAs(featureCollection);

            enumerator.MoveNext().Should().BeTrue();
            enumerator.Current.Key.Should().BeSameAs(typeof(IHttpRequestIdentifierFeature));
            enumerator.Current.Value.Should().BeSameAs(featureCollection);

            enumerator.MoveNext().Should().BeTrue();
            enumerator.Current.Key.Should().BeSameAs(typeof(IHttpResponseFeature));
            enumerator.Current.Value.Should().BeSameAs(featureCollection.Response);

            enumerator.MoveNext().Should().BeFalse();
            enumerator.MoveNext().Should().BeFalse();
        }

        [Fact]
        public void GetEnumeratorNonGeneric()
        {
            var inputStream = Mock.Of<Stream>();
            var headers = new NameValueCollection();
            var serverVariables = new NameValueCollection()
            {
                { "SERVER_PROTOCOL", "The server protocol" },
            };

            var response = new Mock<HttpResponseBase>(MockBehavior.Strict);
            var request = new Mock<HttpRequestBase>(MockBehavior.Strict);
            request.SetupGet(r => r.InputStream)
                .Returns(inputStream);
            request.SetupGet(r => r.Headers)
                .Returns(headers);
            request.SetupGet(r => r.HttpMethod)
                .Returns("THE METHOD");
            request.SetupGet(r => r.Path)
                .Returns("The path");
            request.SetupGet(r => r.ApplicationPath)
                .Returns("The application path");
            request.SetupGet(r => r.ServerVariables)
                .Returns(serverVariables);
            request.SetupGet(r => r.Url)
                .Returns(new Uri("special://the_uri/?a=1&b=2"));

            var context = new Mock<HttpContextBase>(MockBehavior.Strict);
            context.Setup(c => c.Request)
                .Returns(request.Object);
            context.Setup(c => c.Response)
                .Returns(response.Object);

            var features = new FeatureCollection();

            var featureCollection = new HttpRequestFeatureCollection(context.Object, features);

            var enumerator = ((IEnumerable)featureCollection).GetEnumerator();

            enumerator.MoveNext().Should().BeTrue();
            enumerator.Current.As<KeyValuePair<Type, object>>().Key.Should().BeSameAs(typeof(IHttpRequestFeature));
            enumerator.Current.As<KeyValuePair<Type, object>>().Value.Should().BeSameAs(featureCollection);

            enumerator.MoveNext().Should().BeTrue();
            enumerator.Current.As<KeyValuePair<Type, object>>().Key.Should().BeSameAs(typeof(IHttpRequestIdentifierFeature));
            enumerator.Current.As<KeyValuePair<Type, object>>().Value.Should().BeSameAs(featureCollection);

            enumerator.MoveNext().Should().BeTrue();
            enumerator.Current.As<KeyValuePair<Type, object>>().Key.Should().BeSameAs(typeof(IHttpResponseFeature));
            enumerator.Current.As<KeyValuePair<Type, object>>().Value.Should().BeSameAs(featureCollection.Response);

            enumerator.MoveNext().Should().BeFalse();
            enumerator.MoveNext().Should().BeFalse();
        }

        [Fact]
        public void SetFeature()
        {
            var inputStream = Mock.Of<Stream>();
            var headers = new NameValueCollection();
            var serverVariables = new NameValueCollection()
            {
                { "SERVER_PROTOCOL", "The server protocol" },
            };

            var response = new Mock<HttpResponseBase>(MockBehavior.Strict);
            var request = new Mock<HttpRequestBase>(MockBehavior.Strict);
            request.SetupGet(r => r.InputStream)
                .Returns(inputStream);
            request.SetupGet(r => r.Headers)
                .Returns(headers);
            request.SetupGet(r => r.HttpMethod)
                .Returns("THE METHOD");
            request.SetupGet(r => r.Path)
                .Returns("The path");
            request.SetupGet(r => r.ApplicationPath)
                .Returns("The application path");
            request.SetupGet(r => r.ServerVariables)
                .Returns(serverVariables);
            request.SetupGet(r => r.Url)
                .Returns(new Uri("special://the_uri/?a=1&b=2"));

            var context = new Mock<HttpContextBase>(MockBehavior.Strict);
            context.Setup(c => c.Request)
                .Returns(request.Object);
            context.Setup(c => c.Response)
                .Returns(response.Object);

            var features = new FeatureCollection();

            var featureCollection = new HttpRequestFeatureCollection(context.Object, features);

            var feature = Mock.Of<IHttpUpgradeFeature>();

            featureCollection.Set(feature);
            featureCollection.Get<IHttpUpgradeFeature>().Should().BeSameAs(feature);
        }
    }
}

//-----------------------------------------------------------------------
// <copyright file="HttpResponseHeaderDictionaryTest.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace PosInformatique.AspNetCore.Server.AspNet.Tests
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Globalization;
    using System.Linq;
    using System.Web;
    using FluentAssertions;
    using Microsoft.Extensions.Primitives;
    using Moq;
    using Xunit;

    public class HttpResponseHeaderDictionaryTest
    {
        [Fact]
        public void Constructor()
        {
            var headers = new NameValueCollection()
            {
                { "Header#1", "Value #1-A" },
                { "Header#2", "Value #2" },
                { "Header#1", "Value #1-B" },
            };

            var response = new Mock<HttpResponseBase>(MockBehavior.Strict);
            response.SetupGet(r => r.Headers)
                .Returns(headers);

            var responseFeature = new HttpResponseHeaderDictionary(response.Object);

            responseFeature.Should().HaveCount(2);
            headers.Should().HaveCount(2);

            responseFeature.IsReadOnly.Should().BeFalse();
        }

        [Fact]
        public void Values()
        {
            var headers = new NameValueCollection()
            {
                { "Header#1", "Value #1-A" },
                { "Header#2", "Value #2" },
                { "Header#1", "Value #1-B" },
            };

            var response = new Mock<HttpResponseBase>(MockBehavior.Strict);
            response.SetupGet(r => r.Headers)
                .Returns(headers);

            var responseFeature = new HttpResponseHeaderDictionary(response.Object);

            responseFeature.Values.Should().Equal("Value #1-A,Value #1-B", "Value #2");
        }

        [Fact]
        public void Indexer()
        {
            var headers = new NameValueCollection()
            {
                { "Header#1", "Value #1-A" },
                { "Header#2", "Value #2" },
                { "Header#1", "Value #1-B" },
            };

            var response = new Mock<HttpResponseBase>(MockBehavior.Strict);
            response.SetupGet(r => r.Headers)
                .Returns(headers);

            var responseFeature = new HttpResponseHeaderDictionary(response.Object);

            responseFeature["Header#1"].Should().Equal("Value #1-A,Value #1-B");
            responseFeature["Header#2"].Should().Equal("Value #2");

            // ASP .NET Core => ASP .NET non-core
            responseFeature["FromASP.NETCore"] = "The new header";
            responseFeature["FromASP.NETCore"].Should().Equal("The new header");
            headers["FromASP.NETCore"].Should().Be("The new header");

            // ASP .NET non-core => ASP .NET Core
            headers["FromASP.NETNonCore"] = "The new header from ASP .NET non core";
            headers["FromASP.NETNonCore"].Should().Be("The new header from ASP .NET non core");
            responseFeature["FromASP.NETNonCore"].Should().Equal("The new header from ASP .NET non core");
        }

        [Fact]
        public void Indexer_WithContentType()
        {
            var headers = new NameValueCollection()
            {
                { "Header#1", "Value #1-A" },
                { "Header#2", "Value #2" },
            };

            var response = new Mock<HttpResponseBase>(MockBehavior.Strict);
            response.SetupGet(r => r.Headers)
                .Returns(headers);
            response.SetupSet(r => r.ContentType = "application/json");

            var responseFeature = new HttpResponseHeaderDictionary(response.Object);

            // ASP .NET Core => ASP .NET non-core
            responseFeature["Content-Type"] = "application/json";
            responseFeature["Content-Type"].Should().Equal("application/json");
            headers["Content-Type"].Should().Be("application/json");

            response.VerifySet(r => r.ContentType = "application/json");
        }

        [Fact]
        public void ContentLength()
        {
            var headers = new NameValueCollection()
            {
                { "Header#1", "Value #1-A" },
                { "Header#2", "Value #2" },
                { "Content-Length", "1234" },
            };

            var response = new Mock<HttpResponseBase>(MockBehavior.Strict);
            response.SetupGet(r => r.Headers)
                .Returns(headers);

            var responseFeature = new HttpResponseHeaderDictionary(response.Object);

            responseFeature.ContentLength.Should().Be(1234);

            // Changes the content length
            responseFeature.ContentLength = 1664;

            responseFeature.ContentLength.Should().Be(1664);
            responseFeature["Content-Length"].Should().Equal("1664");
            headers["Content-Length"].Should().Be("1664");
        }

        [Fact]
        public void ContentLength_WithNullValue()
        {
            var headers = new NameValueCollection()
            {
                { "Header#1", "Value #1-A" },
                { "Header#2", "Value #2" },
                { "Content-Length", "1234" },
            };

            var response = new Mock<HttpResponseBase>(MockBehavior.Strict);
            response.SetupGet(r => r.Headers)
                .Returns(headers);

            var responseFeature = new HttpResponseHeaderDictionary(response.Object);

            // Changes the content length to null
            responseFeature.ContentLength = null;

            responseFeature.ContentLength.Should().BeNull();
            responseFeature.ContainsKey("Content-Length").Should().BeFalse();
            headers.AllKeys.Contains("Content-Length").Should().BeFalse();
        }

        [Fact]
        public void Keys()
        {
            var headers = new NameValueCollection()
            {
                { "Header#1", "Value #1-A" },
                { "Header#2", "Value #2" },
                { "Header#1", "Value #1-B" },
            };

            var response = new Mock<HttpResponseBase>(MockBehavior.Strict);
            response.SetupGet(r => r.Headers)
                .Returns(headers);

            var responseFeature = new HttpResponseHeaderDictionary(response.Object);

            responseFeature.Keys.Should().Equal("Header#1", "Header#2");
        }

        [Fact]
        public void Add()
        {
            var headers = new NameValueCollection()
            {
                { "Header#1", "Value #1-A" },
                { "Header#2", "Value #2" },
                { "Header#1", "Value #1-B" },
            };

            var response = new Mock<HttpResponseBase>(MockBehavior.Strict);
            response.SetupGet(r => r.Headers)
                .Returns(headers);

            var responseFeature = new HttpResponseHeaderDictionary(response.Object);

            responseFeature.Add("MyHeader", "The header");
            responseFeature["MyHeader"].Should().Equal("The header");
            headers["MyHeader"].Should().Be("The header");
        }

        [Fact]
        public void Add_WithKeyValuePair()
        {
            var headers = new NameValueCollection()
            {
                { "Header#1", "Value #1-A" },
                { "Header#2", "Value #2" },
                { "Header#1", "Value #1-B" },
            };

            var response = new Mock<HttpResponseBase>(MockBehavior.Strict);
            response.SetupGet(r => r.Headers)
                .Returns(headers);

            var responseFeature = new HttpResponseHeaderDictionary(response.Object);

            responseFeature.Add(new KeyValuePair<string, StringValues>("MyHeader", "The header"));
            responseFeature["MyHeader"].Should().Equal("The header");
            headers["MyHeader"].Should().Be("The header");
        }

        [Fact]
        public void Clear()
        {
            var headers = new NameValueCollection()
            {
                { "Header#1", "Value #1-A" },
                { "Header#2", "Value #2" },
                { "Header#1", "Value #1-B" },
            };

            var response = new Mock<HttpResponseBase>(MockBehavior.Strict);
            response.SetupGet(r => r.Headers)
                .Returns(headers);

            var responseFeature = new HttpResponseHeaderDictionary(response.Object);

            responseFeature.Clear();
            responseFeature.Should().HaveCount(0);
        }

        [Fact]
        public void Remove()
        {
            var headers = new NameValueCollection()
            {
                { "Header#1", "Value #1-A" },
                { "Header#2", "Value #2" },
                { "Header#1", "Value #1-B" },
            };

            var response = new Mock<HttpResponseBase>(MockBehavior.Strict);
            response.SetupGet(r => r.Headers)
                .Returns(headers);

            var responseFeature = new HttpResponseHeaderDictionary(response.Object);

            responseFeature.Remove("Header#1").Should().BeTrue();
            responseFeature["Header#1"].Should().BeEmpty();
            responseFeature.Should().HaveCount(1);
            headers["Header#1"].Should().BeNull();
            headers.Should().HaveCount(1);

            responseFeature.Remove("Not exists").Should().BeFalse();
            responseFeature.Should().HaveCount(1);
            headers.Should().HaveCount(1);
        }

        [Fact]
        public void RemoveKeyAndValue()
        {
            var headers = new NameValueCollection()
            {
                { "Header#1", "Value #1-A" },
                { "Header#2", "Value #2" },
                { "Header#1", "Value #1-B" },
                { "Header#3", "Value #3" },
            };

            var response = new Mock<HttpResponseBase>(MockBehavior.Strict);
            response.SetupGet(r => r.Headers)
                .Returns(headers);

            var responseFeature = new HttpResponseHeaderDictionary(response.Object);

            responseFeature.Remove(new KeyValuePair<string, StringValues>("Header#1", "Value #1-A,Value #1-B")).Should().BeTrue();
            responseFeature["Header#1"].Should().BeEmpty();
            responseFeature.Should().HaveCount(2);
            headers["Header#1"].Should().BeNull();
            headers.Should().HaveCount(2);

            responseFeature.Remove(new KeyValuePair<string, StringValues>("Header#2", "Value #2")).Should().BeTrue();
            responseFeature["Header#2"].Should().BeEmpty();
            responseFeature.Should().HaveCount(1);
            headers["Header#2"].Should().BeNull();
            headers.Should().HaveCount(1);

            responseFeature.Remove(new KeyValuePair<string, StringValues>("Header#NotExists", "Value #1-A")).Should().BeFalse();
            responseFeature.Remove(new KeyValuePair<string, StringValues>("Header#3", "Other value")).Should().BeFalse();

            responseFeature.Should().HaveCount(1);
            headers.Should().HaveCount(1);
        }

        [Fact]
        public void Contains()
        {
            var headers = new NameValueCollection()
            {
                { "Header#1", "Value #1-A" },
                { "Header#2", "Value #2" },
                { "Header#1", "Value #1-B" },
            };

            var response = new Mock<HttpResponseBase>(MockBehavior.Strict);
            response.SetupGet(r => r.Headers)
                .Returns(headers);

            var responseFeature = new HttpResponseHeaderDictionary(response.Object);

            responseFeature.Contains(new KeyValuePair<string, StringValues>("Header#1", "Value #1-A,Value #1-B")).Should().BeTrue();
            responseFeature.Contains(new KeyValuePair<string, StringValues>("Header#2", "Value #2")).Should().BeTrue();
            responseFeature.Contains(new KeyValuePair<string, StringValues>("Header#NotExists", "Value #1-A")).Should().BeFalse();
            responseFeature.Contains(new KeyValuePair<string, StringValues>("Header#1", "Other value")).Should().BeFalse();
            responseFeature.Contains(new KeyValuePair<string, StringValues>("Header#2", "Other value")).Should().BeFalse();
            responseFeature.Contains(new KeyValuePair<string, StringValues>("Header#1", "Value #1-A")).Should().BeFalse();
            responseFeature.Contains(new KeyValuePair<string, StringValues>("Header#1", "Value #1-B")).Should().BeFalse();
        }

        [Fact]
        public void ContainsKey()
        {
            var headers = new NameValueCollection()
            {
                { "Header#1", "Value #1-A" },
                { "Header#2", "Value #2" },
                { "Header#1", "Value #1-B" },
            };

            var response = new Mock<HttpResponseBase>(MockBehavior.Strict);
            response.SetupGet(r => r.Headers)
                .Returns(headers);

            var responseFeature = new HttpResponseHeaderDictionary(response.Object);

            responseFeature.ContainsKey("Header#1").Should().BeTrue();
            responseFeature.ContainsKey("Header#2").Should().BeTrue();
            responseFeature.ContainsKey("Header#NotExists").Should().BeFalse();
        }

        [Fact]
        public void GetEnumerator_Generic()
        {
            var headers = new NameValueCollection()
            {
                { "Header#1", "Value #1-A" },
                { "Header#2", "Value #2" },
                { "Header#1", "Value #1-B" },
            };

            var response = new Mock<HttpResponseBase>(MockBehavior.Strict);
            response.SetupGet(r => r.Headers)
                .Returns(headers);

            var responseFeature = new HttpResponseHeaderDictionary(response.Object);

            var enumerator = responseFeature.GetEnumerator();

            enumerator.MoveNext().Should().BeTrue();
            enumerator.Current.Key.Should().Be("Header#1");
            enumerator.Current.Value.Should().Equal("Value #1-A,Value #1-B");

            enumerator.MoveNext().Should().BeTrue();
            enumerator.Current.Key.Should().Be("Header#2");
            enumerator.Current.Value.Should().Equal("Value #2");

            enumerator.MoveNext().Should().BeFalse();
            enumerator.MoveNext().Should().BeFalse();
        }

        [Fact]
        public void GetEnumerator_NonGeneric()
        {
            var headers = new NameValueCollection()
            {
                { "Header#1", "Value #1-A" },
                { "Header#2", "Value #2" },
                { "Header#1", "Value #1-B" },
            };

            var response = new Mock<HttpResponseBase>(MockBehavior.Strict);
            response.SetupGet(r => r.Headers)
                .Returns(headers);

            var responseFeature = new HttpResponseHeaderDictionary(response.Object);

            var enumerator = responseFeature.As<IEnumerable>().GetEnumerator();

            enumerator.MoveNext().Should().BeTrue();
            enumerator.Current.As<KeyValuePair<string, StringValues>>().Key.Should().Be("Header#1");
            enumerator.Current.As<KeyValuePair<string, StringValues>>().Value.Should().Equal("Value #1-A,Value #1-B");

            enumerator.MoveNext().Should().BeTrue();
            enumerator.Current.As<KeyValuePair<string, StringValues>>().Key.Should().Be("Header#2");
            enumerator.Current.As<KeyValuePair<string, StringValues>>().Value.Should().Equal("Value #2");

            enumerator.MoveNext().Should().BeFalse();
            enumerator.MoveNext().Should().BeFalse();
        }

        [Fact]
        public void TryGetValue()
        {
            var headers = new NameValueCollection()
            {
                { "Header#1", "Value #1-A" },
                { "Header#2", "Value #2" },
                { "Header#1", "Value #1-B" },
            };

            var response = new Mock<HttpResponseBase>(MockBehavior.Strict);
            response.SetupGet(r => r.Headers)
                .Returns(headers);

            var responseFeature = new HttpResponseHeaderDictionary(response.Object);

            responseFeature.TryGetValue("Header#1", out var valueFound1).Should().BeTrue();
            valueFound1.ToString().Should().Be("Value #1-A,Value #1-B");

            responseFeature.TryGetValue("Header#2", out var valueFound2).Should().BeTrue();
            valueFound2.ToString().Should().Be("Value #2");

            responseFeature.TryGetValue("Header#NotExists", out var valueFound3).Should().BeFalse();
            valueFound3.Should().BeEmpty();
        }

        [Fact]
        public void CopyTo()
        {
            var headers = new NameValueCollection()
            {
                { "Header#1", "Value #1-A" },
                { "Header#2", "Value #2" },
                { "Header#1", "Value #1-B" },
                { "Header#3", "Value #3" },
                { "Header#4", "Value #4" },
            };

            var response = new Mock<HttpResponseBase>(MockBehavior.Strict);
            response.SetupGet(r => r.Headers)
                .Returns(headers);

            var responseFeature = new HttpResponseHeaderDictionary(response.Object);

            var array = new KeyValuePair<string, StringValues>[10];

            responseFeature.CopyTo(array, 2);

            array.Should().Equal(
                new KeyValuePair<string, StringValues>(null, StringValues.Empty),
                new KeyValuePair<string, StringValues>(null, StringValues.Empty),
                new KeyValuePair<string, StringValues>("Header#1", "Value #1-A,Value #1-B"),
                new KeyValuePair<string, StringValues>("Header#2", "Value #2"),
                new KeyValuePair<string, StringValues>("Header#3", "Value #3"),
                new KeyValuePair<string, StringValues>("Header#4", "Value #4"),
                new KeyValuePair<string, StringValues>(null, StringValues.Empty),
                new KeyValuePair<string, StringValues>(null, StringValues.Empty),
                new KeyValuePair<string, StringValues>(null, StringValues.Empty),
                new KeyValuePair<string, StringValues>(null, StringValues.Empty));
        }
    }
}
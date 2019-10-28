//-----------------------------------------------------------------------
// <copyright file="HttpRequestFeatureCollection.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace PosInformatique.AspNetCore.Server.AspNet
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Web;
    using Microsoft.AspNetCore.Hosting.Server;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Http.Features;
    using Microsoft.Extensions.Primitives;

    /// <summary>
    /// Implementation of the <see cref="IFeatureCollection"/> used when creating the request context
    /// when calling the <see cref="IHttpApplication{TContext}.CreateContext(IFeatureCollection)"/> method.
    /// </summary>
    internal sealed class HttpRequestFeatureCollection : IFeatureCollection, IHttpRequestFeature, IHttpRequestIdentifierFeature
    {
        /// <summary>
        /// Collection of HTTP features of the server.
        /// </summary>
        private readonly FeatureCollection features;

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpRequestFeatureCollection"/> class.
        /// </summary>
        /// <param name="context">Current ASP .NET non-core <see cref="HttpContextBase"/> used to retrieve the properties
        /// values of the <see cref="IHttpRequestFeature"/>.</param>
        /// <param name="features">Default features of the server.</param>
        public HttpRequestFeatureCollection(HttpContextBase context, IFeatureCollection features)
        {
            this.Response = new HttpResponseFeature(context.Response);

            this.features = new FeatureCollection(features);
            this.features.Set<IHttpRequestFeature>(this);
            this.features.Set<IHttpRequestIdentifierFeature>(this);
            this.features.Set<IHttpResponseFeature>(this.Response);

            this.Body = context.Request.InputStream;
            this.Headers = new HeaderDictionary(context.Request.Headers.AllKeys.ToDictionary(key => key, key => (StringValues)context.Request.Headers[key]));
            this.Method = context.Request.HttpMethod;
            this.Path = context.Request.Path;
            this.PathBase = context.Request.ApplicationPath;
            this.Protocol = context.Request.ServerVariables["SERVER_PROTOCOL"];
            this.QueryString = context.Request.Url.Query;
            this.Scheme = context.Request.Url.Scheme;
            this.TraceIdentifier = Convert.ToString(Guid.NewGuid(), CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Gets the <see cref="HttpResponseFeature"/> implementation.
        /// </summary>
        public HttpResponseFeature Response
        {
            get;
        }

        /// <inheritdoc />
        public bool IsReadOnly
        {
            get => this.features.IsReadOnly;
        }

        /// <inheritdoc />
        public int Revision
        {
            get => this.features.Revision;
        }

        /// <inheritdoc />
        public string Protocol
        {
            get;
            set;
        }

        /// <inheritdoc />
        public string Scheme
        {
            get;
            set;
        }

        /// <inheritdoc />
        public string Method
        {
            get;
            set;
        }

        /// <inheritdoc />
        public string PathBase
        {
            get;
            set;
        }

        /// <inheritdoc />
        public string Path
        {
            get;
            set;
        }

        /// <inheritdoc />
        public string QueryString
        {
            get;
            set;
        }

        /// <inheritdoc />
        public string RawTarget
        {
            get;
            set;
        }

        /// <inheritdoc />
        public IHeaderDictionary Headers
        {
            get;
            set;
        }

        /// <inheritdoc />
        public string TraceIdentifier
        {
            get;
            set;
        }

        /// <inheritdoc />
        public Stream Body
        {
            get;
            set;
        }

        /// <inheritdoc />
        public object this[Type key]
        {
            get => this.features[key];
            set => this.features[key] = value;
        }

        /// <inheritdoc />
        public TFeature Get<TFeature>()
        {
            return this.features.Get<TFeature>();
        }

        /// <inheritdoc />
        public IEnumerator<KeyValuePair<Type, object>> GetEnumerator()
        {
            return this.features.GetEnumerator();
        }

        /// <inheritdoc />
        public void Set<TFeature>(TFeature instance)
        {
            this.features.Set<TFeature>(instance);
        }

        /// <inheritdoc />
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}

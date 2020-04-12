//-----------------------------------------------------------------------
// <copyright file="AspNetServer.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace PosInformatique.AspNetCore.Server.AspNet
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Web;
    using System.Web.Routing;
    using Microsoft.AspNetCore.Hosting.Server;
    using Microsoft.AspNetCore.Http.Features;
    using Microsoft.Extensions.Options;

    /// <summary>
    /// Implementation of the <see cref="IServer"/> to execute the current ASP .NET non-core request.
    /// </summary>
    internal sealed class AspNetServer : IServer
    {
        /// <summary>
        /// Options of the <see cref="AspNetServer"/>.
        /// </summary>
        private readonly IOptions<AspNetServerOptions> options;

        /// <summary>
        /// Initializes a new instance of the <see cref="AspNetServer"/> class.
        /// </summary>
        /// <param name="options">Options of the <see cref="AspNetServer"/>.</param>
        public AspNetServer(IOptions<AspNetServerOptions> options)
        {
            this.options = options;

            this.Features = new FeatureCollection();
        }

        /// <inheritdoc />
        public IFeatureCollection Features
        {
            get;
        }

        /// <inheritdoc />
        public void Dispose()
        {
            // Nothing to do.
        }

        /// <inheritdoc />
        public Task StartAsync<TContext>(IHttpApplication<TContext> application, CancellationToken cancellationToken)
        {
            var wrappedApplication = new HttpApplicationWrapper<TContext>(application);

            foreach (var route in this.options.Value.Routes)
            {
                RouteTable.Routes.Add(route, new Route($"{route}/{{*wildcard}}", new AspNetCoreRequestRouteHandler(wrappedApplication, this.Features)));
            }

            RouteTable.Routes.RouteExistingFiles = true;

            return Task.CompletedTask;
        }

        /// <inheritdoc />
        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        /// <summary>
        /// Implementation of <see cref="IRouteHandler"/> which will execute ASP .NET Core requests using the <see cref="AspNetCoreRequestHttpHandler"/>.
        /// </summary>
        private class AspNetCoreRequestRouteHandler : IRouteHandler
        {
            /// <summary>
            /// <see cref="AspNetCoreRequestHttpHandler"/> which will be use to execute the current ASP .NET request
            /// to the ASP .NET Core infrastructure.
            /// </summary>
            private readonly IHttpHandler handler;

            /// <summary>
            /// Initializes a new instance of the <see cref="AspNetCoreRequestRouteHandler"/> class.
            /// </summary>
            /// <param name="application">ASP .NET Core <see cref="IHttpApplication{TContext}"/> where the ASP .NET non-core will be requested.</param>
            /// <param name="features">Collection of HTTP features of the server.</param>
            public AspNetCoreRequestRouteHandler(IHttpApplication<object> application, IFeatureCollection features)
            {
                this.handler = new AspNetCoreRequestHttpHandler(application, features);
            }

            /// <inheritdoc />
            public IHttpHandler GetHttpHandler(RequestContext requestContext)
            {
                return this.handler;
            }
        }

        /// <summary>
        /// <see cref="IHttpApplication{TContext}"/> implementation wrapper used to call the <see cref="IHttpApplication{TContext}.ProcessRequestAsync(TContext)"/>
        /// method in the <see cref="AspNetCoreRequestHttpHandler"/> without knowing the <typeparamref name="TContext"/> type.
        /// </summary>
        /// <typeparam name="TContext">The context associated with the application.</typeparam>
        private class HttpApplicationWrapper<TContext> : IHttpApplication<object>
        {
            /// <summary>
            /// The <see cref="IHttpApplication{TContext}"/> wrapped.
            /// </summary>
            private readonly IHttpApplication<TContext> httpApplication;

            /// <summary>
            /// Initializes a new instance of the <see cref="HttpApplicationWrapper{TContext}"/> class.
            /// </summary>
            /// <param name="httpApplication">The <see cref="IHttpApplication{TContext}"/> wrapped.</param>
            public HttpApplicationWrapper(IHttpApplication<TContext> httpApplication)
            {
                this.httpApplication = httpApplication;
            }

            /// <inheritdoc />
            public object CreateContext(IFeatureCollection contextFeatures)
            {
                return this.httpApplication.CreateContext(contextFeatures);
            }

            /// <inheritdoc />
            public void DisposeContext(object context, Exception exception)
            {
                this.httpApplication.DisposeContext((TContext)context, exception);
            }

            /// <inheritdoc />
            public Task ProcessRequestAsync(object context)
            {
                return this.httpApplication.ProcessRequestAsync((TContext)context);
            }
        }
    }
}

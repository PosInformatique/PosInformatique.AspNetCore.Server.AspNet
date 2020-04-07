//-----------------------------------------------------------------------
// <copyright file="AspNetCoreRequestHttpHandler.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace PosInformatique.AspNetCore.Server.AspNet
{
    using System;
    using System.Threading.Tasks;
    using System.Web;
    using Microsoft.AspNetCore.Hosting.Server;
    using Microsoft.AspNetCore.Http.Features;

    /// <summary>
    /// Implementation of the <see cref="IHttpAsyncHandler"/> which use the current ASP .NET request
    /// and will execute to the ASP .NET Core infrastructure.
    /// </summary>
    internal sealed class AspNetCoreRequestHttpHandler : HttpTaskAsyncHandler
    {
        /// <summary>
        /// The <see cref="IHttpApplication{TContext}"/> wrapped.
        /// </summary>
        private readonly IHttpApplication<object> application;

        /// <summary>
        /// Collection of HTTP features of the server.
        /// </summary>
        private readonly IFeatureCollection features;

        /// <summary>
        /// Initializes a new instance of the <see cref="AspNetCoreRequestHttpHandler"/> class.
        /// </summary>
        /// <param name="application">ASP .NET Core <see cref="IHttpApplication{TContext}"/> where the ASP .NET non-core will be requested.</param>
        /// <param name="features">Collection of HTTP features of the server.</param>
        public AspNetCoreRequestHttpHandler(IHttpApplication<object> application, IFeatureCollection features)
        {
            this.application = application;
            this.features = features;
        }

        /// <inheritdoc />
        public override bool IsReusable
        {
            get => true;
        }

        /// <inheritdoc />
        public override async Task ProcessRequestAsync(HttpContext context)
        {
            // Creates the ASP .NET Core context using the HttpContext of ASP .NET non-core.
            var featureCollection = new HttpRequestFeatureCollection(new HttpContextWrapper(context), this.features);
            var aspNetCoreContext = this.application.CreateContext(featureCollection);

            try
            {
                try
                {
                    await this.application.ProcessRequestAsync(aspNetCoreContext);
                    await featureCollection.Response.StartAsync();
                }
                finally
                {
                    await featureCollection.Response.CompleteAsync();
                }

                this.application.DisposeContext(aspNetCoreContext, null);
            }
            catch (Exception exception)
            {
                this.application.DisposeContext(aspNetCoreContext, exception);
                throw;
            }
        }
    }
}

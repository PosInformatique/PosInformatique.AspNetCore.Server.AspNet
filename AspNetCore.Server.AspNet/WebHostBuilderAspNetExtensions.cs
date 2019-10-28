//-----------------------------------------------------------------------
// <copyright file="WebHostBuilderAspNetExtensions.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Microsoft.AspNetCore.Hosting
{
    using System;
    using Microsoft.AspNetCore.Hosting.Server;
    using Microsoft.Extensions.DependencyInjection;
    using PosInformatique.AspNetCore.Server.AspNet;

    /// <summary>
    /// Contains extensions methods to configure a <see cref="IWebHostBuilder"/> to host the
    /// ASP .NET Core infrastructure into a ASP .NET non-core infrastructure.
    /// </summary>
    public static class WebHostBuilderAspNetExtensions
    {
        /// <summary>
        /// Hosts the ASP .NET Core application into a ASP .NET non-core infrastructure.
        /// </summary>
        /// <param name="builder"><see cref="IWebHostBuilder"/> to configure.</param>
        /// <param name="options">Options of the ASP .NET non-core infrastructure.</param>
        /// <returns>The <paramref name="builder"/> to continue the configuration.</returns>
        /// <exception cref="ArgumentNullException">If the <paramref name="builder"/> argument is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException">If the <paramref name="options"/> argument is <see langword="null"/>.</exception>
        public static IWebHostBuilder UseAspNet(this IWebHostBuilder builder, Action<AspNetServerOptions> options)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            return builder.ConfigureServices(services =>
            {
                services.AddSingleton<IServer, AspNetServer>();
                services.Configure(options);
            });
        }
    }
}

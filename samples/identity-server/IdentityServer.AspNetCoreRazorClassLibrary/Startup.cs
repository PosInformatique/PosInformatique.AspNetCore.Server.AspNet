// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Security.Claims;
using IdentityModel;
using IdentityServer4.Test;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;

namespace IdentityServer.AspNetCoreRazorClassLibrary
{
    public class Startup
    {
        public IHostingEnvironment Environment { get; }

        public Startup(IHostingEnvironment environment)
        {
            Environment = environment;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            // uncomment, if you want to add an MVC-based UI
            services.AddMvc().SetCompatibilityVersion(Microsoft.AspNetCore.Mvc.CompatibilityVersion.Version_2_1);

            services.ConfigureOptions(typeof(WwwrootEmbeddedResourcesOptions));

            var builder = services.AddIdentityServer()
                .AddInMemoryIdentityResources(Config.GetIdentityResources())
                .AddInMemoryApiResources(Config.GetApis())
                .AddInMemoryClients(Config.GetClients())
                .AddTestUsers(new List<TestUser>
                {
                    new TestUser()
                    {
                        SubjectId = "1",
                        Username = "bob",
                        Password = "foobar",
                        Claims =
                        {
                            new Claim(JwtClaimTypes.Name, "Bob DYLAN"),
                            new Claim(JwtClaimTypes.GivenName, "Bob DYLAN"),
                            new Claim(JwtClaimTypes.FamilyName, "DYLAN"),
                            new Claim(JwtClaimTypes.Email, "bob@dylan.com"),
                            new Claim(JwtClaimTypes.EmailVerified, "true", ClaimValueTypes.Boolean),
                        }
                    }
                });

            builder.AddDeveloperSigningCredential(filename: $@"{Path.GetTempPath()}\IdentityServer.AspNetCoreRazorClassLibrary.rsa");
        }

        public void Configure(IApplicationBuilder app)
        {
            if (Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UsePathBase("/identity-server/");

            // uncomment if you want to support static files
            app.UseStaticFiles();

            app.UseIdentityServer();

            // uncomment, if you want to add an MVC-based UI
            app.UseMvcWithDefaultRoute();
        }

        private class WwwrootEmbeddedResourcesOptions : IPostConfigureOptions<StaticFileOptions>
        {
            public void PostConfigure(string name, StaticFileOptions options)
            {
                // Basic initialization in case the options weren't initialized by any other component
                options.ContentTypeProvider = options.ContentTypeProvider ?? new FileExtensionContentTypeProvider();

                // Add our provider
                var filesProvider = new ManifestEmbeddedFileProvider(this.GetType().Assembly, "wwwroot");
                options.FileProvider = new CompositeFileProvider(filesProvider);
            }
        }
    }
}

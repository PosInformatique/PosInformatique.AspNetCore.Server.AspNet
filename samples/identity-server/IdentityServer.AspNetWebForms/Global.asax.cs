using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;
using System.Web.SessionState;
using IdentityServer.AspNetCoreRazorClassLibrary;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace IdentityServer.AspNetWebForms
{
    public class Global : HttpApplication
    {
        private IWebHost webHost;

        void Application_Start(object sender, EventArgs e)
        {
            // Code that runs on application startup
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            this.webHost = WebHost.CreateDefaultBuilder()
                .UseAspNet(options =>
                {
                    options.Routes.Add("identity-server");
                })
                .UseStartup<Startup>()
                .Start();
        }

        protected void Application_End(object sender, EventArgs e)
        {
            if (this.webHost != null)
            {
                this.webHost.StopAsync().GetAwaiter().GetResult();
                this.webHost.Dispose();

                this.webHost = null;
            }
        }
    }
}
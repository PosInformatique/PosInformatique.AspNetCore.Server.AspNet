using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace PosInformatique.AspNetCore.Server.AspNet.IntegrationTests
{
    public class Global : System.Web.HttpApplication
    {
        protected void Application_Start(object sender, EventArgs e)
        {
            WebHost.CreateDefaultBuilder()
                .UseAspNet(options =>
                {
                    options.Routes.Add("api");
                    options.Routes.Add("swagger");
                })
                .UseStartup<Startup>()
                .Start();
        }
    }
}
# PosInformatique.AspNetCore.Server.AspNet
**PosInformatique.AspNetCore.Server.AspNet** is a library to host ASP .NET Core Web API on ASP .NET
non-core (WebForms and MVC) infrastructure based on the .NET Framework.

![Architecture](documentation/Architecture.png)

## Architecture

![Pipeline Execution](documentation/PipelineExecution.png)

The **PosInformatique.AspNetCore.Server.AspNet** is an ASP .NET non-core ``IHttpHandler``
which is executed depending of configured routes in the ASP .NET non-core infrastructure.
When the **PosInformatique.AspNetCore.Server.AspNet** ``IHttpHandler`` internal implementation
is called, the HTTP query is send to the ASP .NET Core infrastructure which execute
the query **with the same behavior** if it was hosted in a dedicated IIS, Console or Kestrel host.

## Installing from NuGet
The **PosInformatique.AspNetCore.Server.AspNet** is available directly on the
[NuGet](https://www.nuget.org/packages/PosInformatique.AspNetCore.Server.AspNet/) official website.
To download and install the library to your Visual Studio project using the following NuGet command line 
```
Install-Package PosInformatique.AspNetCore.Server.AspNet
```

## Setting up
In the an new ASP .NET Web Forms/MVC non-core project install the ASP .NET Core 2.x infrastructure
using the following NuGet command:
```
Install-Package Microsoft.AspNetCore
Install-Package Microsoft.AspNetCore.Mvc
```

After adding the **PosInformatique.AspNetCore.Server.AspNet** package on your ASP .NET
WebForms or MVC non-core project, inside the ``Application_Start`` of your `HttpApplication`
class builds and start an ASP .NET Core ``IWebHost`` instance using the classic
``WebHost.CreateDefaultBuilder()`` and related API
(``Startup`` class, additional services,...).
Instead to choose Kestrel or IIS to host your ASP .NET Core application,
call the ``UseAspNet()`` method to host your ASP .NET Core application
on the ASP .NET non-core infrastructure.

The ``UseAspNet()`` requires to defines the base routes which will be intercepted
and process by the ASP .NET Core infrastructure instead of ASP .NET non-core.

This is an example of a `Global.asax.cs` code behind which builds, start an ASP .NET
Core application and redirect all the request starting by ``/api`` and ``/swagger`` URLs
to the ASP .NET Core application:

```csharp
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
```

You can configure your ASP .NET Core application as usual with the `Startup` class
by registering additional services.
This is an example of a `Startup` class which register the ASP .NET Core MVC infrastructure
and the [Swashbuckle.AspNetCore](https://www.nuget.org/packages/Swashbuckle.AspNetCore)
extensions to add the documentation using Swagger UI.
```csharp
public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddMvc();

        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new Info { Title = "My API", Version = "v1" });

            // Set the comments path for the Swagger JSON and UI.
            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, "bin", xmlFile);
            c.IncludeXmlComments(xmlPath);
        });
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IHostingEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseMvc(routes =>
        {
            routes.MapRoute(
                name: "default",
                template: "{controller=Home}/{action=Index}/{id?}");
        });

        // Enable middleware to serve generated Swagger as a JSON endpoint.
        app.UseSwagger();

        // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
        // specifying the Swagger JSON endpoint.
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
        });
    }
}
````

## Samples
The Git repository contains some samples (in `/samples/` folder) to illustrate usage of this library.
(I use also these projects to do some integration tests).

- `/samples/mvc/`: This sample illustrate how to how a ASP .NET MVC 2 Razor Class
Library (RCL) inside a ASP .NET Web Forms project.

- `/samples/identity/`: This sample illustrate how to use the
[Identity Server](https://github.com/IdentityServer/IdentityServer4)
based on ASP .NET Core MVC 2 Razor Class Library (RCL) and use
an Open ID authentication with an ASP .NET Web Forms application. This
ASP .NET Web Forms application host the ASP .NET Core MVC 2 Razor Class Library
using the
[PosInformatique.AspNetCore.Server.AspNet](https://www.nuget.org/packages/PosInformatique.AspNetCore.Server.AspNet/)
library.
Also, the ASP .NET Web Forms application use OWIN infrastructure to use
the Open ID authentication provider.

## Limitations
You have to becareful to use **ONLY** the components and service of the ASP .NET Core inside
your controllers implementation.
Avoid to access to the components or services of ASP .NET non-core from your controllers
implementations. For example, be sure to access to the ASP .NET Core ``HttpContext``
inside your controllers code and do not use the ASP .NET non-core ``HttpContext``.

![Asp Net Core Limitation](documentation/AspNetCoreLimitation.png)

As is shown in the previous architecture drawing, in your controllers code you should
use only to the ASP .NET Core API even you can access to the ASP .NET non-core infrastructure API.

### ASP .NET Core >= 3.x
Because ASP .NET Core 3.x is based only on the .NET Core 3.0 runtime (and not 
on the .NET Standard 2.0 like before with the ASP .NET Core 2.x), this library
**can not** be used to host ASP .NET Core 3.x Web API on ASP .NET non-core infrastructure.

## Contributions
Do not hesitate to clone my code and submit some changes...
It is a open source project, so everyone is welcome to improve this library...
By the way, I am french... So maybe you will remarks that my english is not really fluent...
So do not hesitate to fix my resources strings or my documentation... Merci !

## Thanks
I want to thank the [DiliTrust](https://www.dilitrust.com/) company to test and gave me their
feedback of this library for their ASP .NET WebForms applications which embedded
API developped on ASP .NET Core Web.
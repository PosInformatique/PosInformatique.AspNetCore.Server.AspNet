# PosInformatique.AspNetCore.Server.AspNet
**PosInformatique.AspNetCore.Server.AspNet** is a library to host ASP .NET Core Web API on ASP .NET
non-core (WebForms and MVC) infrastructure based on the .NET Framework.

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

## Contributions
Do not hesitate to clone my code and submit some changes...
It is a open source project, so everyone is welcome to improve this library...
By the way, I am french... So maybe you will remarks that my english is not really fluent...
So do not hesitate to fix my resources strings or my documentation... Merci !

## Thanks
I want to thank the [DiliTrust](https://www.dilitrust.com/) company to test and gave me their
feedback of this library for their ASP .NET WebForms applications which embedded
API developped on ASP .NET Core Web.
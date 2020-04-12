using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using Microsoft.Owin.Extensions;
using Owin;
using Microsoft.Owin.Security.OpenIdConnect;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Owin.Security.Cookies;

[assembly: OwinStartup(typeof(IdentityServer.AspNetWebForms.OwinStartup))]

namespace IdentityServer.AspNetWebForms
{
    internal class OwinStartup
    {
        private const string Auth = "https://localhost:44377/identity-server/";

        public void Configuration(IAppBuilder app)
        {
            app.UseCookieAuthentication(new CookieAuthenticationOptions()
            {
                AuthenticationType = "Cookies",
                ExpireTimeSpan = TimeSpan.FromMinutes(10),
                SlidingExpiration = true,
            });

            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

            app.UseOpenIdConnectAuthentication(new OpenIdConnectAuthenticationOptions
            {
                AuthenticationType = OpenIdConnectAuthenticationDefaults.AuthenticationType,
                SignInAsAuthenticationType = "Cookies",
                Authority = Auth,
                ClientId = "posinformatique.aspnetcore.server.aspnet.identityserver.samples",
                ClientSecret = "49C1A7E1-0C79-4A89-A3D6-A37998FB86B0",
                RedirectUri = "https://localhost:44377/",
                PostLogoutRedirectUri = "https://localhost:44377/",
                ResponseType = "code id_token",
                Scope = "openid profile email offline_access",
                UseTokenLifetime = false,
                Notifications = new OpenIdConnectAuthenticationNotifications
                {
                    SecurityTokenReceived = context =>
                    {
                        Debug.WriteLine("*** SecurityTokenReceived");
                        return Task.FromResult(0);
                    },
                    AuthorizationCodeReceived = context =>
                    {
                        Debug.WriteLine("*** AuthorizationCodeReceived");
                        return Task.FromResult(0);
                    },
                    MessageReceived = context =>
                    {
                        Debug.WriteLine("*** MessageReceived");
                        return Task.FromResult(0);
                    },
                    AuthenticationFailed = context =>
                    {
                        Debug.WriteLine("*** AuthenticationFailed");
                        return Task.FromResult(0);
                    },
                    SecurityTokenValidated = context =>
                    {
                        Debug.WriteLine("*** SecurityTokenValidated");
                        return Task.FromResult(0);
                    },
                    RedirectToIdentityProvider = context =>
                    {
                        Debug.WriteLine("*** RedirectToIdentityProvider");
                        return Task.FromResult(0);
                    }
                }
            });

            app.UseStageMarker(PipelineStage.Authenticate);
        }
    }
}

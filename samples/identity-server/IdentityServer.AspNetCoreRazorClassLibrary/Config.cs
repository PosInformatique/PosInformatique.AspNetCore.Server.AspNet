// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4.Models;
using System.Collections.Generic;

namespace IdentityServer.AspNetCoreRazorClassLibrary
{
    public static class Config
    {
        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new IdentityResource[]
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Email(),
                new IdentityResources.Profile(),
                new IdentityResources.Address(),
            };
        }

        public static IEnumerable<ApiResource> GetApis()
        {
            return new ApiResource[] { };
        }

        public static IEnumerable<Client> GetClients()
        {
            return new Client[]
            {
                new Client
                {
                    ClientId = "posinformatique.aspnetcore.server.aspnet.identityserver.samples",
                    ClientName = "Web Forms application which host ASP .NET Core Identity Server library",
                    //RequireClientSecret = true,
                    RequireConsent = false,

                    AllowedGrantTypes = GrantTypes.Hybrid,
                    ClientSecrets = { new Secret("49C1A7E1-0C79-4A89-A3D6-A37998FB86B0".Sha256()) },

                    RedirectUris = { "https://localhost:44377/" },
                    FrontChannelLogoutUri = "https://localhost:44377/",
                    PostLogoutRedirectUris = { "https://localhost:44377/" },

                    AllowOfflineAccess = true,
                    AllowedScopes = { "openid", "profile", "roles", "email", "identity.api","test.api" }
                }
            };
        }
    }
}
using IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Test;
using System.Collections.Generic;
using System.Security.Claims;

namespace IdentityServer
{
    public class Config
    {
        public static IEnumerable<Client> Clients =>
            new Client[]
            {
                // Use authentication flow
                //new Client
                //{
                //    ClientId = "movieClient", // client id must be a unique ID
                //    AllowedGrantTypes = GrantTypes.ClientCredentials,
                //    ClientSecrets =
                //    {
                //        new Secret("secret".Sha256())
                //    },
                //    AllowedScopes = { "movieAPI" }
                //},
                // new Client
                //{
                //    ClientId = "movies_mvc_client", // client id must be a unique ID
                //    ClientName = "Movies MVC Web App", // like a description
                //    AllowedGrantTypes = GrantTypes.Code,
                //    AllowRememberConsent = false,
                //    RedirectUris = new List<string>()
                //    {
                //        "https://localhost:5002/signin-oidc"
                //    },
                //    PostLogoutRedirectUris = new List<string>()
                //    {
                //        "https://localhost:5002/signout-callback-oidc"
                //    },
                //    ClientSecrets =
                //    {
                //        new Secret("secret".Sha256())
                //    },
                //    AllowedScopes = new List<string>
                //    {
                //        IdentityServerConstants.StandardScopes.OpenId,
                //        IdentityServerConstants.StandardScopes.Profile
                //    }

                //Use Hybrid flow
                new Client
                {
                    ClientId = "movies_mvc_client", // client id must be a unique ID
                    ClientName = "Movies MVC Web App", // like a description
                    AllowedGrantTypes = GrantTypes.Hybrid,
                    RequirePkce = false,
                    AllowRememberConsent = false,
                    RedirectUris = new List<string>()
                    {
                        "https://localhost:5002/signin-oidc"
                    },
                    PostLogoutRedirectUris = new List<string>()
                    {
                        "https://localhost:5002/signout-callback-oidc"
                    },
                    ClientSecrets =
                    {
                        new Secret("secret".Sha256())
                    },
                    AllowedScopes = new List<string>
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.Address,
                        IdentityServerConstants.StandardScopes.Email,
                        "movieAPI",
                        "roles"
                    }
                }
            };

        public static IEnumerable<ApiScope> ApiScopes =>
            new ApiScope[]
            {
                new ApiScope("movieAPI", "Movie API")
            };

        public static IEnumerable<ApiResource> ApiResources =>
            new ApiResource[]
            {
            };

        public static IEnumerable<IdentityResource> IdentityResources =>
            new IdentityResource[]
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResources.Address(),
                new IdentityResources.Email(),
                new IdentityResource("roles", "Your role(s)", new List<string>{ "role"})
            };

        public static List<TestUser> TestUsers =>
            new List<TestUser>
            {
                new TestUser
                {
                    SubjectId = "1A057447-1BEE-4FA7-A3F3-403E5F02EA28",
                    Username = "lukas",
                    Password = "lukas",
                    Claims = new List<Claim>
                    {
                        new Claim(JwtClaimTypes.GivenName, "Lukas"),
                        new Claim(JwtClaimTypes.FamilyName, "Nguyen")
                    }
                }
            };
    }
}
namespace eBid.Identity.API.Configuration
{
    public class Config
    {
        // ApiResources define the apis in your system
        public static IEnumerable<ApiResource> GetApis()
        {
            return new List<ApiResource> { new("auction", "Auction Service"), new("bidding", "Bidding Service"), };
        }

        // ApiScope is used to protect the API 
        //The effect is the same as that of API resources in IdentityServer 3.x
        public static IEnumerable<ApiScope> GetApiScopes()
        {
            return new List<ApiScope> { new("auction", "Auction Service"), new("bidding", "Bidding Service"), };
        }

        // Identity resources are data like user ID, name, or email address of a user
        // see: http://docs.identityserver.io/en/release/configuration/resources.html
        public static IEnumerable<IdentityResource> GetResources()
        {
            return new List<IdentityResource> { new IdentityResources.OpenId(), new IdentityResources.Profile() };
        }

        // client want to access resources (aka scopes)
        public static IEnumerable<Client> GetClients(IConfiguration configuration)
        {
            return new List<Client>
            {
                new()
                {
                    ClientId = "auctionswaggerui",
                    ClientName = "Auction Swagger UI",
                    AllowedGrantTypes = GrantTypes.Implicit,
                    AllowAccessTokensViaBrowser = true,
                    RedirectUris = { $"{configuration["AuctionApiClient"]}/swagger/oauth2-redirect.html" },
                    PostLogoutRedirectUris = { $"{configuration["AuctionApiClient"]}/swagger/" },
                    AllowedScopes = { "auction" }
                },
                new()
                {
                    ClientId = "webapp",
                    ClientName = "WebApp Client",
                    ClientSecrets = new List<Secret> { new("secret".Sha256()) },
                    AllowedGrantTypes = GrantTypes.CodeAndClientCredentials,
                    RequireConsent = false,
                    AllowOfflineAccess = true,
                    AlwaysIncludeUserClaimsInIdToken = true,
                    IncludeJwtId = false,
                    RequirePkce = false,
                    RedirectUris = new List<string> { $"{configuration["WebAppClient"]}/api/auth/callback/idserver" },
                    PostLogoutRedirectUris =
                        new List<string> { $"{configuration["WebAppClient"]}/signout-callback-oidc" },
                    AllowedScopes = new List<string>
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.OfflineAccess,
                        "auction"
                    },
                    AccessTokenLifetime = 60 * 60 * 2, // 2 hours
                    IdentityTokenLifetime = 60 * 60 * 2 // 2 hours
                }
            };
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Test;

namespace BankOfDotNet.IdentityServer
{
    public class Config
    {
        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new List<IdentityResource>
{
    new IdentityResources.OpenId(),
    new IdentityResources.Profile()
};
        }

        public static List<TestUser> GetUsers()
        {
            return new List<TestUser>
            {
                new TestUser
                {
                    SubjectId="1",
                    Username="Mosh",
                    Password="password"

                },
                new TestUser
                {
                    SubjectId="2",
                    Username="a@b.com",
                    Password="password1"

                }
            };
        }


        public static IEnumerable<ApiResource> GetAllApiResources()
        {
            return new List<ApiResource>
            {
                new ApiResource("bankOfDotNetApi","Customer APi for BankOfDotNet")

            };
        }

        public static IEnumerable<Client> GetClients()
        {
            return new List<Client>
            {
                new Client
                {
                    ClientId="client",
                    AllowedGrantTypes=GrantTypes.ClientCredentials,
                    AllowedScopes={ "bankOfDotNetApi" },
                    ClientSecrets=
                    {
                        new Secret("secret".Sha256())
                    }
                },

                new Client
                {
                    ClientId="ro.client",//re(resource)
                    AllowedGrantTypes=GrantTypes.ResourceOwnerPassword,
                    AllowedScopes={ "bankOfDotNetApi"},
                    ClientSecrets =
                    {
                        new Secret("secret".Sha256())
                    }

                },

                //now we hace to clients

            new Client
            {
                ClientId="mvc",//与mvc中设置的一致
                ClientName="MVC client",
                AllowedGrantTypes=GrantTypes.Implicit,
                RedirectUris={ "http://localhost:50003/sigin-oidc"},//指定用户输入用户密码之后，返回的页面或者URI
                PostLogoutRedirectUris={ "http://localhost:5003/signout-callback"},
                AllowedScopes=new List<string>
                {
                    IdentityServerConstants.StandardScopes.OpenId,
IdentityServerConstants.StandardScopes.Profile
                }

            }
            

       
           
            };
        }


    }
}

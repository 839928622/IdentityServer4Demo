using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4.Models;
using IdentityServer4.Test;

namespace BankOfDotNet.IdentityServer
{
    public class Config
    {

        public static List<TestUser> GetUsers()
        {
            return new List<TestUser>
            {
                new TestUser
                {
                    SubjectId="1",
                    Username="Mash",
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
                    
                }

                //now we hace to clients

            };
        }


    }
}

using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using IdentityModel.Client;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BankOfDotNet.ConsoleClient
{
    class Program
    {
     public  static void Main(string[] args) => MainAsync().GetAwaiter().GetResult();
        private static async Task MainAsync()
        {
            var discoRo = await DiscoveryClient.GetAsync("http://localhost:5000");
            if (discoRo.IsError)
            {
                Console.WriteLine(discoRo.Error);
                return;
            }

            //Grab a bearer token using ResourceOwnerPassword Grant type
            var tokenClientRO = new TokenClient(discoRo.TokenEndpoint, "ro.client", "secret");
            var tokenResponseRO = await tokenClientRO.RequestResourceOwnerPasswordAsync("Mosh","password","bankOfDotNetApi");
            if (tokenResponseRO.IsError)
            {
                Console.WriteLine(tokenResponseRO.Error);
                return;
            }
            Console.WriteLine(tokenResponseRO.Json);
            Console.WriteLine("\n\n");


            //using client credentials flow grant type
            //discover all the endpoints using metadata of identity server
            var disco = await DiscoveryClient.GetAsync("http://localhost:5000");
            if (disco.IsError)
            {
                Console.WriteLine(disco.Error);
                return;
            }
            //Grab a bearer token
            var tokenClient = new TokenClient(disco.TokenEndpoint, "client", "secret");
            var tokenResponse = await tokenClient.RequestClientCredentialsAsync("bankOfDotNetApi");//resourse that what we define on identityserver4
            if (tokenResponse.IsError)
            {
                Console.WriteLine(tokenResponse.Error);
                return;
            }
            Console.WriteLine(tokenResponse.Json);
            Console.WriteLine("\n\n");

            var client = new HttpClient();
            client.SetBearerToken(tokenResponse.AccessToken);
            var customerInfo = new StringContent(
                JsonConvert.SerializeObject(new
                {
                    Id = 10,
                    FirstName = "Mike",
                    LastName = "Nancy"
                }), Encoding.UTF8, "application/json");

            var createCustomerRespinse = await client.PostAsync("http://localhost:54433/api/customers", customerInfo);
            if (!createCustomerRespinse.IsSuccessStatusCode)
            {
                Console.WriteLine(createCustomerRespinse.StatusCode);
            }

            var getCustomerResponse = await client.GetAsync("http://localhost:54433/api/customers");
            if(getCustomerResponse.IsSuccessStatusCode)
            {
                Console.WriteLine(getCustomerResponse.StatusCode);
            }
            else
            {
                var content = await getCustomerResponse.Content.ReadAsStringAsync();
                Console.WriteLine(JArray.Parse(content));
            }
            Console.Read();

        }
    }
}

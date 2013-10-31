using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {
            var token = GetToken();
            CallService(token);
        }

        private static void CallService(string token)
        {
            var client = new HttpClient();
            client.SetBearerToken(Convert.ToBase64String(Encoding.UTF8.GetBytes(token)));


            var response = client.GetAsync("http://localhost:2727/api/identity").Result;
            response.EnsureSuccessStatusCode();

            Console.WriteLine(response.Content.ReadAsStringAsync().Result);

        }

        private static string GetToken()
        {
            var client = new HttpClient();
            client.SetBasicAuthentication("dominick", "abc!123");

            // SAML11
            //var response = client.GetAsync("https://idsrv.local/issue/simple?realm=urn:testrp&tokenType=saml11").Result;
            
            // SAML2
            var response = client.GetAsync("https://idsrv.local/issue/simple?realm=urn:testrp&tokenType=saml2").Result;
            response.EnsureSuccessStatusCode();

            var tokenResponse = response.Content.ReadAsStringAsync().Result;
            var json = JObject.Parse(tokenResponse);
            return json["access_token"].ToString();
        }
    }
}

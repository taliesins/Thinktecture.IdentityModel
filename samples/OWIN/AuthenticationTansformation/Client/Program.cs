using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Net.Http;
using Thinktecture.IdentityModel;

namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {
            var handler = new WebRequestHandler();
            handler.ClientCertificates.Add(
                X509.CurrentUser
                    .My
                    .SubjectDistinguishedName
                    .Find("CN=client")
                    .First());

            //var handler = new HttpClientHandler
            //{
            //    ClientCertificateOptions = ClientCertificateOption.Automatic
            //};

            var client = new HttpClient(handler)
            {
                BaseAddress = new Uri("https://web.local:444/api/")
            };

            client.SetBasicAuthentication("bob", "bob");

            var result = client.GetStringAsync("identity").Result;
            Console.WriteLine(JArray.Parse(result));
        }


    }
}

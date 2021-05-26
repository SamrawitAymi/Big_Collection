using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace APIGateway.Services
{
    public class ClientService
    {
        public async Task<HttpResponseMessage> PostRequestAsync(string apiLocation, object obj, string header)
        {
            var token = ExtractTokenFromAuthorizationHeader(header);

            using (var client = new HttpClient())
            {
                var json = JsonConvert.SerializeObject(obj);
                var request = new HttpRequestMessage(HttpMethod.Post, apiLocation);
                request.Content = new StringContent(json, Encoding.UTF8, "application/json");
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var response = await client.SendAsync(request);
                return response;
            }
        }

        public string ExtractTokenFromAuthorizationHeader(string authHeader)
        {
            return authHeader.ToString().Split(' ')[1];
        }
    }
}

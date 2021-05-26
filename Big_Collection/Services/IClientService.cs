using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Big_Collection.Services
{
    public interface IClientService
    {
        Task<HttpResponseMessage> SendRequestToGatewayAsync(string api, HttpMethod method, object obj = null);
        Task<T> ReadResponseAsync<T>(HttpContent responseContent);
        Task ValidateJwtTokenStatusAsync();
    }
}

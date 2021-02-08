using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace PurchaseOrderTracker.WebUI.Admin.Controllers
{
    public class PurchaseOrderTrackerHttpClient
    {
        private HttpClient _client;

        // TODO httpClient should use its own unique password? Or pass on claim from frontend client?
        public PurchaseOrderTrackerHttpClient(HttpClient client)
        {
            _client = client;
        }

        public async Task<string> GetUsers()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "/api/user");
            var response = await _client.SendAsync(request);

            return await response.Content.ReadAsStringAsync();
        }
    }
}

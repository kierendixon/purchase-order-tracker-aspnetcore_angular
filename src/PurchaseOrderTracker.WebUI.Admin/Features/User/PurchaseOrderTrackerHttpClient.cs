//using System.Net.Http;
//using System.Text;
//using System.Text.Json;
//using System.Threading.Tasks;

//namespace PurchaseOrderTracker.WebUI.Admin.Controllers
//{
//    public class PurchaseOrderTrackerHttpClient
//    {
//        private HttpClient _client;

//        // TODO httpClient should use its own unique password? Or pass on claim from frontend client?
//        public PurchaseOrderTrackerHttpClient(HttpClient client)
//        {
//            _client = client;
//        }

//        public async Task<string> GetUsers()
//        {
//            var response = await _client.GetAsync("/api/user?pageSize=1000");
//            //httpResponse.EnsureSuccessStatusCode();
//            return await response.Content.ReadAsStringAsync();
//        }

//        public async Task<string> CreateUser(string username, string password)
//        {
//            var content = new StringContent(
//                JsonSerializer.Serialize(new { Username= username, Password= password}),
//                Encoding.UTF8,
//                "application/json");

//            var response = await _client.PutAsync("/api/user", content);
//            //httpResponse.EnsureSuccessStatusCode();
//            return await response.Content.ReadAsStringAsync();
//        }
//    }
//}

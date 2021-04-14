using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;

namespace MVCEStorePayment
{
    public static class BinQuery
    {
        private static readonly string binListBaseUrl = "https://lookup.binlist.net/";
        public static async Task<BinQueryResult> CreateQuery(string binNumber)
        {
            using (var httpClient = new HttpClient())
            using (var response = await httpClient.GetAsync($"{binListBaseUrl}{binNumber}"))
            {
                var content = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<BinQueryResult>(content);
            }
        }
    }
}

using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace DashboardApp
{
    public class ApiService
    {
        private readonly HttpClient _httpClient = new HttpClient();

        public async Task<string> GetApiResponse(string url)
        {
            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }
    }
}
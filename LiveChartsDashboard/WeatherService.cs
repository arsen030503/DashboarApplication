using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using LiveChartsDashboard.Models;

namespace LiveChartsDashboard.Services
{
    public class WeatherService
    {
        private readonly HttpClient _httpClient;
        private const string ApiKey = "7acd69a4a5859383ba0042a5c497d472"; // Замените на ваш API-ключ
        private const string BaseUrl = "https://api.openweathermap.org/data/2.5/weather";

        public WeatherService()
        {
            _httpClient = new HttpClient();
        }

        /// <summary>
        /// Получить информацию о погоде по названию города.
        /// </summary>
        /// <param name="city">Название города</param>
        /// <returns>Объект WeatherData с данными о погоде</returns>
        public async Task<WeatherData> GetWeatherAsync(string city)
        {
            var response = await _httpClient.GetAsync($"{BaseUrl}?q={city}&appid={ApiKey}&units=metric");
            response.EnsureSuccessStatusCode(); // Проверка успешного ответа

            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<WeatherData>(json);
        }
    }
}
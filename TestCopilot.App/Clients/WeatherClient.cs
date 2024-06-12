using TestCopilot.Models;

namespace TestCopilot.App.Clients
{
    public interface IWeatherClient
    {
        Task<WeatherForecast[]?> GetWeatherForecastAsync();
    }

    public class WeatherClient : IWeatherClient
    {
        private readonly string _weatherBaseUrl;

        public WeatherClient(IConfiguration configuration)
        {
            _weatherBaseUrl = configuration["TestCopilotApi"] ?? "";
        }

        public async Task<WeatherForecast[]?> GetWeatherForecastAsync()
        {
            using var httpClient = new HttpClient()
            {
                BaseAddress = new Uri(_weatherBaseUrl)
            };
            var forecast = await httpClient.GetFromJsonAsync<WeatherForecast[]>("WeatherForecast");
            return forecast;
        }
    }
}

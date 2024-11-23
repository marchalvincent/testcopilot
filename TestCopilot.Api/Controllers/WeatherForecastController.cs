using Microsoft.AspNetCore.Mvc;
using System.Text;
using TestCopilot.Models;

namespace TestCopilot.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }

        // GPT-4o
        [HttpGet("france-weather", Name = "GetFranceWeather")]
        public IEnumerable<WeatherForecast> GetFranceWeather()
        {
            var cities = new[] { "Paris", "Lyon", "Marseille", "Toulouse", "Nice", "Nantes", "Strasbourg", "Montpellier", "Bordeaux", "Lille" };
            var forecasts = new List<WeatherForecast>();

            foreach (var city in cities)
            {
                forecasts.Add(new WeatherForecast
                {
                    Date = DateOnly.FromDateTime(DateTime.Now),
                    TemperatureC = Random.Shared.Next(-20, 55),
                    Summary = city
                });
                forecasts.Add(new WeatherForecast
                {
                    Date = DateOnly.FromDateTime(DateTime.Now.AddDays(1)),
                    TemperatureC = Random.Shared.Next(-20, 55),
                    Summary = city
                });
            }

            return forecasts;
        }

        // o1-mini
        [HttpGet("franceweather", Name = "FranceWeather")]
        public IEnumerable<WeatherForecast> FranceWeather()
        {
            var cities = new[] { "Paris", "Lyon", "Marseille", "Nice", "Toulouse", "Bordeaux", "Lille", "Nantes", "Strasbourg", "Montpellier" };
            var dates = new[]
            {
                DateOnly.FromDateTime(DateTime.Now),
                DateOnly.FromDateTime(DateTime.Now.AddDays(1))
            };
            return cities.SelectMany(city => dates.Select(date => new WeatherForecast
            {
                Date = date,
                TemperatureC = Random.Shared.Next(-10, 40),
                Summary = city
            }));
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using orders.shared;

namespace orders.webapi.Controllers
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
        private readonly IAppSettingsWrapper _appsettings;
        private readonly IAllSettingsWrapper _allSettings;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, 
            IAppSettingsWrapper appSettings, 
            IAllSettingsWrapper allSettings,
            Microsoft.AspNetCore.Hosting.IHostingEnvironment hostingEnvironment)
        {
            _logger = logger;
            _appsettings = appSettings;
            _allSettings = allSettings;
            HostingEnvironment = hostingEnvironment;
        }

        private Microsoft.AspNetCore.Hosting.IHostingEnvironment HostingEnvironment { get; }

        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get()
        {
            var environment = HostingEnvironment.EnvironmentName;
            var cacheType = _appsettings.CacheType;
            var sessionTimeout = _appsettings.SessionTimeout;
            var dbTimeout = _allSettings.DbSettings.ConnectionString;

            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }
    }
}

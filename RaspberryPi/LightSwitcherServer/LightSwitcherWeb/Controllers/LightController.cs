using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace LightSwitcherWeb.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LightController : ControllerBase
    {
        public LightController(ILightSwitcherGateway lightSwitcherGateway)
        {
            if (lightSwitcherGateway == null)
            {
                throw new ArgumentNullException(nameof(lightSwitcherGateway));
            }

            _lightSwitcherGateway = lightSwitcherGateway;
        }

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
        }
 
        private readonly ILogger<WeatherForecastController> _logger = Log.ForContext<LightController>();
    }
}

using Cryptography.Dto;
using Cryptography.Services;
using Microsoft.AspNetCore.Mvc;

namespace Cryptography.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {

        private readonly IFileKey _fileKey;

        private static readonly string[] Summaries = new[]
        {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };


        public WeatherForecastController(ILogger<WeatherForecastController> logger, IFileKey fileKey)
        {
            _fileKey = fileKey;
        }

        [HttpGet("GetWeatherForecast")]
        public async Task<IActionResult> Get()
        {
            var dados = Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();


            return Ok(dados);
        }

        [HttpGet("GetBadRequest")]
        public async Task<IActionResult> GetBadRequest()
        {
            return NotFound("Ultrapssou o limit do github");
        }

        [HttpGet("GetModel")]
        public async Task<IActionResult> GetModel()
        {
            var dados = new WatherForecastDto()
            {
                TemperatureC = 5
            };


            return Ok(dados);
        }


        [HttpPost("PostTemperature")]
        public async Task<IActionResult> PostTemperature([FromBody] WatherForecastDto watherForecastDto)
        {
            var dados = new WatherForecastDto()
            {
                TemperatureC = watherForecastDto.TemperatureC
            };

            return Ok(dados);
        }

    }
}
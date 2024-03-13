using JWT_Example.Config;
using JWT_Example.Filters;
using JWT_Example.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace JWT_Example.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IApiConfig _configuration;
        private readonly IJWTService _jwtService;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, IOptions<ApiConfig> configuration, IJWTService jwtService)
        {
            _logger = logger;
            _configuration = configuration.Value;
            _jwtService = jwtService;
        }

        /// <summary>
        /// Get JWT token for api users
        /// </summary>
        /// <returns></returns>
        [HttpPost("GetToken")]
        [AuthenticateUser]
        public ActionResult CreateJWTToken()
        {
            try
            {
                string? audience = _configuration.JWTAudience;
                string jwtToken = _jwtService.GenerateJwtToken(audience);
                if (!string.IsNullOrEmpty(jwtToken))
                    return Ok(jwtToken);
                else
                    return Ok("JWT Token could not be created");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetWeatherForecast")]
        [AuthorizeJWT]
        public ActionResult Get()
        {
            var result = Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();

            return Ok(result);
        }
    }
}

using Microsoft.AspNetCore.Mvc;

namespace JWTWebApi.Controllers;

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
        Response.Cookies.Append("JWTTestToken", HelperClass.GenerateJwtToken("your_secret_key!your_secret_key!@", "your_issuer", "your_audience"));
        return Enumerable.Range(1, 5).Select(index => new WeatherForecast
        {
            Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            TemperatureC = Random.Shared.Next(-20, 55),
            Summary = Summaries[Random.Shared.Next(Summaries.Length)]
        })
        .ToArray();
    }

    [HttpPost(Name = "PostTestCookie")]
    public IActionResult PostTestCookie()
    {
        var token = Request.Cookies.Where(k => k.Key == "JWTTestToken").FirstOrDefault();
        var claims = HelperClass.ReadJwtTokenString(token.Value);
        return Ok(claims);
    }
}

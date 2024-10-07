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
    private readonly HelperClass _helperClass;

    public WeatherForecastController(ILogger<WeatherForecastController> logger, HelperClass helperClass)
    {
        _logger = logger;
        _helperClass = helperClass;
    }

    [HttpGet(Name = "GetWeatherForecast")]
    public IEnumerable<WeatherForecast> Get()
    {
        Response.Cookies.Append("JWTTestToken", _helperClass.GenerateJwtToken("your_secret_key!your_secret_key!@", "your_issuer", "your_audience"), new CookieOptions() { HttpOnly = true, Expires = DateTimeOffset.UtcNow.AddMinutes(30) });
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
        var claims = _helperClass.ReadJwtTokenString(token.Value);
        return Ok(claims);
    }
}

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Observability2.API.Controllers;
[Route("api/[controller]")]
[ApiController]
public class StockController(ILogger<StockController> logger) : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        logger.LogInformation("Stock Get Method Calisti");

        var headers= HttpContext.Request.Headers;  // Show Trace Id coming from microservice 1

        using (var activity = ActivitySourceProvider.ActivitySource.StartActivity("File Operation(write)"))
        {
            System.IO.File.WriteAllText("log.txt", "Hello Wordl");
        }
         return Ok(10);
    }
}

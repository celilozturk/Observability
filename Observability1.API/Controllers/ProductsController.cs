using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Observability1.API.Models;
using System.Diagnostics;

namespace Observability1.API.Controllers;
[Route("api/[controller]")]
[ApiController]
public class ProductsController(ILogger<ProductsController> logger,ILoggerFactory loggerFactory,AppDbContext context , IDistributedCache distributedCache) : ControllerBase
{
    private static HttpClient httpClient = new HttpClient();

    [HttpGet]
    public IActionResult Get()
    {
        //var orderLogger= loggerFactory.CreateLogger("Order.API.ProductController");
        //orderLogger.LogInformation("Get Mothodu cagrildi(loggerFactory)");
        //logger.LogInformation("Get Methodu cagrildi!(logger)"); // Type Safe better


        return Ok("Products");
    }
    [HttpPost]
    public IActionResult Post()
    {

        context.Products.Add(new Product() { Name = "kalem 1", Price = 300 });
        context.SaveChanges();


        httpClient.GetAsync("https://www.google.com");

        distributedCache.SetString("userId", "123");

        //var orderCode = 123;
        //var userId = 5;
        //var text=string.Format("Merhaba {0}", "Dunya");
        //logger.LogInformation($"Siparis olustu.(OrderCode={orderCode=123}, UserId={userId})");//Unstructured Format
        //logger.LogInformation("Siparis olustu.(OrderCode={0},UserId={1})",orderCode,userId);//Bu kullanimda ise yaramaz!

        //logger.LogInformation("Siparis olustu.(OrderCode={orderCode},UserId={userId})",orderCode,userId);//Structured Format =>  userId=5 indexler o sebeple kolay arama yapilinca bulunur!(Elastic)

        //logger.LogError("kullanici giris yapamadi!");
        //logger.LogError("kullanici giris yapamadi. Kulllanici sifresinde, two Factor flag-false oldugunda dolayi etc ....");
        return StatusCode(StatusCodes.Status201Created);
    }
}

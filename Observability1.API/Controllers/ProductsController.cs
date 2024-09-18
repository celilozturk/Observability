using MassTransit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Observability1.API.Models;
using Observability1.API.Services;
using Shared.Bus;
using System.Diagnostics;

namespace Observability1.API.Controllers;
[Route("api/[controller]")]
[ApiController]
public class ProductsController(ILogger<ProductsController> logger,ILoggerFactory loggerFactory,AppDbContext context , IDistributedCache distributedCache,IPublishEndpoint publishEndpoint, StockService stockService) : ControllerBase
{
    private static HttpClient httpClient = new HttpClient();

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        await publishEndpoint.Publish(new ProductAddedEvent(1, "kalemler", 20, Activity.Current!.TraceId.ToString()));
        var result = await stockService.GetStock();

        //var orderLogger= loggerFactory.CreateLogger("Order.API.ProductController");
        //orderLogger.LogInformation("Get Mothodu cagrildi(loggerFactory)");
        //logger.LogInformation("Get Methodu cagrildi!(logger)"); // Type Safe better


        return Ok("Products");
    }
    [HttpPost]
    public async Task<IActionResult> Post()
    {
        logger.LogInformation("Products => Post Methodu basladi");
        context.Products.Add(new Product() { Name = "kalem 1", Price = 300 });
        context.SaveChanges();

        //httpClient.GetAsync("https://www.google.com");

        distributedCache.SetString("userId", "123");

        using(var activity = ActivitySourceProvider.ActivitySource.StartActivity("File(app.txt)"))
        {
            activity!.SetTag("userId", "123");
        await System.IO.File.WriteAllTextAsync("app.txt", "Merhaba Dunya");
        }
        //Activity.Current.ParentId
        //MassTransit 8.0 artik gerek yok!
        await publishEndpoint.Publish(new ProductAddedEvent(1, "kalemler", 20,Activity.Current!.TraceId.ToString()));

        var result = await stockService.GetStock();


        //Masstransit 8 versiyonundan once kullaniliyordu
        //using (var activity = ActivitySourceProvider.ActivitySource.StartActivity("queue publish(productaddedevent)"))
        //{
        //    await publishEndpoint.Publish(new ProductAddedEvent(1, "kalemler", 20));
        //}

        //Instrumentation olursa activity kullanmaya gerek yok!
        //using (var activity = ActivitySourceProvider.ActivitySource.StartActivity("SqlServerOpeation"))
        //{
        //    context.Products.Add(new Product() { Name = "kalem 1", Price = 300 });
        //    context.SaveChanges();
        //}
        //using (var activity = ActivitySourceProvider.ActivitySource.StartActivity("HttpClientOperation"))
        //{
        //    activity.SetTag("schema", "https");
        //    httpClient.GetAsync("https://www.google.com");
        //}

        // distributedCache.SetString("userId", "123");

        //var orderCode = 123;
        //var userId = 5;
        //var text=string.Format("Merhaba {0}", "Dunya");
        //logger.LogInformation($"Siparis olustu.(OrderCode={orderCode=123}, UserId={userId})");//Unstructured Format
        //logger.LogInformation("Siparis olustu.(OrderCode={0},UserId={1})",orderCode,userId);//Bu kullanimda ise yaramaz!

        //logger.LogInformation("Siparis olustu.(OrderCode={orderCode},UserId={userId})",orderCode,userId);//Structured Format =>  userId=5 indexler o sebeple kolay arama yapilinca bulunur!(Elastic)

        //logger.LogError("kullanici giris yapamadi!");
        //logger.LogError("kullanici giris yapamadi. Kulllanici sifresinde, two Factor flag-false oldugunda dolayi etc ....");
        //return StatusCode(StatusCodes.Status201Created);
        logger.LogInformation("Products => Post Methodu bitti!");
        logger.LogInformation("Siparis olustu(OrderCode={orderCode})(UserId={userId})", "abc", "123");

        return Ok(result);
    }
}

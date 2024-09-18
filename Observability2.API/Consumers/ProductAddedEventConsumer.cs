using MassTransit;
using Shared.Bus;
using System.Diagnostics;

namespace Observability2.API.Consumers;

public class ProductAddedEventConsumer : IConsumer<ProductAddedEvent>
{
    public Task Consume(ConsumeContext<ProductAddedEvent> context)
    {
        Console.WriteLine($"Gelen Mesaj (ProductId={context.Message.Id})");
        //var activityTraceId = ActivityTraceId.CreateFromString(context.Message.TraceId);
        //var spanId = ActivitySpanId.CreateFromString("");
        //var activityContext = new ActivityContext(activityTraceId, spanId, ActivityTraceFlags.Recorded);
        //using (var activity = ActivitySourceProvider.ActivitySource.StartActivity(ActivityKind.Consumer, activityContext))
        //{

        //}
        return Task.CompletedTask; //await kullanmayinca task donmek lazim!
    }
}

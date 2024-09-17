using System.Diagnostics;

namespace Observability1.API;

public class ActivitySourceProvider
{
   public static ActivitySource ActivitySource = new ActivitySource("Observability.API.ActivitySource");
}

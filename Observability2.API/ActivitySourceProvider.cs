using System.Diagnostics;

namespace Observability2.API;

public class ActivitySourceProvider
{
    public static ActivitySource ActivitySource = new ActivitySource("Observability2.API.ActivitySource");
}

using System.Diagnostics;

namespace Observability1.API;

public class ActivitySourceProvider
{
   public static ActivitySource ActivitySource = new ActivitySource("Observability.API.ActivitySource");
   //public static ActivitySource ActivitySourceFile = new ActivitySource("Observability.API.ActivitySourceFile");
   //public static ActivitySource ActivitySourceSql = new ActivitySource("Observability.API.ActivitySourceSql");

    //Bazi aktiviteleri baska yerlere gondermek icin kullanilir!
   // public ActivitySourceProvider()
   // {
   //     ActivitySource.AddActivityListener(new ActivityListener);
   // }
}

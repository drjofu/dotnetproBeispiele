using Hangfire.Annotations;
using Hangfire.Dashboard;
using System;
using System.Collections.Generic;
using System.Text;

namespace HFCommonLib
{
  public class HFAuthorizationFilter : IDashboardAuthorizationFilter
  {
    public bool Authorize([NotNull] DashboardContext context)
    {
      // Autorisierung hier anpassen
      return true;
    }
  }
}

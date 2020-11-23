using Hangfire.Client;
using Hangfire.Common;
using Hangfire.Server;
using Hangfire.States;
using Hangfire.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HFCommonLib
{
  public class OnPerformedCallbackFilter : JobFilterAttribute, IServerFilter,IClientFilter, IApplyStateFilter, IElectStateFilter
  {
    private readonly object app;

    public OnPerformedCallbackFilter(object app)
    {
      this.app = app;
    }
    public void OnCreated(CreatedContext filterContext)
    {
    }

    public void OnCreating(CreatingContext filterContext)
    {
    }

    public void OnPerformed(PerformedContext filterContext)
    {
      
    }

    public void OnPerforming(PerformingContext filterContext)
    {
      
    }

    public void OnStateApplied(ApplyStateContext context, IWriteOnlyTransaction transaction)
    {
    }

    public void OnStateElection(ElectStateContext context)
    {
    }

    public void OnStateUnapplied(ApplyStateContext context, IWriteOnlyTransaction transaction)
    {
    }
  }
}

using System;
using System.Collections.Concurrent;

namespace client.Common.Controllers
{
    public class Worker
    {
        public Worker(Views.WorldLayer worldLayer)
        {
            WorldLayer = worldLayer;
            Queue = new ConcurrentQueue<@base.model.Action> ();
        }

        public void Schedule(float frameTimesInSecond)
        {
            if (Action != null)
            {
                var actionV = (Views.Action.Action)Action.View;
                if (actionV.Schedule (frameTimesInSecond))
                {
                    // action was successfully executed, let the next be executed
                    Action = null;
                }
            }
            else if (Queue.TryDequeue (out Action))
            {
                var regionC = @base.control.Controller.Instance.RegionManagerController;
                var actionC = (@base.control.action.Action) Action.Control;
                var actionV = CreateActionView(Action);
                var affectedRegions = actionC.GetAffectedRegions (regionC);
                actionC.Possible (regionC);
                actionV.BeforeDo ();
                actionC.Do (regionC);
            }

        }

        static Views.Action.Action CreateActionView(@base.model.Action action)
        {
            switch(action.Type)
            {
            case(@base.model.Action.ActionType.CreateUnit):
                return new Views.Action.CreateUnit (action, WorldLayer);

            case(@base.model.Action.ActionType.CreateHeadquarter):
            case(@base.model.Action.ActionType.MoveUnit):
                throw new NotImplementedException ();
            }

            return new Views.Action.Action(action);
        }

        static public ConcurrentQueue<@base.model.Action> Queue;
        static public @base.model.Action Action = null;
        static public Views.WorldLayer WorldLayer;

    }
}


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

                for (int i = 0; i < 1; ++i)
                {
                    actionC.Possible (regionC);
                }
                actionV.BeforeDo ();
                actionC.Do (regionC);
            }
        }

        Views.Action.Action CreateActionView(@base.model.Action action)
        {
            switch(action.Type)
            {
            case(@base.model.Action.ActionType.CreateUnit):
                return new Views.Action.CreateUnit (action, WorldLayer);

            case(@base.model.Action.ActionType.MoveUnit):
                return new Views.Action.MoveUnit (action, WorldLayer);

            case(@base.model.Action.ActionType.CreateHeadquarter):
                throw new NotImplementedException ();
            }

            return new Views.Action.Action(action);
        }

        public ConcurrentQueue<@base.model.Action> Queue;
        public @base.model.Action Action = null;
        public Views.WorldLayer WorldLayer;

    }
}


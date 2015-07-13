using System;
using System.Collections.Concurrent;

namespace client.Common.Views
{
    public class Worker
    {
        public Worker(Views.WorldLayer worldLayer)
        {
            WorldLayer = worldLayer;
            Queue = new ConcurrentQueue<Core.Models.Action>();
        }

        public void Schedule(float frameTimesInSecond)
        {
            if (Action != null)
            {
                var actionV = (Views.Action.Action)Action.View;
                if (actionV.Schedule(frameTimesInSecond))
                {
                    // action was successfully executed, let the next be executed
                    Action = null;
                }
            }
            else if (Queue.TryDequeue(out Action))
            {
                var regionC = Core.Controllers.Controller.Instance.RegionManagerController;
                var actionC = (Core.Controllers.Actions.Action)Action.Control;
                var actionV = CreateActionView(Action);
                var affectedRegions = actionC.GetAffectedRegions(regionC);
                actionC.Possible(regionC);
                actionV.BeforeDo();
                actionC.Do(regionC);
            }
        }

        Views.Action.Action CreateActionView(Core.Models.Action action)
        {
            switch (action.Type)
            {
                case(Core.Models.Action.ActionType.CreateUnit):
                    return new Views.Action.CreateUnit(action, WorldLayer);

                case(Core.Models.Action.ActionType.MoveUnit):
                    return new Views.Action.MoveUnit(action, WorldLayer);

                case(Core.Models.Action.ActionType.CreateHeadquarter):
                    throw new NotImplementedException();

                case(Core.Models.Action.ActionType.CreateBuilding):
                    return new Views.Action.CreateBuilding(action, WorldLayer);

            }


            return new Views.Action.Action(action);
        }

        public ConcurrentQueue<Core.Models.Action> Queue;
        public Core.Models.Action Action = null;
        public Views.WorldLayer WorldLayer;

    }
}


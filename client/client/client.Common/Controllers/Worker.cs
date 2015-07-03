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


            var param = new ConcurrentDictionary<string, object> ();
            param [@base.control.action.CreateUnit.CREATE_POSITION] = new @base.model.PositionI ((int) (WorldLayer.CenterPosition.X), (int) (WorldLayer.CenterPosition.Y - 1));
            param [@base.control.action.CreateUnit.CREATION_TYPE] = (long)60;

            var action = new @base.model.Action (GameAppDelegate.Account, @base.model.Action.ActionType.CreateUnit, param);

            Queue.Enqueue (action);

            var param2 = new ConcurrentDictionary<string, object> ();
            param2 [@base.control.action.MoveUnit.START_POSITION] = new @base.model.PositionI ((int) (WorldLayer.CenterPosition.X), (int) (WorldLayer.CenterPosition.Y ));
            param2 [@base.control.action.MoveUnit.END_POSITION] = new @base.model.PositionI ((int) (WorldLayer.CenterPosition.X + 8), (int) (WorldLayer.CenterPosition.Y ));
            var action2 = new @base.model.Action (GameAppDelegate.Account, @base.model.Action.ActionType.MoveUnit, param2);

            Queue.Enqueue (action2);

               
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
                    try
                    {
                        actionC.Possible (regionC);
                    }
                    catch
                    {
                    }
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


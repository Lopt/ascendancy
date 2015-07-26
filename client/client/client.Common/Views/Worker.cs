using System;
using System.Collections.Concurrent;

namespace Client.Common.Views
{
    /// <summary>
    /// The Worker do the actions on the view.
    /// </summary>
    public class Worker
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Client.Common.Views.Worker"/> class.
        /// </summary>
        /// <param name="worldLayer">World layer.</param>
        public Worker(Views.WorldLayer worldLayer)
        {
            WorldLayer = worldLayer;
            Queue = new ConcurrentQueue<Core.Models.Action>();
        }

        /// <summary>
        /// Do the actions.
        /// </summary>
        /// <param name="frameTimesInSecond">Frame times in second.</param>
        public void Schedule(float frameTimesInSecond)
        {
            
            if (Action != null)
            {
                var actionV = (Client.Common.Views.Actions.Action)Action.View;
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
                var affectedRegions = actionC.GetAffectedRegions();
                actionC.Possible();
                actionV.BeforeDo();
                actionC.Do();
            }
        }

        /// <summary>
        /// Creates the action.
        /// </summary>
        /// <returns>The action.</returns>
        /// <param name="action">Action.</param>
        Client.Common.Views.Actions.Action CreateActionView(Core.Models.Action action)
        {
            switch (action.Type)
            {
                case(Core.Models.Action.ActionType.CreateUnit):
                    return new Client.Common.Views.Actions.CreateUnit(action, WorldLayer);

                case(Core.Models.Action.ActionType.MoveUnit):
                    return new Client.Common.Views.Actions.MoveUnit(action, WorldLayer);

                case(Core.Models.Action.ActionType.CreateHeadquarter):
                    throw new NotImplementedException();

                case(Core.Models.Action.ActionType.CreateBuilding):
                    return new Client.Common.Views.Actions.CreateBuilding(action, WorldLayer);

            }


            return new Client.Common.Views.Actions.Action(action);
        }

        /// <summary>
        /// The actionqueue.
        /// </summary>
        public ConcurrentQueue<Core.Models.Action> Queue;
        /// <summary>
        /// The action.
        /// </summary>
        public Core.Models.Action Action = null;
        /// <summary>
        /// The world layer.
        /// </summary>
        public Views.WorldLayer WorldLayer;

    }
}


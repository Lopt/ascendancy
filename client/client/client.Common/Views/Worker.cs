namespace Client.Common.Views
{
    using System;
    using System.Collections.Concurrent;

    using Core.Models;

    /// <summary>
    /// The Worker do the actions on the view.
    /// </summary>
    public class Worker
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Client.Common.Views.Worker"/> class.
        /// </summary>
        /// <param name="worldLayerHex">World layer hex.</param>
        public Worker(WorldLayerHex worldLayerHex)
        {
            WorldLayerHex = worldLayerHex;
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
                var actionV = CreateActionView(Action, actionC.GetRegionPosition());
                var affectedRegions = actionC.GetAffectedRegions();
                actionC.Possible();
                actionV.BeforeDo();
                actionC.Do();
            }
        }

        /// <summary>
        /// Creates the action view, depending on the action.
        /// </summary>
        /// <returns>The action view.</returns>
        /// <param name="action">Action without action view.</param>
        private Client.Common.Views.Actions.Action CreateActionView(Core.Models.Action action, RegionPosition regionPosition)
        {
            switch (action.Type)
            {
                case Core.Models.Action.ActionType.CreateUnit:
                    return new Client.Common.Views.Actions.CreateUnit(action, WorldLayerHex.GetRegionViewHex(regionPosition));

                case Core.Models.Action.ActionType.MoveUnit:
                    return new Client.Common.Views.Actions.MoveUnit(action);

                case Core.Models.Action.ActionType.CreateHeadquarter:
                    throw new NotImplementedException();

                case Core.Models.Action.ActionType.CreateBuilding:
                    return new Client.Common.Views.Actions.CreateBuilding(action, WorldLayerHex.GetRegionViewHex(regionPosition));
            }
            return new Client.Common.Views.Actions.Action(action);
        }

        /// <summary>
        /// The action queue.
        /// </summary>
        public ConcurrentQueue<Core.Models.Action> Queue;

        /// <summary>
        /// The action.
        /// </summary>
        public Core.Models.Action Action = null;

        /// <summary>
        /// The world layer hex.
        /// </summary>
        public WorldLayerHex WorldLayerHex;
    }
}
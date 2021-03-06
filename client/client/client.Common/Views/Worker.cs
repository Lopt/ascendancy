﻿namespace Client.Common.Views
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
        /// Gets the instance.
        /// </summary>
        /// <value>The instance.</value>
        public static Worker Instance
        {
            get
            {
                return Singleton.Value;
            }
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
            WorldLayerHex.UglyDraw();
        }

        /// <summary>
        /// Init the specified worldLayer.
        /// </summary>
        /// <param name="worldLayer">World layer.</param>
        public void Init(WorldLayerHex worldLayer)
        {
            WorldLayerHex = worldLayer;
        }

        /// <summary>
        /// Creates the action view.
        /// </summary>
        /// <returns>The action view.</returns>
        /// <param name="action">The action.</param>
        /// <param name="regionPosition">The regionposition.</param>
        private Client.Common.Views.Actions.Action CreateActionView(Core.Models.Action action, RegionPosition regionPosition)
        {
            switch (action.Type)
            {
                case Core.Models.Action.ActionType.CreateUnit:
                    return new Client.Common.Views.Actions.CreateUnit(action);

                case Core.Models.Action.ActionType.MoveUnit:
                    return new Client.Common.Views.Actions.MoveUnit(action, WorldLayerHex);

                case Core.Models.Action.ActionType.CreateTerritoryBuilding:
                    return new Client.Common.Views.Actions.CreateTerritoryBuilding(action, WorldLayerHex);

                case Core.Models.Action.ActionType.CreateBuilding:
                    return new Client.Common.Views.Actions.CreateBuilding(action, WorldLayerHex.GetRegionViewHex(regionPosition));
            }
            return new Client.Common.Views.Actions.Action(action);
        }

        /// <summary>
        /// Prevents a default instance of the <see cref="Client.Common.Views.Worker"/> class from being created.
        /// </summary>
        private Worker()
        {
            Queue = new ConcurrentQueue<Core.Models.Action>();
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

        /// <summary>
        /// The singleton instance.
        /// </summary>
        private static readonly Lazy<Worker> Singleton =
            new Lazy<Worker>(() => new Worker());
    }
}
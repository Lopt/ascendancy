namespace Client.Common.Views.Actions
{
    using System;
    using Core.Models;

    /// <summary>
    /// Create a territory building.
    /// </summary>
    public class CreateTerritoryBuilding : Client.Common.Views.Actions.Action
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Client.Common.Views.Actions.CreateTerritoryBuilding"/> class.
        /// </summary>
        /// <param name="model">The entity model of the building.</param>
        /// <param name="worldLayerHex">World layer.</param>
        public CreateTerritoryBuilding(Core.Models.ModelEntity model, WorldLayerHex worldLayerHex)
            : base(model)
        {
            WorldLayerHex = worldLayerHex;
        }

        /// <summary>
        /// Gets called before ActionControl.Do() gets executed. Should get and store data which will be needed in Schedule.
        /// </summary>
        public override void BeforeDo()
        {
            Helper.Logging.Info("CreateTerritoryBuiding Executed");
        }

        /// <summary>
        /// Schedules the action. Should do anything do animate the action (e.g. draw the entity, animate his moving or start/end animating a fight)
        /// Returns true if the action has ended, otherwise false.
        /// </summary>
        /// <param name="frameTimesInSecond">frames times in seconds.</param>
        /// <returns>true if the schedule of the action is done</returns>
        public override bool Schedule(float frameTimesInSecond)
        {
            var action = (Core.Models.Action)Model;

            var position = (Core.Models.PositionI)action.Parameters[Core.Controllers.Actions.CreateBuilding.CREATE_POSITION];
            var entity = Core.Controllers.Controller.Instance.RegionManagerController.GetRegion(position.RegionPosition).GetEntity(position.CellPosition);
            if (entity != null)
            {
                WorldLayerHex.GetRegionViewHex(position.RegionPosition).SetBuilding(new CocosSharp.CCTileMapCoordinates(position.CellPosition.CellX, position.CellPosition.CellY), entity);
                WorldLayerHex.DrawBorders(entity.Owner);            
            }
            return true;
        }

        /// <summary>
        /// Gets the world layer hex.
        /// </summary>
        /// <value>The world layer hex.</value>
        public WorldLayerHex WorldLayerHex
        {
            get;
            private set;
        }
    }
}
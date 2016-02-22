namespace Client.Common.Views.Actions
{
    using System;

    /// <summary>
    /// Create a building.
    /// </summary>
    public class CreateBuilding : Client.Common.Views.Actions.Action
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Client.Common.Views.Actions.CreateBuilding"/> class.
        /// </summary>
        /// <param name="model">Model of the Building</param>
        /// <param name="regionViewHex">World layer.</param>
        public CreateBuilding(Core.Models.ModelEntity model, RegionViewHex regionViewHex)
            : base(model)
        {
            RegionViewHex = regionViewHex;
        }

        /// <summary>
        /// Gets called before ActionControl.Do() gets executed. Should get and store data which will be needed in Schedule.
        /// </summary>
        public override void BeforeDo()
        {
            Helper.Logging.Info("CreateBuiding Executed");
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
                RegionViewHex.SetBuilding(new CocosSharp.CCTileMapCoordinates(position.CellPosition.CellX, position.CellPosition.CellY), entity);
            }
            return true;
        }

        /// <summary>
        /// Gets the world layer.
        /// </summary>
        /// <value>The world layer.</value>
        public RegionViewHex RegionViewHex
        {
            get;
            private set;
        }
    }
}
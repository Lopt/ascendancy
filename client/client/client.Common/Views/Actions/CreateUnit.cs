namespace Client.Common.Views.Actions
{
    using System;

    /// <summary>
    /// Create a unit.
    /// </summary>
    public class CreateUnit : Client.Common.Views.Actions.Action
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Client.Common.Views.Actions.CreateUnit"/> class.
        /// </summary>
        /// <param name="model">Model of the unit.</param>
        /// <param name="worldLayer">World layer.</param>
        public CreateUnit(Core.Models.ModelEntity model, RegionViewHex regionViewHex)
            : base(model)
        {
            RegionViewHex = regionViewHex;
        }

        /// <summary>
        /// Gets called before ActionControl.Do() gets executed. Should get and store data which will be needed in Schedule.
        /// </summary>
        public override void BeforeDo()
        {
            Helper.Logging.Info("CreateUnit Executed");
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
            var actionC = (Core.Controllers.Actions.CreateUnit)Model.Control;

            var position = actionC.RealCreatePosition; 
            var entity = Core.Controllers.Controller.Instance.RegionManagerController.GetRegion(position.RegionPosition).GetEntity(position.CellPosition);
            RegionViewHex.DrawUnit(entity);
            //WorldLayer.UglyDraw();

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
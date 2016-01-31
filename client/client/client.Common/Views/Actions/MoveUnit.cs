namespace Client.Common.Views.Actions
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    /// <summary>
    /// Move a unit.
    /// </summary>
    public class MoveUnit : Client.Common.Views.Actions.Action
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Client.Common.Views.Actions.MoveUnit"/> class.
        /// </summary>
        /// <param name="model">The action Model.</param>
        /// <param name="worldLayer">World layer.</param>
        public MoveUnit(Core.Models.ModelEntity model)
            : base(model)
        {
        }

        /// <summary>
        /// Gets called before ActionControl.Do() gets executed. Should get and store data which will be needed in Schedule.
        /// </summary>
        public override void BeforeDo()
        {
            Helper.Logging.Info("MoveUnit Executed");

            var action = (Core.Models.Action)Model;
            var actionC = (Core.Controllers.Actions.MoveUnit)Model.Control;
            var startPosition = (Core.Models.PositionI)action.Parameters[Core.Controllers.Actions.MoveUnit.START_POSITION];
            var endPosition = (Core.Models.PositionI)action.Parameters[Core.Controllers.Actions.MoveUnit.END_POSITION];

            m_entity = Core.Controllers.Controller.Instance.RegionManagerController.GetRegion(startPosition.RegionPosition).GetEntity(startPosition.CellPosition);
            m_path = actionC.Path;
            m_currentPosition = startPosition;

            var entityView = (UnitView)m_entity.View;
        }

        /// <summary>
        /// Schedules the action. Should do anything do animate the action (e.g. draw the entity, animate his moving or start/end animating a fight)
        /// Returns true if the action has ended, otherwise false.
        /// </summary>
        /// <param name="frameTimesInSecond">frames times in seconds.</param>
        /// <returns>true if the schedule of the action is done</returns>
        public override bool Schedule(float frameTimesInSecond)
        {
            frameTimesInSecond /= Common.Constants.ClientConstants.MOVE_SPEED_PER_FIELD;

            var action = (Core.Models.Action)Model;

            m_runTime += frameTimesInSecond;

            if (m_runTime < m_path.Count)
            {
                var nextPosition = (Core.Models.PositionI)m_path[(int)m_runTime];
                if (m_currentPosition != nextPosition)
                {
                    var originalPosition = m_entity.Position;
                    m_entity.Position = nextPosition;
                    var currRegion = Core.Models.World.Instance.RegionManager.GetRegion(m_currentPosition.RegionPosition);
                    var nextRegion = Core.Models.World.Instance.RegionManager.GetRegion(nextPosition.RegionPosition);
                    if (currRegion != null && currRegion.View != null)
                    {
                        var regionV1 = (RegionViewHex)currRegion.View;
                        regionV1.DrawUnit(m_entity);
                    }
                    if (nextRegion != null && nextRegion.View != null)
                    {
                        var regionV2 = (RegionViewHex)nextRegion.View;
                        regionV2.DrawUnit(m_entity);
                    }

                    m_currentPosition = nextPosition;
                    m_entity.Position = originalPosition;
                }
            }

            if (m_runTime >= m_path.Count && m_entity.Health <= 0)
            {
//                var mapCoordinatNext = Helper.PositionHelper.PositionToTileMapCoordinates(WorldLayer.CenterPosition, m_entity.Position);
//                WorldLayer.RegionView.SetUnit(mapCoordinatNext, null);
            }
//            WorldLayer.UglyDraw();
            //WorldLayer.UglyDraw();
            return m_runTime >= m_path.Count;
        }

        /// <summary>
        /// The runtime of the action.
        /// </summary>
        private float m_runTime;

        /// <summary>
        /// The entities which should be moved.
        /// </summary>
        private Core.Models.Entity m_entity;

        /// <summary>
        /// The current position of the entity.
        /// </summary>
        private Core.Models.PositionI m_currentPosition;

        /// <summary>
        /// The path which the enemy should go.
        /// </summary>
        private IList m_path;
    }
}
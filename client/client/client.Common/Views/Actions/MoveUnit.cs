using CocosSharp;
using Core.Controllers;

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
        public MoveUnit(Core.Models.ModelEntity model, RegionViewHex regionViewHex)
            : base(model)
        {
            RegionViewHex = regionViewHex;
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


                RegionViewHex.SetUnit(new CocosSharp.CCTileMapCoordinates(m_currentPosition.CellPosition.CellX, m_currentPosition.CellPosition.CellY), null);
                RegionViewHex.UglyDraw();

                var region = Core.Controllers.Controller.Instance.RegionManagerController.GetRegion(nextPosition.RegionPosition);
                RegionViewHex = (RegionViewHex)region.View;
                if (RegionViewHex != null)
                {
                    RegionViewHex.SetUnit(new CocosSharp.CCTileMapCoordinates(nextPosition.CellPosition.CellX, nextPosition.CellPosition.CellY), m_entity);
                }
                RegionViewHex.UglyDraw();
                m_currentPosition = nextPosition;
            }

            if (m_runTime >= m_path.Count && m_entity.Health <= 0)
            {
                var region = Core.Controllers.Controller.Instance.RegionManagerController.GetRegion(m_entity.Position.RegionPosition);
                RegionViewHex = (RegionViewHex)region.View;
                if (RegionViewHex != null)
                {
                    RegionViewHex.SetUnit(new CCTileMapCoordinates(m_entity.Position.CellPosition.CellX, m_entity.Position.CellPosition.CellY), null);
                    RegionViewHex.WorldLayer.BorderView.RemoveBorder(m_entity);
                }

            }

            return m_runTime >= m_path.Count;
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
﻿namespace Client.Common.Views.Actions
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
        public MoveUnit(Core.Models.ModelEntity model, WorldLayerHex worldLayerHex)
            : base(model)
        {
            WorldLayerHex = worldLayerHex;
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
            m_path.Insert(0, startPosition);

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

            if (m_runTime < m_path.Count - 1)
            {

                var currPos = (Core.Models.PositionI)m_path[(int)m_runTime];
                var nextPos = (Core.Models.PositionI)m_path[(int)m_runTime + 1];
                //if (m_currentPosition != nextPosition)
                {
                    var unitV = (UnitView)m_entity.View;
                    var currRegion = Core.Models.World.Instance.RegionManager.GetRegion(currPos.RegionPosition);

                    var currRegionPoint = Helper.PositionHelper.RegionToWorldspace(currPos.RegionPosition);
                    var nextRegionPoint = Helper.PositionHelper.RegionToWorldspace(nextPos.RegionPosition);
                    var currPoint = Helper.PositionHelper.CellToTile(currPos.CellPosition); 
                    var nextPoint = Helper.PositionHelper.CellToTile(nextPos.CellPosition); 


                    var diff = (nextRegionPoint + nextPoint) - (currRegionPoint + currPoint);
                    var point = currPoint + diff * ((float)m_runTime - (float)(int)m_runTime);

                    unitV.DrawPoint = point;
                    unitV.DrawRegion = currPos.RegionPosition;
                        
                    if (currRegion != null && currRegion.View != null)
                    {
                        var regionV = (RegionViewHex)currRegion.View;
                        regionV.DrawUnit(m_entity);
                    }
                }
            }

            if (m_runTime >= m_path.Count && m_entity.Health <= 0)
            {
                var region = Core.Controllers.Controller.Instance.RegionManagerController.GetRegion(m_entity.Position.RegionPosition);
                var regionViewHex = (RegionViewHex)region.View;
                var owner = m_entity.Owner;
                if (regionViewHex != null)
                {
                    regionViewHex.RemoveUnit(m_entity);
                }
                region.RemoveEntity(DateTime.Now, m_entity);
                WorldLayerHex.DrawBorders(owner);
            }

            return m_runTime >= m_path.Count;
        }

        public WorldLayerHex WorldLayerHex
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

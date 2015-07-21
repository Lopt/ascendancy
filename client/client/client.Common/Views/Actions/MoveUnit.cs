using System;
using System.Collections;
using System.Collections.Generic;


namespace Client.Common.Views.Actions
{
    public class MoveUnit : Client.Common.Views.Actions.Action
    {
        public MoveUnit(Core.Models.ModelEntity model, WorldLayer worldLayer)
            : base(model)
        {
            WorldLayer = worldLayer;
        }

        override public void BeforeDo()
        {
            var action = (Core.Models.Action)Model;
            var actionC = (Core.Controllers.Actions.MoveUnit)Model.Control;
            var startPosition = (Core.Models.PositionI)action.Parameters[Core.Controllers.Actions.MoveUnit.START_POSITION];
            var endPosition = (Core.Models.PositionI)action.Parameters[Core.Controllers.Actions.MoveUnit.END_POSITION];

            m_entity = Core.Controllers.Controller.Instance.RegionManagerController.GetRegion(startPosition.RegionPosition).GetEntity(startPosition.CellPosition);
            m_path = actionC.Path;
            m_currentPosition = startPosition;
        }


        override public bool Schedule(float frameTimesInSecond)
        {
            frameTimesInSecond /= Helper.ClientConstants.MOVE_SPEED_PER_FIELD;

            var action = (Core.Models.Action)Model;

            m_runTime += frameTimesInSecond;

            if (m_runTime < m_path.Count)
            {
                var nextPosition = (Core.Models.PositionI)m_path[(int)m_runTime];

                var mapCoordinatCurrent = Helper.PositionHelper.PositionToTileMapCoordinates(WorldLayer.CenterPosition, m_currentPosition);
                WorldLayer.RegionView.SetUnit(mapCoordinatCurrent, null);

                var mapCoordinatNext = Helper.PositionHelper.PositionToTileMapCoordinates(WorldLayer.CenterPosition, nextPosition);
                WorldLayer.RegionView.SetUnit(mapCoordinatNext, m_entity);

                m_currentPosition = nextPosition;
            }
            WorldLayer.UglyDraw();
            return m_runTime >= m_path.Count;
        }

        public WorldLayer WorldLayer
        {
            private set;
            get;
        }


        float m_runTime;
        Core.Models.Entity m_entity;
        Core.Models.PositionI m_currentPosition;
        IList m_path;

    }
}


using System;
using System.Collections;
using System.Collections.Generic;


namespace client.Common.Views.Action
{
    public class MoveUnit : @client.Common.Views.Action.Action
    {
        public MoveUnit(@base.model.ModelEntity model, WorldLayer worldLayer)
            : base(model)
        {
            WorldLayer = worldLayer;
        }

        override public void BeforeDo()
        {
            var action = (@base.model.Action)Model;
            var actionC = (@base.control.action.MoveUnit)Model.Control;
            var startPosition = (@base.model.PositionI)action.Parameters [@base.control.action.MoveUnit.START_POSITION];
            var endPosition = (@base.model.PositionI)action.Parameters [@base.control.action.MoveUnit.END_POSITION];

            m_entity = @base.control.Controller.Instance.RegionManagerController.GetRegion (startPosition.RegionPosition).GetEntity (startPosition.CellPosition);
            m_path = new List<@base.model.PositionI> ();//actionC.Path;
            m_currentPosition = startPosition;
            for (int i = 0; i < 60; ++i)
            {
                m_path.Add(new @base.model.PositionI(startPosition.X, startPosition.Y + i));
            }
            for (int i = 1; i < 60; ++i)
            {
                m_path.Add(new @base.model.PositionI(startPosition.X - i, startPosition.Y + 60));
            }
        }


        override public bool Schedule(float frameTimesInSecond)
        {
            var action = (@base.model.Action)Model;
            var actionC = (@base.control.action.MoveUnit)Model.Control;

            m_runTime += frameTimesInSecond;

            if (m_runTime < m_path.Count)
            {
                var nextPosition = (@base.model.PositionI)m_path [(int)m_runTime];

                var mapCoordinatCurrent = WorldLayer.PositionToTileMapCoordinates (m_currentPosition);
                WorldLayer.RegionView.SetUnit (mapCoordinatCurrent, null);

                var mapCoordinatNext = WorldLayer.PositionToTileMapCoordinates (nextPosition);
                WorldLayer.RegionView.SetUnit (mapCoordinatNext, m_entity);

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
        @base.model.Entity m_entity;
        @base.model.PositionI m_currentPosition;
        IList m_path;

    }
}


using System;

namespace client.Common.Views.Action
{
    public class CreateUnit : @client.Common.Views.Action.Action
    {
        public CreateUnit(@base.model.ModelEntity model, WorldLayer worldLayer)
            : base(model)
        {
            WorldLayer = worldLayer;
        }

        override public void BeforeDo()
        {
        }


        override public bool Schedule(float frameTimesInSecond)
        {
            m_runTime += frameTimesInSecond;

            var action = (@base.model.Action)Model;
            var actionC = (@base.control.action.CreateUnit)Model.Control;

            var position = (@base.model.PositionI)action.Parameters [@base.control.action.CreateUnit.CREATE_POSITION];
            //var mapCoordinat = WorldLayer.RegionView.GetCurrentTileInMap(new @base.model.Position(position.X, position.Y));
            var mapCoordinat = WorldLayer.PositionToTileMapCoordinates(position);
            var entity = @base.control.Controller.Instance.RegionManagerController.GetRegion (position.RegionPosition).GetEntity (position.CellPosition);
            WorldLayer.RegionView.SetUnit (mapCoordinat, entity);//positionI.Get, CCTileMapCoordinates mapCoordinat, position.RegionPosition);
            WorldLayer.UglyDraw();


            return m_runTime >= Helper.ClientConstants.CREATE_UNIT_SPEED;
        }

        public WorldLayer WorldLayer
        {
            private set;
            get;
        }

        float m_runTime;

    }
}


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
            var action = (@base.model.Action)Model;
            var position = (@base.model.PositionI)action.Parameters [@base.control.action.CreateUnit.CREATE_POSITION];
            Entity = @base.control.Controller.Instance.RegionManagerController.GetRegion (position.RegionPosition).GetEntity (position.CellPosition);
        }


        override public bool Schedule(float frameTimesInSecond)
        {
            var action = (@base.model.Action)Model;
            var actionC = (@base.control.action.CreateUnit)Model.Control;

            var mapCoordinat = WorldLayer.PositionToTileMapCoordinates (Entity.Position);
            WorldLayer.RegionView.SetUnit (mapCoordinat, null);//positionI.Get, CCTileMapCoordinates mapCoordinat, position.RegionPosition);

            return true;
        }

        public @base.model.Entity Entity
        {
            private set;
            get;
        }

        public WorldLayer WorldLayer
        {
            private set;
            get;
        }
    }
}


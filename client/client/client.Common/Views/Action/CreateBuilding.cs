using System;

namespace client.Common.Views.Action
{
    public class CreateBuilding : @client.Common.Views.Action.Action
    {
        public CreateBuilding(@base.model.ModelEntity model, WorldLayer worldLayer)
            : base(model)
        {
            WorldLayer = worldLayer;
        }

        override public void BeforeDo()
        {
        }


        override public bool Schedule(float frameTimesInSecond)
        {
            var action = (@base.model.Action)Model;
            var actionC = (@base.control.action.CreatBuilding)Model.Control;

            var position = (@base.model.PositionI)action.Parameters[@base.control.action.CreatBuilding.CREATE_POSITION];
            //var mapCoordinat = WorldLayer.RegionView.GetCurrentTileInMap(new @base.model.Position(position.X, position));
            var mapCoordinat = WorldLayer.PositionToTileMapCoordinates(position);
            var entity = @base.control.Controller.Instance.RegionManagerController.GetRegion(position.RegionPosition).GetEntity(position.CellPosition);
            WorldLayer.RegionView.SetBuilding(mapCoordinat, entity);//positionI.Get, CCTileMapCoordinates mapCoordinat, position.RegionPosition);           
            WorldLayer.UglyDraw();
            return true;
        }

        public WorldLayer WorldLayer
        {
            private set;
            get;
        }
    }
}


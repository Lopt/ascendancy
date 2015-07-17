using System;

namespace client.Common.Views.Actions
{
    public class CreateBuilding : client.Common.Views.Actions.Action
    {
        public CreateBuilding(Core.Models.ModelEntity model, WorldLayer worldLayer)
            : base(model)
        {
            WorldLayer = worldLayer;
        }

        override public void BeforeDo()
        {
        }


        override public bool Schedule(float frameTimesInSecond)
        {
            var action = (Core.Models.Action)Model;

            var position = (Core.Models.PositionI)action.Parameters[Core.Controllers.Actions.CreatBuilding.CREATE_POSITION];
            //var mapCoordinat = WorldLayer.RegionView.GetCurrentTileInMap(new @base.model.Position(position.X, position));
            var mapCoordinat = Helper.PositionHelper.PositionToTileMapCoordinates(WorldLayer.CenterPosition, position);
            var entity = Core.Controllers.Controller.Instance.RegionManagerController.GetRegion(position.RegionPosition).GetEntity(position.CellPosition);
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


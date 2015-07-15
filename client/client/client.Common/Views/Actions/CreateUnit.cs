using System;

namespace client.Common.Views.Actions
{
    public class CreateUnit : client.Common.Views.Actions.Action
    {
        public CreateUnit(Core.Models.ModelEntity model, WorldLayer worldLayer)
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
            var actionC = (Core.Controllers.Actions.CreateUnit)Model.Control;

            var position = actionC.RealCreatePosition;//(@base.model.PositionI)action.Parameters [@base.control.action.CreateUnit.CREATE_POSITION];
            //var mapCoordinat = WorldLayer.RegionView.GetCurrentTileInMap(new @base.model.Position(position.X, position.Y));
            var mapCoordinat = Helper.PositionHelper.PositionToTileMapCoordinates(WorldLayer.CenterPosition, position);
            var entity = Core.Controllers.Controller.Instance.RegionManagerController.GetRegion(position.RegionPosition).GetEntity(position.CellPosition);
            WorldLayer.RegionView.SetUnit(mapCoordinat, entity);//positionI.Get, CCTileMapCoordinates mapCoordinat, position.RegionPosition);
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


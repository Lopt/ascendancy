using System;
using System.Collections.Concurrent;
using System.Collections.ObjectModel;

using @base.control;
using @base.model.definitions;
using @base.model;

namespace @base.control.action
{ 
    public class MoveEntity : Action
    {
        public MoveEntity(model.ModelEntity model)
            : base(model)
        {
        }

        public const string CREATE_POSITION = "EntityPosition";
        public const string CREATE_UNIT_TYPE = "NewPosition";

        public override ConcurrentBag<model.Region> GetAffectedRegions(RegionManagerController regionManagerC)
        {
            ConcurrentBag<model.Region> Bag = new ConcurrentBag<model.Region>();

            var action = (model.Action)Model;
            var startPosition = new model.PositionI((Newtonsoft.Json.Linq.JContainer)action.Parameters[CREATE_POSITION]);
            
            Bag.Add( regionManagerC.GetRegion(startPosition.RegionPosition) );
            

            //            var newPosition = new model.CombinedPosition((Newtonsoft.Json.Linq.JContainer) action.Parameters[NEW_POSITION]);
            return Bag;
        }

        public ObservableCollection<PositionI> FindWay(PositionI start, PositionI end, UnitDefinition type, RegionManagerController regionManagerC)
        {
            
            var xDiff = start.X - end.X;
            var yDiff = start.Y - end.Y;

            for (int xAdd = 1; xAdd < xDiff; ++xAdd)
            {
                var position = (new Position(start.X + xAdd, start.Y + yDiff));
            }

            return null; 
        }

        /// <summary>
        /// Returns if the action is even possible.
        /// </summary>
        //public override bool Possible (RegionManagerController regionManagerC)
        //{   
        //    var action = (model.Action)Model;
                            
        //        if (Parameters.ContainsKey(CREATE_POSITION))
        //        {
        //            var positionC = (CombinedPosition) Parameters[CREATE_POSITION];
        //            var region = regionManagerC.GetRegion(positionC.RegionPosition);
        //            if (region.Exist && this.Regions.Length == 1 && this.Regions[0] == region &&
        //                region.GetEntity(positionC.CellPosition) != null)
        //            {
        //                return true;
        //            }
        //        }
        //        return true;
        //}

        ///// <summary>
        ///// Apply action-related changes to the world.
        ///// Returns false if something went terrible wrong
        ///// </summary>
        //public override ConcurrentBag<model.Region> Do (RegionManagerController regionManagerC)
        //{   
        //    var action = (model.Action)Model;

        //    var regionManagerC = Controller.Instance.RegionManagerController;
        //    var combinedPos = new model.CombinedPosition((Newtonsoft.Json.Linq.JContainer) action.Parameters[CREATE_POSITION]);
        //    var region = regionManagerC.GetRegion(combinedPos.RegionPosition);

        //    var entity = new @base.model.Entity(Guid.NewGuid(), 
        //        new UnitDefinition(Guid.NewGuid(),
        //            model.definitions.UnitDefinition.DefinitionType.building,
        //            UnitDefinition.UnitDefinitionType.Hero,
        //            new Action[0], 
        //            0, 0, 0, 0),
        //        combinedPos);

        //    entity.Position = combinedPos;
        //    region.AddEntity(action.ActionTime, entity);

        //    return true;
        //}

        /// <summary>
        /// In case of errors, revert the world data to a valid state.
        /// </summary>        public bool Catch()
        public override bool Catch()
        {
            throw new NotImplementedException();
        }
    }
}


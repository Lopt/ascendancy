using System;
using System.Collections.Concurrent;
using System.Collections.ObjectModel;

using @base.control;
using @base.model.definitions;

namespace @base.control.action
{ /*
    public class MoveEntity : Action
    {
        public MoveEntity(model.ModelEntity model)
            : base(model)
        {
        }



        public const string CREATE_POSITION = "EntityPosition";
        public const string CREATE_UNIT_TYPE = "NewPosition";

        public override model.Region[] GetAffectedRegions()
        {
            var action = (model.Action)Model;
            var startPosition = new model.Position((Newtonsoft.Json.Linq.JContainer) action.Parameters[CREATE_POSITION]);
            //            var newPosition = new model.CombinedPosition((Newtonsoft.Json.Linq.JContainer) action.Parameters[NEW_POSITION]);
            return Controller.Instance.RegionManagerController.GetRegion(startPosition.RegionPosition); 
        }

        public ObservableCollection<model.CombinedPosition> FindWay(model.Position start, model.Position end)
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
        public override bool Possible()
        {   
            var action = (model.Action)Model;
            
            var regionManagerC = Controller.Instance.RegionManagerController;

                
                if (Parameters.ContainsKey(CREATE_POSITION))
                {
                    var positionC = (CombinedPosition) Parameters[CREATE_POSITION];
                    var region = regionManagerC.GetRegion(positionC.RegionPosition);
                    if (region.Exist && this.Regions.Length == 1 && this.Regions[0] == region &&
                        region.GetEntity(positionC.CellPosition) != null)
                    {
                        return true;
                    }
                }
                return true;
        }

        /// <summary>
        /// Apply action-related changes to the world.
        /// Returns false if something went terrible wrong
        /// </summary>
        public override model.Region[] Do()
        {   
            var action = (model.Action)Model;

            var regionManagerC = Controller.Instance.RegionManagerController;
            var combinedPos = new model.CombinedPosition((Newtonsoft.Json.Linq.JContainer) action.Parameters[CREATE_POSITION]);
            var region = regionManagerC.GetRegion(combinedPos.RegionPosition);

            var entity = new @base.model.Entity(Guid.NewGuid(), 
                new UnitDefinition(Guid.NewGuid(),
                    model.definitions.UnitDefinition.DefinitionType.building,
                    UnitDefinition.UnitDefinitionType.Hero,
                    new Action[0], 
                    0, 0, 0, 0),
                combinedPos);

            entity.Position = combinedPos;
            region.AddEntity(action.ActionTime, entity);

            return true;
        }

        /// <summary>
        /// In case of errors, revert the world data to a valid state.
        /// </summary>        public bool Catch()
        public override bool Catch()
        {
            throw new NotImplementedException();
        }
    }*/
}


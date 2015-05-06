using System;
using System.Collections.Concurrent;

using @base.control;
using @base.model.definitions;

namespace @base.control.action
{
    public class CreateHeadquarter : Action
    {
        public CreateHeadquarter(model.ModelEntity model)
            : base(model)
        {
        }


        public const string CREATE_POSITION = "CreatePosition";
        /// <summary>
        /// Initializes a new instance of the <see cref="base.control.action.Action"/> class.
        /// </summary>
        /// <param name="actionType">Action type.</param>
        /// <param name="regions">Affected Regions of this action.</param>
        /// <param name="parameters">Parameters.</param>
        public override model.Region GetMainRegion()
        {
            var action = (model.Action)Model;
            var combinedPos = new model.CombinedPosition((Newtonsoft.Json.Linq.JContainer) action.Parameters[CREATE_POSITION]);
            return Controller.Instance.RegionManagerController.GetRegion(combinedPos.RegionPosition); 
        }

        /// <summary>
        /// Returns if the action is even possible.
        /// </summary>
        public override bool Possible()
        {   
            var action = (model.Action)Model;
            
            var regionManagerC = Controller.Instance.RegionManagerController;
            if (action.Account.Headquarters.Count == 0)
            {
                /*
                if (Parameters.ContainsKey(CREATE_POSITION))
                {
                    var positionC = (CombinedPosition) Parameters[CREATE_POSITION];
                    var region = regionManagerC.GetRegion(positionC.RegionPosition);
                    if (region.Exist && this.Regions.Length == 1 && this.Regions[0] == region &&
                        region.GetEntity(positionC.CellPosition) != null)
                    {
                        return true;
                    }
                }*/
                return true;
            }
            return false;
        }

        /// <summary>
        /// Apply action-related changes to the world.
        /// Returns false if something went terrible wrong
        /// </summary>
        public override bool Do()
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
    }
}


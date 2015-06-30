using System;
using System.Collections.Concurrent;

using @base.control;
using @base.model.definitions;
using @base.model;

namespace @base.control.action
{
    public class CreateHeadquarter : Action
    {
        public const string CREATE_POSITION = "CreatePosition";

        public CreateHeadquarter(model.ModelEntity model)
            : base(model)
        {
            var action = (model.Action)Model;               
            var param = action.Parameters;

            if (param[CREATE_POSITION].GetType() != typeof(PositionI))
            {
                param[CREATE_POSITION] = new model.PositionI((Newtonsoft.Json.Linq.JContainer) param[CREATE_POSITION]);
            }
        }



        /// <summary>
        /// Initializes a new instance of the <see cref="base.control.action.Action"/> class.
        /// </summary>
        /// <param name="actionType">Action type.</param>
        /// <param name="regions">Affected Regions of this action.</param>
        /// <param name="parameters">Parameters.</param>
        override public ConcurrentBag<model.Region> GetAffectedRegions(RegionManagerController regionManagerC)
        {
            var action = (model.Action)Model;
            var positionI = (PositionI) action.Parameters[CREATE_POSITION];

            return new ConcurrentBag<model.Region>() { regionManagerC.GetRegion(positionI.RegionPosition) }; 
        }

        /// <summary>
        /// Returns if the action is even possible.
        /// </summary>
        public override bool Possible(RegionManagerController regionManagerC)
        {   
            var action = (model.Action)Model;  
                        
            if (action.Account.Headquarters.Count == 0)
            {
                var positionI = (PositionI) action.Parameters[CREATE_POSITION];
                var td = (TerrainDefinition) regionManagerC.GetRegion(positionI.RegionPosition).GetTerrain(positionI.CellPosition);              
                // entity and terrain check 

                return td.Buildable;
            }
            return false;
        }

        /// <summary>
        /// Apply action-related changes to the world.
        /// Returns false if something went terrible wrong
        /// </summary>
        public override ConcurrentBag<model.Region> Do(RegionManagerController regionManagerC)
        {   
            string[] actionParameter = {"CreateUnits"};
            
            var action = (model.Action)Model;
            var positionI = (PositionI) action.Parameters[CREATE_POSITION];
            var region = regionManagerC.GetRegion(positionI.RegionPosition);

            // create the new entity and link to the correct account
            var entity = new @base.model.Entity(action.Account.ID, 
                new UnitDefinition(UnitDefinition.UnitDefinitionType.Headquarter,
                    actionParameter, 
                    0, 0, 0, 0),
                positionI);

            entity.Position = positionI;
            region.AddEntity(action.ActionTime, entity);
            action.Account.Headquarters.AddLast(entity.Position);

            return new ConcurrentBag<model.Region>() { region };
        }

        /// <summary>
        /// In case of errors, revert the world data to a valid state.
        /// </summary>        public bool Catch()
        public override bool Catch(RegionManagerController regionManagerC)
        {
            throw new NotImplementedException();
        }

        override public @base.model.RegionPosition GetRegionPosition()
        {
            var action = (model.Action)Model;
            var positionI = (PositionI) action.Parameters[CREATE_POSITION];
            return positionI.RegionPosition;
        }

    }
}


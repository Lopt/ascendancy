using System;
using System.Collections.Concurrent;

using @base.control;
using @base.model.definitions;
using @base.model;

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
        override public ConcurrentBag<model.Region> GetAffectedRegions(RegionManagerController regionManagerC)
        {
            var action = (model.Action)Model;               
            var positionI = new model.PositionI((Newtonsoft.Json.Linq.JContainer) action.Parameters[CREATE_POSITION]);

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
                var positionI = new model.PositionI((Newtonsoft.Json.Linq.JContainer) action.Parameters[CREATE_POSITION]);
                var td = regionManagerC.GetRegion(positionI.RegionPosition).GetTerrain(positionI.CellPosition).TerrainType;              
                // entity and terrain check 

                if (td != TerrainDefinition.TerrainDefinitionType.Forbidden && td != TerrainDefinition.TerrainDefinitionType.Water )
                {
                    return true;
                }
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
            var positionI = new model.PositionI((Newtonsoft.Json.Linq.JContainer) action.Parameters[CREATE_POSITION]);
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
    }
}


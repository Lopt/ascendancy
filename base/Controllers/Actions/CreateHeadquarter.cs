namespace Core.Controllers.Actions
{
    using System;
    using System.Collections.Concurrent;

    using Core.Controllers.Actions;
    using Core.Models;
    using Core.Models.Definitions;

    /// <summary>
    /// The action which creates the headquarter.
    /// </summary>
    public class CreateHeadquarter : Action
    {
        /// <summary>
        /// Position where the building should be build
        /// </summary>
        public const string CREATE_POSITION = "CreatePosition";

        /// <summary>
        /// Initializes a new instance of the <see cref="Core.Controllers.Actions.CreateHeadquarter"/> class.
        /// </summary>
        /// <param name="model">action model.</param>
        public CreateHeadquarter(Core.Models.ModelEntity model)
            : base(model)
        {
            var action = (Core.Models.Action)Model;               
            var param = action.Parameters;

            if (param[CREATE_POSITION].GetType() != typeof(PositionI))
            {
                param[CREATE_POSITION] = new Core.Models.PositionI((Newtonsoft.Json.Linq.JContainer)param[CREATE_POSITION]);
            }
        }

        /// <summary>
        /// Returns a bag of all regions which could be affected by this action.
        /// </summary>
        /// <returns>The affected regions.</returns>
        public override ConcurrentBag<Core.Models.Region> GetAffectedRegions()
        {
            var regionManagerC = Controller.Instance.RegionManagerController;

            var action = (Core.Models.Action)Model;
            var positionI = (PositionI)action.Parameters[CREATE_POSITION];

            return new ConcurrentBag<Core.Models.Region>() { regionManagerC.GetRegion(positionI.RegionPosition) }; 
        }

        /// <summary>
        /// Returns if the action is even possible.
        /// </summary>
        /// <returns> True if the Headquarter is buildable at the current position, otherwise false.</returns>
        public override bool Possible()
        {   
            var regionManagerC = Controller.Instance.RegionManagerController;
            var action = (Core.Models.Action)Model;  
            var positionI = (PositionI)action.Parameters[CREATE_POSITION];
                        
            if (action.Account.Headquarters.Count == 0 && regionManagerC.GetRegion(positionI.RegionPosition).GetEntity(positionI.CellPosition) == null)
            {                
                var td = (TerrainDefinition)regionManagerC.GetRegion(positionI.RegionPosition).GetTerrain(positionI.CellPosition);       
       
                // entity and terrain check 
                return td.Buildable;
            }
            return false;
        }

        /// <summary>
        /// Apply action-related changes to the world.
        /// </summary>
        /// <returns> Returns <see cref="System.Collections.Concurrent.ConcurrentBag"/> class with the affected region./></returns>
        public override ConcurrentBag<Core.Models.Region> Do()
        {   
            var regionManagerC = Controller.Instance.RegionManagerController;

            string[] actionParameter = { "CreateUnits" };
            
            var action = (Core.Models.Action)Model;
            var positionI = (PositionI)action.Parameters[CREATE_POSITION];
            var region = regionManagerC.GetRegion(positionI.RegionPosition);

            // create the new entity and link to the correct account
            var entity = new Core.Models.Entity(
                IdGenerator.GetId(),
                new UnitDefinition(
                    EntityType.Headquarter,
                    actionParameter, 
                    0,
                    0,
                    0,
                    0,
                    0),
                action.Account,
                positionI,
                100);

            entity.Position = positionI;
            region.AddEntity(action.ActionTime, entity);
            action.Account.Headquarters.AddLast(entity.Position);

            return new ConcurrentBag<Core.Models.Region>() { region };
        }

        /// <summary>
        /// If an error occurred in do, this function will be called to set the data to the last known valid state.
        /// </summary>
        /// <returns>true if rewind was successfully</returns>
        public override bool Catch()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Override <see cref="base.model.RegionPosition"/> class and return the positionI of the region.
        /// </summary>
        /// <returns>The region position.</returns>
        public override Core.Models.RegionPosition GetRegionPosition()
        {
            var action = (Core.Models.Action)Model;
            var positionI = (PositionI)action.Parameters[CREATE_POSITION];
            return positionI.RegionPosition;
        }
    }
}
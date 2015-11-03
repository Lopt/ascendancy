namespace Core.Controllers.Actions
{
    using System;
    using System.Collections.Concurrent;

    using Core.Controllers.Actions;
    using Core.Models;
    using Core.Models.Definitions;

    /// <summary>
    /// The action which creates an building.
    /// </summary>
    public class CreateBuilding : Action
    {
        /// <summary>
        /// Position where the building should be build
        /// </summary>
        public const string CREATE_POSITION = "CreatePosition";

        /// <summary>
        /// Type which building should be build
        /// </summary>
        public const string CREATION_TYPE = "CreateBuilding";

        /// <summary>
        /// Initializes a new instance of the <see cref="Core.Controllers.Actions.CreateBuilding"/> class.
        /// </summary>
        /// <param name="model">action model.</param>
        public CreateBuilding(Core.Models.ModelEntity model)
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
        /// Identify the affected region by this action.
        /// </summary>
        /// <returns> Returns <see cref="System.Collections.Concurrent.ConcurrentBag"/> with the affected region. </returns>
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
        /// <returns> True if the Building is buildable at the current position, otherwise false.</returns>
        public override bool Possible()
        {
            var regionManagerC = Controller.Instance.RegionManagerController;
            var action = (Core.Models.Action)Model;
            var positionI = (PositionI)action.Parameters[CREATE_POSITION];

            if (regionManagerC.GetRegion(positionI.RegionPosition).GetEntity(positionI.CellPosition) == null)
            {
                // entity and terrain check
                var td = (TerrainDefinition)regionManagerC.GetRegion(positionI.RegionPosition).GetTerrain(positionI.CellPosition);
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
            var action = (Core.Models.Action)Model;
            var positionI = (PositionI)action.Parameters[CREATE_POSITION];
            var type = (long)action.Parameters[CREATION_TYPE];

            var region = regionManagerC.GetRegion(positionI.RegionPosition);
            var entityDef = Controller.Instance.DefinitionManagerController.DefinitionManager.GetDefinition((EntityType)type);
            var entityHealth = ((UnitDefinition)entityDef).Health; 
            var entityMoves = ((UnitDefinition)entityDef).Moves;

            // create the new entity and link to the correct account
            var entity = new Core.Models.Entity(
                IdGenerator.GetId(),               
                entityDef,  
                action.Account,
                positionI,
                entityHealth,
                entityMoves);

            entity.Position = positionI;
            region.AddEntity(action.ActionTime, entity);

            if (action.Account != null)
            {
                action.Account.Buildings.AddLast(entity.Position);
            }
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
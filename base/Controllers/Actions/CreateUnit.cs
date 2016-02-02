namespace Core.Controllers.Actions
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using Core.Controllers.Actions;
    using Core.Models;
    using Core.Models.Definitions;

    /// <summary>
    /// Create unit.
    /// </summary>
    public class CreateUnit : Action
    {
        /// <summary>
        /// Position of the parent-entity (e.g. position of barracks, if it spawns an unit)
        /// </summary>
        public const string CREATE_POSITION = "CreatePosition";

        /// <summary>
        /// Unit Type which should be created
        /// </summary>
        public const string CREATION_TYPE = "CreateUnit";

        /// <summary>
        /// Initializes a new instance of the <see cref="Core.Controllers.Actions.CreateUnit"/> class.
        /// </summary>
        /// <param name="model">action model.</param>
        public CreateUnit(Core.Models.ModelEntity model)
            : base(model)
        {
            var action = (Core.Models.Action)Model;               
            var param = action.Parameters;

            if (param[CREATE_POSITION].GetType() != typeof(PositionI))
            {
                param[CREATE_POSITION] = new PositionI((Newtonsoft.Json.Linq.JContainer)param[CREATE_POSITION]);
            }
        }
            
        /// <summary>
        /// Returns a bag of all regions which could be affected by this action.
        /// </summary>
        /// <returns>The affected regions.</returns>
        public override ConcurrentBag<Core.Models.Region> GetAffectedRegions()
        {
            var regionManagerC = Controller.Instance.RegionManagerController;

            ConcurrentBag<Core.Models.Region> bag = new ConcurrentBag<Core.Models.Region>();
            var action = (Core.Models.Action)Model;
            var positionI = (PositionI)action.Parameters[CREATE_POSITION];
            var region = regionManagerC.GetRegion(positionI.RegionPosition);

            bag.Add(region);

            var adjacentRegions = GetAdjacentRegions(regionManagerC, region.RegionPosition, positionI);

            foreach (var adjRegions in adjacentRegions)
            {
                bag.Add(regionManagerC.GetRegion(adjRegions));
            }

            return bag;
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
            var type = (long)action.Parameters[CREATION_TYPE];
            var entity = Controller.Instance.RegionManagerController.GetRegion(positionI.RegionPosition).GetEntity(positionI.CellPosition);
            var list = new List<long>(); 

            action.Account.BuildableBuildings.TryGetValue(entity.DefinitionID, out list);

            if (list != null)
            {
                if (action.AccountID == entity.OwnerID && list.Contains(type))
                {                
                    RealCreatePosition = GetFreeField(positionI, regionManagerC);

                    return RealCreatePosition != null && LogicRules.ConsumeResource(action.Account, Controller.Instance.DefinitionManagerController.DefinitionManager.GetDefinition((EntityType)type));
                }
            }
            return false;
            
        }

        /// <summary>
        /// Apply action-related changes to the world.
        /// Returns set of changed Regions if everything worked, otherwise null
        /// </summary>
        /// <returns>all affected (changed) regions</returns>
        public override ConcurrentBag<Core.Models.Region> Do()
        {   
            var regionManagerC = Controller.Instance.RegionManagerController;

            var action = (Core.Models.Action)Model;
            var positionI = (PositionI)action.Parameters[CREATE_POSITION];
            var type = (long)action.Parameters[CREATION_TYPE];

            positionI = RealCreatePosition;
            var region = regionManagerC.GetRegion(positionI.RegionPosition);
            var entityDef = Controller.Instance.DefinitionManagerController.DefinitionManager.GetDefinition((EntityType)type);
            var unitHealth = ((UnitDefinition)entityDef).Health;
            var unitMoves = ((UnitDefinition)entityDef).Moves;

            // create the new entity and link to the correct account
            var entity = new Core.Models.Entity(
                IdGenerator.GetId(),
                entityDef,  
                action.Account,
                positionI,
                unitHealth,
                unitMoves);

            entity.Position = positionI;
            region.AddEntity(action.ActionTime, entity);

            if (action.Account != null)
            {
                action.Account.Units.AddLast(entity.Position);
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
     
        /// <summary>
        /// Check all possible spawn locations around a building.
        /// Check surrounding area around the building for units, if there is a free location this will be returned.
        /// </summary>
        /// <returns>The free field.</returns>
        /// <param name="position">IPosition of the current selected building..</param>
        /// <param name="regionManagerC">Region manager c.</param>
        private PositionI GetFreeField(PositionI position, RegionManagerController regionManagerC)
        {     
            foreach (var surpos in LogicRules.GetSurroundedFields(position))
            {                   
                var td = regionManagerC.GetRegion(surpos.RegionPosition).GetTerrain(surpos.CellPosition);
                var ed = regionManagerC.GetRegion(surpos.RegionPosition).GetEntity(surpos.CellPosition);

                if (td.Walkable && ed == null)
                {
                    return surpos;
                }
            }
            return null;
        }

       /// <summary>
       /// Gets the adjacent regions, which can be affected.
       /// </summary>
       /// <returns>The adjacent regions.</returns>
       /// <param name="regionManagerC">Region manager c.</param>
       /// <param name="position">Current Position od the selected building.</param>
       /// <param name="buildpoint">PositionI from the new unit.</param>
        private ConcurrentBag<RegionPosition> GetAdjacentRegions(RegionManagerController regionManagerC, RegionPosition position, PositionI buildpoint)
        {
            var list = new ConcurrentBag<RegionPosition>();
            var surlist = LogicRules.SurroundRegions;
            var regionSizeX = Constants.REGION_SIZE_X;
            var regionSizeY = Constants.REGION_SIZE_Y;

            var currentpos = LogicRules.GetSurroundedFields(buildpoint);
            var currentregion = regionManagerC.RegionManager.GetRegion(buildpoint.RegionPosition);

            foreach (var checkingpos in currentpos)
            {
                if (regionManagerC.GetRegion(checkingpos.RegionPosition) != currentregion)
                {
                    list.Add(regionManagerC.RegionManager.GetRegion(buildpoint.RegionPosition).RegionPosition);
                }
            }

            return list;
        }

        /// <summary>
        /// Position where the unit should be created
        /// </summary>
        public PositionI RealCreatePosition;
    }
}
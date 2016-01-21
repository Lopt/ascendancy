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
            var type = (long)action.Parameters[CREATION_TYPE];
            var test = regionManagerC.GetRegion(positionI.RegionPosition).GetEntity(positionI.CellPosition);

            if (type == (long)Models.Definitions.EntityType.Headquarter)
            {
                ConcurrentBag<Core.Models.Region> bag = new ConcurrentBag<Core.Models.Region>();
                var region = regionManagerC.GetRegion(positionI.RegionPosition);
                bag.Add(region);

                var adjacentRegions = GetAdjacentRegions(regionManagerC, region.RegionPosition, positionI);

                foreach (var adjRegions in adjacentRegions)
                {
                    bag.Add(regionManagerC.GetRegion(adjRegions));
                }

                return bag;
            }

            return new ConcurrentBag<Core.Models.Region>() { regionManagerC.GetRegion(positionI.RegionPosition) };
        }

        /// <summary>
        /// Returns if the action is even possible.
        /// </summary>
        /// <returns> True if the Building is buildable at the current position, otherwise false.</returns>
        public override bool Possible()
        {          
            var action = (Core.Models.Action)Model;
            var entityPosition = (PositionI)action.Parameters[CREATE_POSITION];
            var entityCellPostion = entityPosition.CellPosition;
            var region = Controller.Instance.RegionManagerController.GetRegion(entityPosition.RegionPosition);
            var type = (long)action.Parameters[CREATION_TYPE];

            if (!action.Account.TerritoryBuildings.ContainsKey((long)Core.Models.Definitions.EntityType.Headquarter) && 
                type == (long)Models.Definitions.EntityType.Headquarter &&
                region.GetEntity(entityCellPostion) == null &&
                region.GetClaimedTerritory(entityCellPostion) == null)
            {
                // terrain check
                var td = (TerrainDefinition)region.GetTerrain(entityCellPostion);
                var list = LogicRules.GetSurroundedPositions(entityPosition, Constants.HEADQUARTER_TERRITORY_RANGE);
                bool territoryFlag = true;
                // check the map for enemy territory if there is a enemy territory to close at new borders a territory building cant be build
                foreach (var position in list)
                {
                    if (region.GetClaimedTerritory(position.CellPosition) != null)
                    {
                        territoryFlag = false;
                    }                  
                }
                if (territoryFlag)
                {
                    m_headquarterFlag = true;
                    return td.Buildable; 
                }
            }           
            else if (region.GetEntity(entityCellPostion) == null && 
                     type != (long)Models.Definitions.EntityType.Headquarter &&
                     region.GetClaimedTerritory(entityCellPostion) == action.Account)
            {
                // check for free tile and the terrain is possesed from the current player
                // terrain check
                var td = (TerrainDefinition)region.GetTerrain(entityCellPostion);
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
            var action = (Core.Models.Action)Model;
            var entityPosition = (PositionI)action.Parameters[CREATE_POSITION];
            var region = Controller.Instance.RegionManagerController.GetRegion(entityPosition.RegionPosition);
            var entityCellPostion = entityPosition.CellPosition;
            var type = (long)action.Parameters[CREATION_TYPE];

            var entityDef = Controller.Instance.DefinitionManagerController.DefinitionManager.GetDefinition((EntityType)type);
            var entityHealth = ((UnitDefinition)entityDef).Health; 
            var entityMoves = ((UnitDefinition)entityDef).Moves;

            // create the new entity and link to the correct account
            var entity = new Core.Models.Entity(
                IdGenerator.GetId(),               
                entityDef,  
                action.Account,
                entityPosition,
                entityHealth,
                entityMoves);

            entity.Position = entityPosition;
            region.AddEntity(action.ActionTime, entity);

            // link the headquarter to the current account and claim territory, enable build options
            if (m_headquarterFlag && 
                action.Account != null)
            {
                action.Account.TerritoryBuildings.Add(type, entity.Position);             
                LogicRules.EnableBuildOptions(type, action.Account);
                region.ClaimTerritory(LogicRules.GetSurroundedPositions(entityPosition, Constants.HEADQUARTER_TERRITORY_RANGE), action.Account, region.RegionPosition, Controller.Instance.RegionManagerController.RegionManager);
                LogicRules.IncreaseHoleStorage(action.Account);
                LogicRules.GatherResources(action.Account, Controller.Instance.RegionManagerController);
            }
            else if (action.Account != null)
            {
                action.Account.Buildings.AddLast(entity.Position);
                LogicRules.EnableBuildOptions(type, action.Account);
                LogicRules.IncreaseStorage(action.Account, entity);
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

            if (buildpoint.CellPosition.CellX <= Constants.HEADQUARTER_TERRITORY_RANGE)
            {
                var tempReg = position + surlist[LogicRules.SurroundRegions.Length - 1];
                if (regionManagerC.GetRegion(tempReg).Exist)
                {
                    list.Add(tempReg);
                }

                for (int index = 0; index < 4; ++index)
                {
                    tempReg = position + surlist[index];
                    if (regionManagerC.GetRegion(tempReg).Exist)
                    {
                        list.Add(tempReg);
                    }
                }
            }
            else if (buildpoint.CellPosition.CellY <= Constants.HEADQUARTER_TERRITORY_RANGE)
            {
                for (int index = 5; index < LogicRules.SurroundRegions.Length; ++index)
                {
                    var tempReg = position + surlist[index];
                    if (regionManagerC.GetRegion(tempReg).Exist)
                    {
                        list.Add(tempReg);
                    }
                }

                var reg = position + surlist[0];
                if (regionManagerC.GetRegion(reg).Exist)
                {
                    list.Add(reg);
                }
                reg = position + surlist[1];
                if (regionManagerC.GetRegion(reg).Exist)
                {
                    list.Add(reg);
                }
            }
            else if (buildpoint.CellPosition.CellX >= regionSizeX - Constants.HEADQUARTER_TERRITORY_RANGE)
            {
                for (int index = 1; index < 6; ++index)
                {
                    var tempReg = position + surlist[index];
                    if (regionManagerC.GetRegion(tempReg).Exist)
                    {
                        list.Add(tempReg);
                    }
                }
            }
            else if (buildpoint.CellPosition.CellY >= regionSizeY - Constants.HEADQUARTER_TERRITORY_RANGE)
            {
                for (int index = 3; index < LogicRules.SurroundRegions.Length; ++index)
                {
                    var tempReg = position + surlist[index];
                    if (regionManagerC.GetRegion(tempReg).Exist)
                    {
                        list.Add(tempReg);
                    }
                }
            }
            return list;
        }

        /// <summary>
        /// The m headquarter flag.
        /// </summary>
        private bool m_headquarterFlag = false;
    }
}
namespace Core.Controllers.Actions
{
    using System;
    using System.Collections;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    using Core.Controllers.Actions;
    using Core.Controllers.AStar;
    using Core.Models;
    using Core.Models.Definitions;

    /// <summary>
    /// Action to move an unit
    /// </summary>
    public class MoveUnit : Action
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Core.Controllers.Actions.MoveUnit"/> class.
        /// </summary>
        /// <param name="model">action model.</param>
        public MoveUnit(Core.Models.ModelEntity model)
            : base(model)
        {
            var action = (Core.Models.Action)Model;
            var param = action.Parameters;

            if (param[START_POSITION].GetType() != typeof(PositionI))
            {
                param[START_POSITION] = new Core.Models.PositionI((Newtonsoft.Json.Linq.JContainer)param[START_POSITION]);
            }

            if (param[END_POSITION].GetType() != typeof(PositionI))
            {
                param[END_POSITION] = new Core.Models.PositionI((Newtonsoft.Json.Linq.JContainer)param[END_POSITION]);
            }
        }

        /// <summary>
        /// Position where the unit stands
        /// </summary>
        public const string START_POSITION = "EntityPosition";

        /// <summary>
        /// Target Position
        /// </summary>
        public const string END_POSITION = "NewPosition";

        /// <summary>
        /// don't send from the client
        /// </summary>
        public const string CLIENT_UNIT_INFOS = "Unit";

        /// <summary>
        /// Returns a bag of all regions which could be affected by this action.
        /// </summary>
        /// <returns>The affected regions.</returns>
        public override ConcurrentBag<Core.Models.Region> GetAffectedRegions()
        {
            var regionManagerC = Controller.Instance.RegionManagerController;

            var bag = new ConcurrentBag<Core.Models.Region>();

            var action = (Core.Models.Action)Model;
            var startPosition = (PositionI)action.Parameters[START_POSITION];
            var endPosition = (PositionI)action.Parameters[END_POSITION];

            bag.Add(regionManagerC.GetRegion(startPosition.RegionPosition));
            var adjacentRegions = GetAdjacentRegions(regionManagerC, regionManagerC.GetRegion(startPosition.RegionPosition).RegionPosition);

            foreach (var adjRegions in adjacentRegions)
            {
                bag.Add(regionManagerC.GetRegion(adjRegions));
            }

            return bag;
        }

        /// <summary>
        /// Returns if the action is even possible.
        /// </summary>
        /// <returns>true if this is action possible</returns>
        public override bool Possible()
        {
            var action = (Core.Models.Action)Model;

            var startPosition = (PositionI)action.Parameters[START_POSITION];
            var endPosition = (PositionI)action.Parameters[END_POSITION];
            var unit = Controller.Instance.RegionManagerController.GetRegion(startPosition.RegionPosition).GetEntity(startPosition.CellPosition);
            var endLocationUnit = Controller.Instance.RegionManagerController.GetRegion(endPosition.RegionPosition).GetEntity(endPosition.CellPosition);

            if (startPosition == endPosition)
            {
                return false;
            }        

            if (unit != null && action.Account != null && action.Account.ID == unit.Owner.ID)
            {
                var pathfinder = new PathFinder(new SearchParameters(startPosition, endPosition, action.Account.ID));
                Path = pathfinder.FindPath(((UnitDefinition)unit.Definition).Moves);

                if (endLocationUnit != null && endLocationUnit.OwnerID != action.AccountID)
                {
                    if (((UnitDefinition)unit.Definition).AttackRange >= Path.Count)
                    {
                        m_fight = true;
                        m_fightDistance = Path.Count;
                        Path.Clear();
                        return true;         
                    }
                    else
                    {
                        return false;
                    }                   
                }
                return Path.Count != 0;
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
            var action = (Core.Models.Action)Model;
            var startPosition = (PositionI)action.Parameters[START_POSITION];
            var endPosition = (PositionI)action.Parameters[END_POSITION];

            var regionStartPos = Controller.Instance.RegionManagerController.GetRegion(startPosition.RegionPosition);
            var regionEndPos = Controller.Instance.RegionManagerController.GetRegion(endPosition.RegionPosition);
            var bag = new ConcurrentBag<Core.Models.Region>();

            var entity = regionStartPos.GetEntity(startPosition.CellPosition);

            if (m_fight)
            {
                var enemyEntity = regionEndPos.GetEntity(endPosition.CellPosition);

                // Ranged attack units deal only 1 dmg to buildings
                if (((UnitDefinition)entity.Definition).AttackRange >= 1 && enemyEntity.Definition.Category == Category.Building)
                {                    
                    enemyEntity.Health -= 1;
                }
                else if (((UnitDefinition)entity.Definition).AttackRange - m_fightDistance < 0)
                {
                    // iterate trough all methods to modifie the attack
                    LogicRules.AllAttackModifierRangedInMeele(entity);

                    enemyEntity.Health += entity.ModfiedAttackValue - enemyEntity.ModifiedDefenseValue;

                    if (((UnitDefinition)enemyEntity.Definition).AttackRange >= m_fightDistance)
                    {
                        entity.Health += enemyEntity.ModfiedAttackValue - entity.ModifiedDefenseValue;
                    }
                }
                else
                {
                    // iterate trough all methods to modifie the attack
                    LogicRules.AllAttackModifier(entity);
                    LogicRules.AllDefenseModifier(enemyEntity);

                    enemyEntity.Health += entity.ModfiedAttackValue - enemyEntity.ModifiedDefenseValue;

                    if (((UnitDefinition)enemyEntity.Definition).AttackRange >= m_fightDistance)
                    {
                        entity.Health += enemyEntity.ModfiedAttackValue - entity.ModifiedDefenseValue;
                    }
                }

                if (enemyEntity.Health <= 0)
                { 
                    LogicRules.DestroyBuilding(enemyEntity, regionEndPos, action.ActionTime, Controller.Instance.RegionManagerController);
                    regionEndPos.RemoveEntity(action.ActionTime, enemyEntity);
                    enemyEntity.Owner.Units.Remove(enemyEntity.Position);
                }
                else 
                {
                    LogicRules.DestroyBuilding(entity, regionStartPos, action.ActionTime, Controller.Instance.RegionManagerController);
                    regionStartPos.RemoveEntity(action.ActionTime, entity);
                    entity.Owner.Units.Remove(entity.Position);
                }
            }
            else
            {                
                regionStartPos.RemoveEntity(action.ActionTime, entity);
                regionEndPos.AddEntity(action.ActionTime, entity);   
                entity.Position = endPosition;
            }

            bag.Add(regionStartPos);

            if (startPosition.RegionPosition != endPosition.RegionPosition)
            {
                bag.Add(regionEndPos);
            }

            action.Parameters[CLIENT_UNIT_INFOS] = entity;

            return bag;
        }

        /// <summary>
        /// Gets the region position.
        /// </summary>
        /// <returns>The region position.</returns>
        public override Core.Models.RegionPosition GetRegionPosition()
        {
            var action = (Core.Models.Action)Model;

            var startPosition = (PositionI)action.Parameters[START_POSITION];
            return startPosition.RegionPosition;
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
        /// Check all possible regions around the start region of a unit and add them to a ConcurrentBag.
        /// </summary>
        /// <returns>The adjacent regions.</returns>
        /// <param name="regionManagerC">Region manager c.</param>
        /// <param name="position">PositionI of the region.</param>
        private ConcurrentBag<RegionPosition> GetAdjacentRegions(RegionManagerController regionManagerC, RegionPosition position)
        {
            var list = new ConcurrentBag<RegionPosition>();
            var surlist = LogicRules.SurroundRegions;
            var regionSizeX = Constants.REGION_SIZE_X / 2;
            var regionSizeY = Constants.REGION_SIZE_Y / 2;

            if (position.RegionX <= regionSizeX && position.RegionY <= regionSizeY)
            {
                var tempReg = position + surlist[LogicRules.SurroundRegions.Length];
                if (regionManagerC.GetRegion(tempReg).Exist)
                {
                    list.Add(tempReg);
                }

                for (int index = 0; index < 2; ++index)
                {
                    tempReg = position + surlist[index];
                    if (regionManagerC.GetRegion(tempReg).Exist)
                    {
                        list.Add(tempReg);
                    }
                }
            }
            else if (position.RegionX > regionSizeX && position.RegionY <= regionSizeY)
            {
                for (int index = 1; index < 4; ++index)
                {
                    var tempReg = position + surlist[index];
                    if (regionManagerC.GetRegion(tempReg).Exist)
                    {
                        list.Add(tempReg);
                    }
                }
            }
            else if (position.RegionX > regionSizeX && position.RegionY > regionSizeY)
            {
                for (int index = 3; index < 7; ++index)
                {
                    var tempReg = position + surlist[index];
                    if (regionManagerC.GetRegion(tempReg).Exist)
                    {
                        list.Add(tempReg);
                    }
                }
            }
            else
            {
                for (int index = 5; index < 8; ++index)
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
        /// The path.
        /// </summary>
        public IList Path;

        /// <summary>
        /// The m fight.
        /// </summary>
        private bool m_fight;

        /// <summary>
        /// The m fight distance.
        /// </summary>
        private int m_fightDistance;
    }
}
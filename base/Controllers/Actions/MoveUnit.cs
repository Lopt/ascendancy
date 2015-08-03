using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using Core.Controllers.Actions;
using Core.Models.Definitions;
using Core.Models;
using System.Collections;
using Core.Controllers.AStar;

namespace Core.Controllers.Actions
{
    public class MoveUnit : Action
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Core.Models.Actions.MoveUnit"/> class.
        /// </summary>
        /// <param name="model">Model.</param>
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

        public const string START_POSITION = "EntityPosition";
        public const string END_POSITION = "NewPosition";


        // don't send from the client
        public const string CLIENT_UNIT_INFOS = "Unit";

        /// <summary>
        /// Gets the affected regions.
        /// </summary>
        /// <returns>The affected regions.</returns>
        /// <param name="regionManagerC">Region manager c.</param>
        public override ConcurrentBag<Core.Models.Region> GetAffectedRegions()
        {
            var regionManagerC = Controller.Instance.RegionManagerController;

            var Bag = new ConcurrentBag<Core.Models.Region>();

            var action = (Core.Models.Action)Model;
            var startPosition = (PositionI)action.Parameters[START_POSITION];
            var endPosition = (PositionI)action.Parameters[END_POSITION];

            Bag.Add(regionManagerC.GetRegion(startPosition.RegionPosition));
            var adjacentRegions = GetAdjacentRegions(regionManagerC, regionManagerC.GetRegion(startPosition.RegionPosition).RegionPosition);

            foreach (var adjRegions in adjacentRegions)
            {
                Bag.Add(regionManagerC.GetRegion(adjRegions));
            }

            return Bag;
        }

        /// <summary>
        /// Returns true if the action is even possible.
        /// </summary>
        /// <param name="regionManagerC"></param>
        /// <returns></returns>
        public override bool Possible()
        {
            var action = (Core.Models.Action)Model;

            var startPosition = (PositionI)action.Parameters[START_POSITION];
            var endPosition = (PositionI)action.Parameters[END_POSITION];
            var unit = Controller.Instance.RegionManagerController.GetRegion(startPosition.RegionPosition).GetEntity(startPosition.CellPosition);

            if (startPosition == endPosition)
            {
                return false;
            }

            if (unit != null && action.Account != null && action.Account.ID == unit.Account.ID)
            {
                var pathfinder = new PathFinder(new SearchParameters(startPosition, endPosition, action.Account.ID));
                Path = pathfinder.FindPath(((UnitDefinition)unit.Definition).Moves);
                
                if (Path.Count != 0)
                {
                    var destpoint = Controller.Instance.RegionManagerController.GetRegion(endPosition.RegionPosition).GetEntity(endPosition.CellPosition);

                    if (destpoint != null && action.Account.ID != unit.AccountID)
                    {
                        m_fightPos = (PositionI)Path[Path.Count - 1];
                        Path.RemoveAt(Path.Count - 1);
                        m_fight = true;
                    }
                    return true;   
                }     
            }
            return false;           
        }

        /// <summary>
        /// Apply action-related changes to the world.
        /// Returns set of changed Regions if everything worked, otherwise null
        /// </summary>
        /// <param name="regionManagerC">Region manager c.</param>
        public override ConcurrentBag<Core.Models.Region> Do()
        {
            var regionManagerC = Controller.Instance.RegionManagerController;

            var Bag = new ConcurrentBag<Core.Models.Region>();

            var action = (Core.Models.Action)Model;

            var startPosition = (PositionI)action.Parameters[START_POSITION];
            var endPosition = (PositionI)action.Parameters[END_POSITION];
            
            var entity = regionManagerC.GetRegion(startPosition.RegionPosition).GetEntity(startPosition.CellPosition);
            regionManagerC.GetRegion(startPosition.RegionPosition).RemoveEntity(action.ActionTime, entity);

            if (m_fight)
            {
                if (LogicRules.FightSystem())
                {
                    var defeatetentity = regionManagerC.GetRegion(endPosition.RegionPosition).GetEntity(endPosition.CellPosition);
                    regionManagerC.GetRegion(m_fightPos.RegionPosition).AddEntity(action.ActionTime, entity);
                    regionManagerC.GetRegion(endPosition.RegionPosition).RemoveEntity(action.ActionTime, defeatetentity);
                    entity.Position = m_fightPos;
                }

            }    

            Bag.Add(regionManagerC.GetRegion(startPosition.RegionPosition));

            if (startPosition.RegionPosition != endPosition.RegionPosition)
            {
                Bag.Add(regionManagerC.GetRegion(endPosition.RegionPosition));
            }


            action.Parameters[CLIENT_UNIT_INFOS] = entity;

            return Bag;
        }

        /// <summary>
        /// In case of errors, revert the world data to a valid state.
        /// </summary> 
        public override bool Catch()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Check all possible regions around the startregion of a unit and add them to a ConcurrentBag.
        /// </summary>
        /// <param name="regionManagerC"></param>
        /// <param name="position"></param>
        /// <returns> Returns all regions around the startregion</returns>
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
        /// Gets the region position.
        /// </summary>
        /// <returns>The region position.</returns>
        override public Core.Models.RegionPosition GetRegionPosition()
        {
            var action = (Core.Models.Action)Model;

            var startPosition = (PositionI)action.Parameters[START_POSITION];
            return startPosition.RegionPosition;
        }

        public IList Path;
        private bool m_fight;
        private PositionI m_fightPos;
    }
}


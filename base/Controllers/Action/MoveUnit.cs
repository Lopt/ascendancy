using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using @base.control;
using @base.model.definitions;
using @base.model;
using @base.Models;
using @base.Models.Definition;
using System.Collections;
using AStar;
using @base.control.action;

namespace @base.control.action
{
    public class MoveUnit : Action
    {
        public MoveUnit(model.ModelEntity model)
            : base(model)
        {
            var action = (model.Action)Model;
            var param = action.Parameters;


            if (param[START_POSITION].GetType() != typeof(PositionI))
            {
                param[START_POSITION] = new model.PositionI((Newtonsoft.Json.Linq.JContainer)param[START_POSITION]);
            }

            if (param[END_POSITION].GetType() != typeof(PositionI))
            {
                param[END_POSITION] = new model.PositionI((Newtonsoft.Json.Linq.JContainer)param[END_POSITION]);
            }
        }

        public const string START_POSITION = "EntityPosition";
        public const string END_POSITION = "NewPosition";

        /// <summary>
        /// 
        /// </summary>
        /// <param name="regionManagerC"></param>
        /// <returns></returns>
        public override ConcurrentBag<model.Region> GetAffectedRegions(RegionManagerController regionManagerC)
        {
            var Bag = new ConcurrentBag<model.Region>();

            var action = (model.Action)Model;
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
        ///  Returns if the action is even possible.
        /// </summary>
        /// <param name="regionManagerC"></param>
        /// <returns></returns>
        public override bool Possible (RegionManagerController regionManagerC)
        {
            var action = (model.Action)Model;

            var startPosition = (PositionI)action.Parameters[START_POSITION];
            var endPosition = (PositionI)action.Parameters[END_POSITION];
            var unit = Controller.Instance.RegionManagerController.GetRegion(startPosition.RegionPosition).GetEntity(startPosition.CellPosition);
                   

            if (unit != null && action.Account.ID == unit.Account.ID)
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
        /// </summary>
        /// <param name="regionManagerC"></param>
        /// <returns> </returns>
        public override ConcurrentBag<model.Region> Do (RegionManagerController regionManagerC)
        {
            var Bag = new ConcurrentBag<model.Region>();

            var action = (model.Action)Model;

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

            return Bag;
        }

        /// <summary>
        /// In case of errors, revert the world data to a valid state.
        /// </summary> 
        virtual public bool Catch(RegionManagerController regionManagerC)
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

            if (position.RegionX <= regionSizeX  && position.RegionY <= regionSizeY)
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
            else if (position.RegionX > regionSizeX  && position.RegionY <= regionSizeY)
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
            else if (position.RegionX > regionSizeX  && position.RegionY > regionSizeY)
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

        override public @base.model.RegionPosition GetRegionPosition()
        {
            var action = (model.Action)Model;

            var startPosition = (PositionI)action.Parameters[START_POSITION];
            return startPosition.RegionPosition;
        }

        public IList Path;
        private bool m_fight;
        private PositionI m_fightPos;
    }
}


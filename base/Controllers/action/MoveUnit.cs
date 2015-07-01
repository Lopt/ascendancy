using System;
using System.Collections.Concurrent;
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
            m_Bag = new ConcurrentBag<model.Region>();

            var action = (model.Action)Model;
            var startPosition = new model.PositionI((Newtonsoft.Json.Linq.JContainer)action.Parameters[START_POSITION]);
            var endPosition = new model.PositionI((Newtonsoft.Json.Linq.JContainer)action.Parameters[END_POSITION]);

            m_Bag.Add(regionManagerC.GetRegion(startPosition.RegionPosition));
            var adjacentRegions = GetAdjacentRegions(regionManagerC, regionManagerC.GetRegion(startPosition.RegionPosition).RegionPosition);

            foreach (var adjRegions in adjacentRegions)
            {
                m_Bag.Add(regionManagerC.GetRegion(adjRegions));
            }

            return m_Bag;
        }

        /// <summary>
        ///  Returns if the action is even possible.
        /// </summary>
        /// <param name="regionManagerC"></param>
        /// <returns></returns>
        public override bool Possible (RegionManagerController regionManagerC)
        {
            var action = (model.Action)Model;

            var startPosI = new model.PositionI((Newtonsoft.Json.Linq.JContainer)action.Parameters[START_POSITION]);
            var endPosI = new model.PositionI((Newtonsoft.Json.Linq.JContainer)action.Parameters[END_POSITION]);
            var unit = Controller.Instance.RegionManagerController.GetRegion(startPosI.RegionPosition).GetEntity(startPosI.CellPosition);
        
            if (action.Account.ID == unit.Account.ID)
            {
                var pathfinder = new PathFinder(new SearchParameters(startPosI, endPosI));             
                Path = pathfinder.FindPath( ((UnitDefinition) unit.Definition).Moves);
                return Path.Count != 0;                
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
            var action = (model.Action)Model;

            var startPosition = new model.PositionI((Newtonsoft.Json.Linq.JContainer)action.Parameters[START_POSITION]);
            var endPosI = new model.PositionI((Newtonsoft.Json.Linq.JContainer)action.Parameters[END_POSITION]);

            startPosition = endPosI;

            return m_Bag;
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
            var positionI = new model.PositionI((Newtonsoft.Json.Linq.JContainer) action.Parameters[START_POSITION]);
            return positionI.RegionPosition;
        }

        public IList Path;
        private ConcurrentBag<model.Region> m_Bag;
    }
}


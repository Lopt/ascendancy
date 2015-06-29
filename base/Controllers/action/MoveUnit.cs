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

namespace @base.control.action
{
    public class MoveEntity : Action
    {
        public MoveEntity(model.ModelEntity model)
            : base(model)
        {
        }

        public const string START_POSITION = "EntityPosition";
        public const string END_POSITION = "NewPosition";
        public const string UNIT_TYPE = "UnitType";

        /// <summary>
        /// 
        /// </summary>
        /// <param name="regionManagerC"></param>
        /// <returns></returns>
        public override ConcurrentBag<model.Region> GetAffectedRegions(RegionManagerController regionManagerC)
        {
            ConcurrentBag<model.Region> Bag = new ConcurrentBag<model.Region>();

            var action = (model.Action)Model;
            var startPosition = new model.PositionI((Newtonsoft.Json.Linq.JContainer)action.Parameters[START_POSITION]);
            var endPosition = new model.PositionI((Newtonsoft.Json.Linq.JContainer)action.Parameters[END_POSITION]);
            
            Bag.Add(regionManagerC.GetRegion(startPosition.RegionPosition));
            var adjacentRegions = GetAdjacentRegions(regionManagerC, regionManagerC.GetRegion(startPosition.RegionPosition).RegionPosition);

            foreach (var adjRegions in adjacentRegions)
            {
                Bag.Add(regionManagerC.GetRegion(adjRegions));
            }

            return Bag;
        }



        /// <summary>
        /// Returns if the action is even possible.
        /// </summary>
    /// 
        public override bool Possible (RegionManagerController regionManagerC)
        {   
            var action = (model.Action)Model;

                //if (Parameters.ContainsKey(CREATE_POSITION))
                //{
                //    var positionC = (CombinedPosition) Parameters[CREATE_POSITION];
                //    var region = regionManagerC.GetRegion(positionC.RegionPosition);
                //    if (region.Exist && this.Regions.Length == 1 && this.Regions[0] == region &&
                //        region.GetEntity(positionC.CellPosition) != null)
                //    {
                //        return true;
                //    }
                //}

            var startPosI = new model.PositionI((Newtonsoft.Json.Linq.JContainer)action.Parameters[START_POSITION]);
            var endPosI = new model.PositionI((Newtonsoft.Json.Linq.JContainer)action.Parameters[END_POSITION]);
            var unittype = (UnitDefinition)action.Parameters[UNIT_TYPE];           
            
            var pathfinder = new PathFinder(new SearchParameters(startPosI, endPosI));

            m_path = pathfinder.FindPath(unittype.Moves);

            return true;
        }

        ///// <summary>
        ///// Apply action-related changes to the world.
        ///// Returns false if something went terrible wrong
        ///// </summary>
        //public override ConcurrentBag<model.Region> Do (RegionManagerController regionManagerC)
        //{   
        //    var action = (model.Action)Model;

        //    var regionManagerC = Controller.Instance.RegionManagerController;
        //    var combinedPos = new model.CombinedPosition((Newtonsoft.Json.Linq.JContainer) action.Parameters[CREATE_POSITION]);
        //    var region = regionManagerC.GetRegion(combinedPos.RegionPosition);

        //    var entity = new @base.model.Entity(Guid.NewGuid(), 
        //        new UnitDefinition(Guid.NewGuid(),
        //            model.definitions.UnitDefinition.DefinitionType.building,
        //            UnitDefinition.UnitDefinitionType.Hero,
        //            new Action[0], 
        //            0, 0, 0, 0),
        //        combinedPos);

        //    entity.Position = combinedPos;
        //    region.AddEntity(action.ActionTime, entity);

        //    return true;
        //}

        /// <summary>
        /// In case of errors, revert the world data to a valid state.
        /// </summary>        public bool Catch()
        virtual public bool Catch(RegionManagerController regionManagerC)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Check all possible regions around the startregion of a unit and add them to a ConcurrentBag.
        /// </summary>
        /// <param name="regionManagerC"></param>
        /// <param name="position"></param>
        /// <returns> Get all regions around the startregion</returns>
        private ConcurrentBag<RegionPosition> GetAdjacentRegions(RegionManagerController regionManagerC, RegionPosition position)
        {
            var list = new ConcurrentBag<RegionPosition>();
            var surlist = LogicRules.SurroundRegions;


            if (position.RegionX <= Constants.REGION_SIZE_X / 2 && position.RegionY <= Constants.REGION_SIZE_Y / 2)
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
            else if (position.RegionX > Constants.REGION_SIZE_X / 2 && position.RegionY <= Constants.REGION_SIZE_Y / 2)
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
            else if (position.RegionX > Constants.REGION_SIZE_X / 2 && position.RegionY > Constants.REGION_SIZE_Y / 2)
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

        private IList m_path; 
    }
}


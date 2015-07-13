using System;
using System.Collections.Concurrent;

using Core.Controllers.Actions;
using Core.Models.Definitions;
using Core.Models;
using @base.Models.Definition;

namespace Core.Controllers.Actions
{
    public class CreateUnit : Action
    {
        public const string CREATE_POSITION = "CreatePosition";
        public const string CREATION_TYPE = "CreateUnit";

        /// <summary>
        /// Constructor of the class CreateUnit.
        /// </summary>
        /// <param name="model"></param>
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
        /// /// <summary>
        /// Initializes a new instance of the <see cref="base.control.action.Action"/> class.
        /// Identify alle affected regions by this action.       
        /// </summary>
        /// <param name="regionManagerC"></param>
        /// <returns> Returns <see cref="System.Collections.Concurrent.ConcurrentBag<t>"/> class with the affected regions. </returns>
        override public ConcurrentBag<Core.Models.Region> GetAffectedRegions(RegionManagerController regionManagerC)
        {
            ConcurrentBag<Core.Models.Region> Bag = new ConcurrentBag<Core.Models.Region>();
            var action = (Core.Models.Action)Model;
            var positionI = (PositionI)action.Parameters[CREATE_POSITION];
            var region = regionManagerC.GetRegion(positionI.RegionPosition);

            Bag.Add(region);

            var adjacentRegions = GetAdjacentRegions(regionManagerC, region.RegionPosition, positionI);

            foreach (var adjRegions in adjacentRegions)
            {
                Bag.Add(regionManagerC.GetRegion(adjRegions));
            }

            return Bag;
        }

        /// <summary>
        /// Returns if the action is even possible.
        /// </summary>
        /// <param name="regionManagerC"></param>
        /// <returns> True if the actions is possible, otherwise false.</returns>
        public override bool Possible(RegionManagerController regionManagerC)
        {   
            var action = (Core.Models.Action)Model;
            var positionI = (PositionI)action.Parameters[CREATE_POSITION];
            var type = (long)action.Parameters[CREATION_TYPE];

            RealCreatePosition = GetFreeField(positionI, regionManagerC);
            return RealCreatePosition != null;
        }

        /// <summary>
        /// Apply action-related changes to the world.
        /// </summary>
        /// <param name="regionManagerC"></param>
        /// <returns> Returns <see cref="System.Collections.Concurrent.ConcurrentBag<t>"/> class with the affected region.</returns>
        public override ConcurrentBag<Core.Models.Region> Do(RegionManagerController regionManagerC)
        {   
            var action = (Core.Models.Action)Model;
            var positionI = (PositionI)action.Parameters[CREATE_POSITION];
            var type = (long)action.Parameters[CREATION_TYPE];

            positionI = RealCreatePosition;
            var region = regionManagerC.GetRegion(positionI.RegionPosition);
            var dt = Controller.Instance.DefinitionManagerController.DefinitionManager.GetDefinition((EntityType)type);

            // create the new entity and link to the correct account
            var entity = new Core.Models.Entity(IdGenerator.GetId(),
                             dt,  
                             action.Account,
                             positionI);

            entity.Position = positionI;
            region.AddEntity(action.ActionTime, entity);

            if (action.Account != null)
            {
                action.Account.Units.AddLast(entity.Position);
            }


            return new ConcurrentBag<Core.Models.Region>() { region };
        }

        /// <summary>
        /// In case of errors, revert the world data to a valid state.
        /// </summary>
        public override bool Catch(RegionManagerController regionManagerC)
        {
            throw new NotImplementedException();
        }

     
        /// <summary>
        /// Check all possible spawn locations around a building.
        /// </summary>
        /// <param name="position"></param>
        /// <param name="regionManagerC"></param>
        /// <returns> Position which is free or null if nothing is free </returns>   
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
        /// 
        /// </summary>
        /// <param name="regionManagerC"></param>
        /// <param name="position"></param>
        /// <param name="buildpoint"></param>
        /// <returns></returns>
        private ConcurrentBag<RegionPosition> GetAdjacentRegions(RegionManagerController regionManagerC, RegionPosition position, PositionI buildpoint)
        {
            var list = new ConcurrentBag<RegionPosition>();
            var surlist = LogicRules.SurroundRegions;
            var regionSizeX = Constants.REGION_SIZE_X;
            var regionSizeY = Constants.REGION_SIZE_Y;

            if (buildpoint.CellPosition.CellX == 0)
            {
                var tempReg = position + surlist[LogicRules.SurroundRegions.Length];
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
            else if (buildpoint.CellPosition.CellY == 0)
            {
                for (int index = 5; index <= LogicRules.SurroundRegions.Length; ++index)
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
            else if (buildpoint.CellPosition.CellX == regionSizeX)
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
            else if (buildpoint.CellPosition.CellY == regionSizeY)
            {
                for (int index = 3; index <= LogicRules.SurroundRegions.Length; ++index)
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
            var positionI = (PositionI)action.Parameters[CREATE_POSITION];
            return positionI.RegionPosition;
        }

        public PositionI RealCreatePosition;
    }

}

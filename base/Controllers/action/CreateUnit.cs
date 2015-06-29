using System;
using System.Collections.Concurrent;

using @base.control;
using @base.model.definitions;
using @base.model;
using @base.Models.Definition;

namespace @base.control.action
{
    public class CreateUnit : Action
    {
        public const string CREATE_POSITION = "CreatePosition";
        public const string CREATION_TYPE = "CreateUnit";

        public CreateUnit(model.ModelEntity model)
            : base(model)
        {
            var action = (model.Action)Model;               
            var param = action.Parameters;

            if (param[CREATE_POSITION].GetType() != typeof(PositionI))
            {
                param[CREATE_POSITION] = new PositionI((Newtonsoft.Json.Linq.JContainer) param[CREATE_POSITION]);
            }
        }



        /// <summary>
        /// Initializes a new instance of the <see cref="base.control.action.Action"/> class.
        /// </summary>
        /// <param name="actionType">Action type.</param>
        /// <param name="regions">Affected Regions of this action.</param>
        /// <param name="parameters">Parameters.</param>
        override public ConcurrentBag<model.Region> GetAffectedRegions(RegionManagerController regionManagerC)
        {
            var action = (model.Action) Model;
            var positionI = (PositionI) action.Parameters[CREATE_POSITION];
            var region = regionManagerC.GetRegion(positionI.RegionPosition);
            return new ConcurrentBag<model.Region>() {  region };
        }

        /// <summary>
        /// Returns if the action is even possible.
        /// </summary>
        public override bool Possible(RegionManagerController regionManagerC)
        {   
            var action = (model.Action)Model;
            var positionI = (PositionI) action.Parameters[CREATE_POSITION];
            var type = (long) action.Parameters[CREATION_TYPE];

            if (CheckSurroundedArea(positionI, regionManagerC))
            {
                return true;              
            }
            return false;
        }

        /// <summary>
        /// Apply action-related changes to the world.
        /// Returns false if something went terrible wrong
        /// </summary>
        public override ConcurrentBag<model.Region> Do(RegionManagerController regionManagerC)
        {   
            var temp = LogicRules.SurroundTiles;
            var action = (model.Action)Model;
            var positionI = (PositionI) action.Parameters[CREATE_POSITION];
            var type = (long) action.Parameters[CREATION_TYPE];

            positionI = positionI + temp[m_index];
            var region = regionManagerC.GetRegion(positionI.RegionPosition);

            var dt = Controller.Instance.DefinitionManagerController.DefinitionManager.GetDefinition((int)type);
            // create the new entity and link to the correct account
            var entity = new @base.model.Entity(IdGenerator.GetId(),
                dt,  
                positionI);

            entity.Position = positionI;
            region.AddEntity(action.ActionTime, entity);
            action.Account.Units.AddLast(entity.Position);

            return new ConcurrentBag<model.Region>() { region };
        }

        /// <summary>
        /// In case of errors, revert the world data to a valid state.
        /// </summary>        public bool Catch()
        public override bool Catch(RegionManagerController regionManagerC)
        {
            throw new NotImplementedException();
        }


        /// <summary>
        /// Check all possible spawn locations around a building.        /// 
        /// </summary>       
        private bool CheckSurroundedArea(PositionI position, RegionManagerController regionManagerC)
        {      
            var temp = LogicRules.SurroundTiles;

            for (int index = 0; index < LogicRules.SurroundTiles.Length; ++index)
            {                   
                var surpos = temp[index] + position;

                var td = regionManagerC.GetRegion(surpos.RegionPosition).GetTerrain(surpos.CellPosition);
                var ed = regionManagerC.GetRegion(surpos.RegionPosition).GetEntity(surpos.CellPosition);

                if (td.Walkable && ed == null)
//                    td != TerrainDefinition.TerrainDefinitionType.Water &&
//                    ed.Definition.Type < UnitDefinition.DefinitionType.Unit)
                {
                    m_index = index;
                    return true;
                }

            }

            return false;
        }

        override public @base.model.RegionPosition GetRegionPosition()
        {
            var action = (model.Action)Model;
            var positionI = (PositionI)action.Parameters[CREATE_POSITION];
            return positionI.RegionPosition;
        }



        private int m_index;
    }

}

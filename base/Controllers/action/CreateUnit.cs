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
        public CreateUnit(model.ModelEntity model)
            : base(model)
        {
        }

        public const string CREATE_POSITION = "CreatePosition";

        /// <summary>
        /// Initializes a new instance of the <see cref="base.control.action.Action"/> class.
        /// </summary>
        /// <param name="actionType">Action type.</param>
        /// <param name="regions">Affected Regions of this action.</param>
        /// <param name="parameters">Parameters.</param>
        override public ConcurrentBag<model.Region> GetAffectedRegions(RegionManagerController regionManagerC)
        {
            var action = (model.Action)Model;               
            var positionI = new model.PositionI((Newtonsoft.Json.Linq.JContainer) action.Parameters[CREATE_POSITION]);

            return new ConcurrentBag<model.Region>() { regionManagerC.GetRegion(positionI.RegionPosition) }; 
        }

        /// <summary>
        /// Returns if the action is even possible.
        /// </summary>
        public override bool Possible(RegionManagerController regionManagerC)
        {   
            var action = (model.Action)Model;
            var positionI = new model.PositionI((Newtonsoft.Json.Linq.JContainer)action.Parameters[CREATE_POSITION]);         
             
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
        public override ConcurrentBag<model.Region> Do(RegionManagerController regionManagerC, UnitDefinition.UnitDefinitionType type)
        {   
            string[] actionParameter = {"CreateUnits"};

            var temp = LogicRules.SurroundTiles.ToArray();
            var action = (model.Action)Model;
            var positionI = new model.PositionI((Newtonsoft.Json.Linq.JContainer) action.Parameters[CREATE_POSITION]);
            var region = regionManagerC.GetRegion(positionI.RegionPosition);

            positionI += temp[m_index];

            // create the new entity and link to the correct account
            var entity = new @base.model.Entity(action.Account.ID, 
                new UnitDefinition(type,
                    actionParameter, 
                    0, 0, 0, 0),
                positionI);

            entity.Position = positionI;
            region.AddEntity(action.ActionTime, entity);
            action.Account.Units.Add(entity.Position);

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
           var temp = LogicRules.SurroundTiles.ToArray();

           for (int index = 0; index < LogicRules.SurroundTiles.Count; ++index)
            {                   
                var surpos = temp[index] + position;

                var td = regionManagerC.GetRegion(surpos.RegionPosition).GetTerrain(surpos.CellPosition).TerrainType;
                var ed = regionManagerC.GetRegion(surpos.RegionPosition).GetEntity(surpos.CellPosition);                            

                if (td != TerrainDefinition.TerrainDefinitionType.Forbidden &&
                    td != TerrainDefinition.TerrainDefinitionType.Water &&
                    ed.Definition.ID >= 60 && ed.Definition.ID <= 275)
                {
                    m_index = index;
                    return true;
                }

            }

            return false;
        }

        private int m_index;
    }
}

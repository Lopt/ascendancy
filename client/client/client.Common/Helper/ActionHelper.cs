using System;
using Core.Models;
using Core.Models.Definitions;

namespace client.Common.Helper
{
    public class ActionHelper
    {


        static public Core.Models.Action CreateEntity(PositionI positionI, Core.Models.Definitions.Definition definition)
        {
            switch (definition.Category)
            {
                case(Category.Building):
                    return CreateBuilding(positionI, definition);
                case(Category.Unit):
                    return CreateUnit(positionI, definition);
            }
            return null;
        }

        /// <summary>
        /// Creates the building.
        /// </summary>
        /// <returns>The building.</returns>
        /// <param name="positionI">Position.</param>
        /// <param name="definition">Definition.</param>
        static private  Core.Models.Action CreateUnit(PositionI positionI, Definition definition)
        {
            var dictParam = new System.Collections.Generic.Dictionary<string,object>();
            dictParam[Core.Controllers.Actions.CreateUnit.CREATE_POSITION] = positionI; 
            dictParam[Core.Controllers.Actions.CreateUnit.CREATION_TYPE] = (long)definition.SubType;
            return new  Core.Models.Action(GameAppDelegate.Account, Core.Models.Action.ActionType.CreateUnit, dictParam);
        }

        /// <summary>
        /// Creates the building.
        /// </summary>
        /// <returns>The building.</returns>
        /// <param name="positionI">Position i.</param>
        /// <param name="definition">Definition.</param>
        static private  Core.Models.Action CreateBuilding(PositionI positionI, Definition definition)
        {
            var dictParam = new System.Collections.Generic.Dictionary<string,object>();
            dictParam[Core.Controllers.Actions.CreatBuilding.CREATE_POSITION] = positionI; 
            dictParam[Core.Controllers.Actions.CreatBuilding.CREATION_TYPE] = (long)definition.SubType;
            return new  Core.Models.Action(GameAppDelegate.Account, Core.Models.Action.ActionType.CreateBuilding, dictParam);

        }


    }
}


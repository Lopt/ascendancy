﻿namespace Client.Common.Helper
{
    using System;
    using Core.Models;
    using Core.Models.Definitions;

    /// <summary>
    /// The Action helper create actions.
    /// </summary>
    public class ActionHelper
    {
        /// <summary>
        /// Creates the action to move a unit.
        /// </summary>
        /// <returns>The move unit action.</returns>
        /// <param name="start">Start position of move.</param>
        /// <param name="end">End position of move.</param>
        public static Core.Models.Action MoveUnit(PositionI start, PositionI end)
        {
            var dictParam = new System.Collections.Generic.Dictionary<string, object>();
            dictParam[Core.Controllers.Actions.MoveUnit.START_POSITION] = start;
            dictParam[Core.Controllers.Actions.MoveUnit.END_POSITION] = end;
            return new Core.Models.Action(GameAppDelegate.Account, Core.Models.Action.ActionType.MoveUnit, dictParam, GameAppDelegate.ServerTime);
        }

        /// <summary>
        /// Creates the action which creates the unit
        /// </summary>
        /// <returns>The entity create action.</returns>
        /// <param name="positionI">Position where the entity should be created.</param>
        /// <param name="definition">Definition which entity should be created.</param>
        /// <param name="account">Which account creates the action.</param>
        public static Core.Models.Action CreateEntity(PositionI positionI, Core.Models.Definitions.Definition definition, Account account)
        {
            switch (definition.Category)
            {
                // TODO: add more sub categorys and better check
                case Category.Building:
                    if (definition.SubType == EntityType.Headquarter)
                    {
                        return CreatTerritoryBuilding(positionI, definition, account);                
                    }
                    else if (definition.SubType == EntityType.GuardTower)
                    {
                        return CreatTerritoryBuilding(positionI, definition, account);
                    }
                    else
                    {
                        return CreateBuilding(positionI, definition, account);
                    }
                case Category.Unit:
                    return CreateUnit(positionI, definition, account);
            }
            return null;
        }

        /// <summary>
        /// Creates the action which creates the unit.
        /// </summary>
        /// <returns>The create unit action.</returns>
        /// <param name="positionI">Position where the unit should be created.</param>
        /// <param name="definition">Definition which unit should be created.</param>
        /// <param name="account">Which account creates the action.</param>
        private static Core.Models.Action CreateUnit(PositionI positionI, Definition definition, Account account)
        {
            var dictParam = new System.Collections.Generic.Dictionary<string, object>();
            dictParam[Core.Controllers.Actions.CreateUnit.CREATE_POSITION] = positionI; 
            dictParam[Core.Controllers.Actions.CreateUnit.CREATION_TYPE] = (long)definition.SubType;
            return new Core.Models.Action(account, Core.Models.Action.ActionType.CreateUnit, dictParam, GameAppDelegate.ServerTime);
        }

        /// <summary>
        /// Creates the action which creates a territory building.
        /// </summary>
        /// <returns>The territory building.</returns>
        /// <param name="positionI">Position where the building should be created.</param>
        /// <param name="definition">Definition which building should be created.</param>
        /// <param name="account">Which account creates the action.</param>
        private static Core.Models.Action CreatTerritoryBuilding(PositionI positionI, Definition definition, Account account)
        {
            var dictParam = new System.Collections.Generic.Dictionary<string, object>();
            dictParam[Core.Controllers.Actions.CreateBuilding.CREATE_POSITION] = positionI; 
            dictParam[Core.Controllers.Actions.CreateBuilding.CREATION_TYPE] = (long)definition.SubType;
            return new Core.Models.Action(account, Core.Models.Action.ActionType.CreateTerritoryBuilding, dictParam, GameAppDelegate.ServerTime);
        }

        /// <summary>
        /// Creates the action which creates the building.
        /// </summary>
        /// <returns>The create building action.</returns>
        /// <param name="positionI">Position where the building should be created.</param>
        /// <param name="definition">Definition which building should be created.</param>
        /// <param name="account">Which account creates the action.</param>
        private static Core.Models.Action CreateBuilding(PositionI positionI, Definition definition, Account account)
        {
            var dictParam = new System.Collections.Generic.Dictionary<string, object>();
            dictParam[Core.Controllers.Actions.CreateBuilding.CREATE_POSITION] = positionI; 
            dictParam[Core.Controllers.Actions.CreateBuilding.CREATION_TYPE] = (long)definition.SubType;
            return new Core.Models.Action(account, Core.Models.Action.ActionType.CreateBuilding, dictParam, GameAppDelegate.ServerTime);
        }
    }
}
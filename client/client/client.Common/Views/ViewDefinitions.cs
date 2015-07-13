using System;
using CocosSharp;
using Core.Models.Definitions;
using System.Collections.Generic;
using client.Common.Helper;
using Core.Models;


namespace client.Common.Views
{
    public class ViewDefinitions
    {
        public ViewDefinitions()
        {
            InitTerrainDefToTileGid();
            InitUnitDefToTileGid();
        }


        private void InitTerrainDefToTileGid()
        {
            m_TerrainDefToTileGid = new Dictionary<EntityType,  CCTileGidAndFlags>();

			m_TerrainDefToTileGid.Add(EntityType.Beach, new CCTileGidAndFlags(ClientConstants.BEACH_GID));
			m_TerrainDefToTileGid.Add(EntityType.Buildings, new CCTileGidAndFlags(ClientConstants.BUILDINGS_GID));
			m_TerrainDefToTileGid.Add(EntityType.Fields, new CCTileGidAndFlags(ClientConstants.FIELDS_GID));
			m_TerrainDefToTileGid.Add(EntityType.Forbidden, new CCTileGidAndFlags(ClientConstants.FORBIDDEN_GID));
			m_TerrainDefToTileGid.Add(EntityType.Glacier, new CCTileGidAndFlags(ClientConstants.GLACIER_GID));
			m_TerrainDefToTileGid.Add(EntityType.Grassland, new CCTileGidAndFlags(ClientConstants.GRASSLAND_GID));
			m_TerrainDefToTileGid.Add(EntityType.Invalid, new CCTileGidAndFlags(ClientConstants.INVALID_GID));
			m_TerrainDefToTileGid.Add(EntityType.NotDefined, new CCTileGidAndFlags(ClientConstants.NOTDEFINED_GID));
			m_TerrainDefToTileGid.Add(EntityType.Park, new CCTileGidAndFlags(ClientConstants.PARK_GID));
			m_TerrainDefToTileGid.Add(EntityType.Streets, new CCTileGidAndFlags(ClientConstants.STREETS_GID));
			m_TerrainDefToTileGid.Add(EntityType.Town, new CCTileGidAndFlags(ClientConstants.TOWN_GID));
			m_TerrainDefToTileGid.Add(EntityType.Water, new CCTileGidAndFlags(ClientConstants.WATER_GID));
			m_TerrainDefToTileGid.Add(EntityType.Woods, new CCTileGidAndFlags(ClientConstants.WOODS_GID));
        }


        private void InitUnitDefToTileGid()
        {
			m_UnitDefToTileGid = new Dictionary<EntityType, CCTileGidAndFlags>();

			m_UnitDefToTileGid.Add(EntityType.Hero, new CCTileGidAndFlags(ClientConstants.HERO_GID));
			m_UnitDefToTileGid.Add(EntityType.Warrior, new CCTileGidAndFlags(ClientConstants.WARRIOR_GID));
			m_UnitDefToTileGid.Add(EntityType.Mage, new CCTileGidAndFlags(ClientConstants.MAGE_GID));
			m_UnitDefToTileGid.Add(EntityType.Scout, new CCTileGidAndFlags(ClientConstants.SCOUT_GID));
			m_UnitDefToTileGid.Add(EntityType.Archer, new CCTileGidAndFlags(ClientConstants.BOWMAN_GID));
			m_UnitDefToTileGid.Add(EntityType.Unknown3, new CCTileGidAndFlags(ClientConstants.UNKNOWN_GID));
			m_UnitDefToTileGid.Add(EntityType.Headquarter, new CCTileGidAndFlags(ClientConstants.HEADQUARTER_GID));
			m_UnitDefToTileGid.Add(EntityType.Outposts, new CCTileGidAndFlags(ClientConstants.HEADQUARTER_GID));
			m_UnitDefToTileGid.Add(EntityType.Houses, new CCTileGidAndFlags(ClientConstants.HOUSE_GID));
			m_UnitDefToTileGid.Add(EntityType.Wall, new CCTileGidAndFlags(ClientConstants.WALL1_GID));
			m_UnitDefToTileGid.Add(EntityType.Barracks, new CCTileGidAndFlags(ClientConstants.GARNISION_GID));
			m_UnitDefToTileGid.Add(EntityType.RessourceHarvester, new CCTileGidAndFlags(ClientConstants.FARM_GID));

        }

        public CCTileGidAndFlags DefinitionToTileGid(Definition definition)
        {
            CCTileGidAndFlags gid;

			if (definition.Category == Core.Models.Definitions.Category.Terrain)
            {
                var terrainDefinition = definition as TerrainDefinition;
                if (!m_TerrainDefToTileGid.TryGetValue(terrainDefinition.SubType, out gid))
                    gid = new CCTileGidAndFlags(ClientConstants.INVALID_GID);

            }
            else
            {
                var unitDefinition = definition as UnitDefinition;
                if (!m_UnitDefToTileGid.TryGetValue(unitDefinition.SubType, out gid))
                    gid = new CCTileGidAndFlags(ClientConstants.INVALID_GID);
            }

            return gid;
        }


        #region Fields

		private Dictionary<EntityType, CCTileGidAndFlags> m_UnitDefToTileGid;

		private Dictionary<EntityType, CCTileGidAndFlags> m_TerrainDefToTileGid;

        #endregion
    }
}


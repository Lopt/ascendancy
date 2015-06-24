using System;
using CocosSharp;
using @base.model.definitions;
using System.Collections.Generic;
using client.Common.Helper;
using @base.model;


namespace client.Common.Views
{
    public class ViewDefinitions
    {
        public ViewDefinitions ()
        {
            InitTerrainDefToTileGid ();
            InitUnitDefToTileGid ();
        }


        private void InitTerrainDefToTileGid ()
        {
            m_TerrainDefToTileGid = new Dictionary<TerrainDefinition.TerrainDefinitionType,  CCTileGidAndFlags> ();

            m_TerrainDefToTileGid.Add (TerrainDefinition.TerrainDefinitionType.Beach, new CCTileGidAndFlags (ClientConstants.BEACH_GID));
            m_TerrainDefToTileGid.Add (TerrainDefinition.TerrainDefinitionType.Buildings, new CCTileGidAndFlags (ClientConstants.BUILDINGS_GID));
            m_TerrainDefToTileGid.Add (TerrainDefinition.TerrainDefinitionType.Fields, new CCTileGidAndFlags (ClientConstants.FIELDS_GID));
            m_TerrainDefToTileGid.Add (TerrainDefinition.TerrainDefinitionType.Forbidden, new CCTileGidAndFlags (ClientConstants.FORBIDDEN_GID));
            m_TerrainDefToTileGid.Add (TerrainDefinition.TerrainDefinitionType.Glacier, new CCTileGidAndFlags (ClientConstants.GLACIER_GID));
            m_TerrainDefToTileGid.Add (TerrainDefinition.TerrainDefinitionType.Grassland, new CCTileGidAndFlags (ClientConstants.GRASSLAND_GID));
            m_TerrainDefToTileGid.Add (TerrainDefinition.TerrainDefinitionType.Invalid, new CCTileGidAndFlags (ClientConstants.INVALID_GID));
            m_TerrainDefToTileGid.Add (TerrainDefinition.TerrainDefinitionType.NotDefined, new CCTileGidAndFlags (ClientConstants.NOTDEFINED_GID));
            m_TerrainDefToTileGid.Add (TerrainDefinition.TerrainDefinitionType.Park, new CCTileGidAndFlags (ClientConstants.PARK_GID));
            m_TerrainDefToTileGid.Add (TerrainDefinition.TerrainDefinitionType.Streets, new CCTileGidAndFlags (ClientConstants.STREETS_GID));
            m_TerrainDefToTileGid.Add (TerrainDefinition.TerrainDefinitionType.Town, new CCTileGidAndFlags (ClientConstants.TOWN_GID));
            m_TerrainDefToTileGid.Add (TerrainDefinition.TerrainDefinitionType.Water, new CCTileGidAndFlags (ClientConstants.WATER_GID));
            m_TerrainDefToTileGid.Add (TerrainDefinition.TerrainDefinitionType.Woods, new CCTileGidAndFlags (ClientConstants.WOODS_GID));
        }


        private void InitUnitDefToTileGid ()
        {
            m_UnitDefToTileGid = new Dictionary<UnitDefinition.UnitDefinitionType, CCTileGidAndFlags> ();

            m_UnitDefToTileGid.Add (UnitDefinition.UnitDefinitionType.Hero, new CCTileGidAndFlags (ClientConstants.Bogenschütze));
            m_UnitDefToTileGid.Add (UnitDefinition.UnitDefinitionType.Warrior, new CCTileGidAndFlags (ClientConstants.Krieger));
            m_UnitDefToTileGid.Add (UnitDefinition.UnitDefinitionType.Mage, new CCTileGidAndFlags (ClientConstants.Magier));
            m_UnitDefToTileGid.Add (UnitDefinition.UnitDefinitionType.Unknown1, new CCTileGidAndFlags (ClientConstants.Späher));
            m_UnitDefToTileGid.Add (UnitDefinition.UnitDefinitionType.Unknown2, new CCTileGidAndFlags (ClientConstants.unbekanteUnit));
            m_UnitDefToTileGid.Add (UnitDefinition.UnitDefinitionType.Unknown3, new CCTileGidAndFlags (ClientConstants.unbekanteUnit));
            m_UnitDefToTileGid.Add (UnitDefinition.UnitDefinitionType.Headquarter, new CCTileGidAndFlags (ClientConstants.Hauptgebäude));
            m_UnitDefToTileGid.Add (UnitDefinition.UnitDefinitionType.Outposts, new CCTileGidAndFlags (ClientConstants.Hauptgebäude));
            m_UnitDefToTileGid.Add (UnitDefinition.UnitDefinitionType.Houses, new CCTileGidAndFlags (ClientConstants.Haus));
            m_UnitDefToTileGid.Add (UnitDefinition.UnitDefinitionType.Wall, new CCTileGidAndFlags (ClientConstants.Mauer1));
            m_UnitDefToTileGid.Add (UnitDefinition.UnitDefinitionType.Barracks, new CCTileGidAndFlags (ClientConstants.Garnision));
            m_UnitDefToTileGid.Add (UnitDefinition.UnitDefinitionType.RessourceHarvester, new CCTileGidAndFlags (ClientConstants.Farm));

        }

        public CCTileGidAndFlags DefinitionToTileGid (Definition definition)
        {
            CCTileGidAndFlags gid;

            if (definition.Type == Definition.DefinitionType.Terrain) {
                var terrainDefinition = definition as TerrainDefinition;
                if (!m_TerrainDefToTileGid.TryGetValue (terrainDefinition.TerrainType, out gid))
                    gid = new CCTileGidAndFlags (ClientConstants.INVALID_GID);

            } else {
                var unitDefinition = definition as UnitDefinition;
                if (!m_UnitDefToTileGid.TryGetValue (unitDefinition.UnitType, out gid))
                    gid = new CCTileGidAndFlags (ClientConstants.INVALID_GID);
            }

            return gid;
        }


        #region Fields

        private Dictionary<UnitDefinition.UnitDefinitionType, CCTileGidAndFlags> m_UnitDefToTileGid;

        private Dictionary<TerrainDefinition.TerrainDefinitionType, CCTileGidAndFlags> m_TerrainDefToTileGid;

        #endregion
    }
}


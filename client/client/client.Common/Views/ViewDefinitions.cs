using System;
using CocosSharp;
using Core.Models.Definitions;
using System.Collections.Generic;
using Client.Common.Helper;
using Core.Models;


namespace Client.Common.Views
{
    /// <summary>
    /// View definitions to set the correct pictures to the entity and terrain types. 
    /// </summary>
    public class ViewDefinitions
    {
        /// <summary>
        /// The definition sort.
        /// </summary>
        public enum Sort
        {
            Normal,
            Enemy,
            Menu,
        }

        /// <summary>
        /// The lazy m_singleton.
        /// </summary>
        private static readonly Lazy<ViewDefinitions> m_singleton =
            new Lazy<ViewDefinitions>(() => new ViewDefinitions());

        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <value>The instance.</value>
        public static ViewDefinitions Instance
        {
            get
            {
                return m_singleton.Value;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Client.Common.Views.ViewDefinitions"/> class.
        /// </summary>
        private ViewDefinitions()
        {
            InitTerrainsGid();
            InitEntitiesGid();
            InitMenuEntitiesGid();
            InitEnemyEntitiesGid();
        }

        /// <summary>
        /// Inits the terrains gids.
        /// </summary>
        private void InitTerrainsGid()
        {
            m_TerrainsGid = new Dictionary<EntityType,  CCTileGidAndFlags>();

            m_TerrainsGid.Add(EntityType.Beach, new CCTileGidAndFlags(client.Common.Helper.TerrainGid.BEACH));
            m_TerrainsGid.Add(EntityType.Buildings, new CCTileGidAndFlags(client.Common.Helper.TerrainGid.BUILDINGS));
            m_TerrainsGid.Add(EntityType.Fields, new CCTileGidAndFlags(client.Common.Helper.TerrainGid.FIELDS));
            m_TerrainsGid.Add(EntityType.Forbidden, new CCTileGidAndFlags(client.Common.Helper.TerrainGid.FORBIDDEN));
            m_TerrainsGid.Add(EntityType.Glacier, new CCTileGidAndFlags(client.Common.Helper.TerrainGid.GLACIER));
            m_TerrainsGid.Add(EntityType.Grassland, new CCTileGidAndFlags(client.Common.Helper.TerrainGid.GRASSLAND));
            m_TerrainsGid.Add(EntityType.Invalid, new CCTileGidAndFlags(client.Common.Helper.TerrainGid.INVALID));
            m_TerrainsGid.Add(EntityType.NotDefined, new CCTileGidAndFlags(client.Common.Helper.TerrainGid.NOTDEFINED));
            m_TerrainsGid.Add(EntityType.Park, new CCTileGidAndFlags(client.Common.Helper.TerrainGid.PARK));
            m_TerrainsGid.Add(EntityType.Streets, new CCTileGidAndFlags(client.Common.Helper.TerrainGid.STREETS));
            m_TerrainsGid.Add(EntityType.Town, new CCTileGidAndFlags(client.Common.Helper.TerrainGid.TOWN));
            m_TerrainsGid.Add(EntityType.Water, new CCTileGidAndFlags(client.Common.Helper.TerrainGid.WATER));
            m_TerrainsGid.Add(EntityType.Woods, new CCTileGidAndFlags(client.Common.Helper.TerrainGid.WOODS));
        }

        /// <summary>
        /// Inits the entities gids.
        /// </summary>
        private void InitEntitiesGid()
        {
            m_EntitiesGid = new Dictionary<EntityType, CCTileGidAndFlags>();

            m_EntitiesGid.Add(EntityType.Hero, new CCTileGidAndFlags(client.Common.Helper.UnitGid.HERO));
            m_EntitiesGid.Add(EntityType.Warrior, new CCTileGidAndFlags(client.Common.Helper.UnitGid.WARRIOR));
            m_EntitiesGid.Add(EntityType.Mage, new CCTileGidAndFlags(client.Common.Helper.UnitGid.MAGE));
            m_EntitiesGid.Add(EntityType.Scout, new CCTileGidAndFlags(client.Common.Helper.UnitGid.SCOUT));
            m_EntitiesGid.Add(EntityType.Archer, new CCTileGidAndFlags(client.Common.Helper.UnitGid.BOWMAN));
            m_EntitiesGid.Add(EntityType.Unknown3, new CCTileGidAndFlags(client.Common.Helper.UnitGid.UNKNOWN));
            m_EntitiesGid.Add(EntityType.Headquarter, new CCTileGidAndFlags(client.Common.Helper.BuildingGid.HEADQUARTER));
            m_EntitiesGid.Add(EntityType.Outposts, new CCTileGidAndFlags(client.Common.Helper.BuildingGid.HEADQUARTER));
            m_EntitiesGid.Add(EntityType.Houses, new CCTileGidAndFlags(client.Common.Helper.BuildingGid.HOUSE));
            m_EntitiesGid.Add(EntityType.Wall, new CCTileGidAndFlags(client.Common.Helper.BuildingGid.WALL1));
            m_EntitiesGid.Add(EntityType.Barracks, new CCTileGidAndFlags(client.Common.Helper.BuildingGid.GARNISION));
            m_EntitiesGid.Add(EntityType.RessourceHarvester, new CCTileGidAndFlags(client.Common.Helper.BuildingGid.FARM));

        }

        /// <summary>
        /// Inits the enemy entities gids.
        /// </summary>
        private void InitEnemyEntitiesGid()
        {
            m_EnemyEntitiesGid = new Dictionary<EntityType, CCTileGidAndFlags>();

            m_EnemyEntitiesGid.Add(EntityType.Hero, new CCTileGidAndFlags(client.Common.Helper.EnemyUnitGid.HERO));
            m_EnemyEntitiesGid.Add(EntityType.Warrior, new CCTileGidAndFlags(client.Common.Helper.EnemyUnitGid.WARRIOR));
            m_EnemyEntitiesGid.Add(EntityType.Mage, new CCTileGidAndFlags(client.Common.Helper.EnemyUnitGid.MAGE));
            m_EnemyEntitiesGid.Add(EntityType.Scout, new CCTileGidAndFlags(client.Common.Helper.EnemyUnitGid.SCOUT));
            m_EnemyEntitiesGid.Add(EntityType.Archer, new CCTileGidAndFlags(client.Common.Helper.EnemyUnitGid.BOWMAN));
            //m_EnemyEntitiesGid.Add(EntityType.Unknown3, new CCTileGidAndFlags(ClientConstants.ENEMYUNKNOWN_GID));
            m_EnemyEntitiesGid.Add(EntityType.Headquarter, new CCTileGidAndFlags(client.Common.Helper.EnemyBuildingGid.HEADQUARTER));
            m_EnemyEntitiesGid.Add(EntityType.Outposts, new CCTileGidAndFlags(client.Common.Helper.EnemyBuildingGid.HEADQUARTER));
            //            m_EnemyEntitiesGid.Add(EntityType.Houses, new CCTileGidAndFlags(ClientConstants.ENEMYHOUSE_GID));
            //m_EnemyEntitiesGid.Add(EntityType.Wall, new CCTileGidAndFlags(ClientConstants.ENEMYWALL1_GID));
            //m_EnemyEntitiesGid.Add(EntityType.Barracks, new CCTileGidAndFlags(ClientConstants.ENEMYGARNISION_GID));
            //m_EnemyEntitiesGid.Add(EntityType.RessourceHarvester, new CCTileGidAndFlags(ClientConstants.ENEMYFARM_GID));
        }

        /// <summary>
        /// Inits the menu entities gids.
        /// </summary>
        private void InitMenuEntitiesGid()
        {
            m_MenuEntitiesGid = new Dictionary<EntityType, CCTileGidAndFlags>();

            m_MenuEntitiesGid.Add(EntityType.Hero, new CCTileGidAndFlags(client.Common.Helper.UnitMenuGid.HERO));
            m_MenuEntitiesGid.Add(EntityType.Warrior, new CCTileGidAndFlags(client.Common.Helper.UnitMenuGid.WARRIOR));
            m_MenuEntitiesGid.Add(EntityType.Mage, new CCTileGidAndFlags(client.Common.Helper.UnitMenuGid.MAGE));
            m_MenuEntitiesGid.Add(EntityType.Scout, new CCTileGidAndFlags(client.Common.Helper.UnitMenuGid.SCOUT));
            m_MenuEntitiesGid.Add(EntityType.Archer, new CCTileGidAndFlags(client.Common.Helper.UnitMenuGid.BOWMAN));
            m_MenuEntitiesGid.Add(EntityType.Unknown3, new CCTileGidAndFlags(client.Common.Helper.UnitMenuGid.UNKNOWN));

            m_MenuEntitiesGid.Add(EntityType.Headquarter, new CCTileGidAndFlags(client.Common.Helper.BuildingMenuGid.HEADQUARTER));
            m_MenuEntitiesGid.Add(EntityType.Outposts, new CCTileGidAndFlags(client.Common.Helper.BuildingMenuGid.HEADQUARTER));
            m_MenuEntitiesGid.Add(EntityType.Houses, new CCTileGidAndFlags(client.Common.Helper.BuildingMenuGid.FIRE));
            m_MenuEntitiesGid.Add(EntityType.Wall, new CCTileGidAndFlags(client.Common.Helper.BuildingMenuGid.EARTH));
            m_MenuEntitiesGid.Add(EntityType.Barracks, new CCTileGidAndFlags(client.Common.Helper.BuildingMenuGid.GOLD));
            m_MenuEntitiesGid.Add(EntityType.RessourceHarvester, new CCTileGidAndFlags(client.Common.Helper.BuildingMenuGid.AIR));

        }

        /// <summary>
        /// Sets the Definitions to tile gids.
        /// </summary>
        /// <returns>The tile gid.</returns>
        /// <param name="definition">Definition.</param>
        /// <param name="sort">Sort.</param>
        public CCTileGidAndFlags DefinitionToTileGid(Definition definition, Sort sort = Sort.Normal)
        {
            CCTileGidAndFlags gid;
            gid = new CCTileGidAndFlags(ClientConstants.INVALID_GID);

            if (definition.Category == Core.Models.Definitions.Category.Terrain)
            {
                var terrainDefinition = definition as TerrainDefinition;
                m_TerrainsGid.TryGetValue(terrainDefinition.SubType, out gid);
            }
            else
            {
                var unitDefinition = definition as UnitDefinition;
                switch (sort)
                {
                    case(Sort.Normal):
                        m_EntitiesGid.TryGetValue(unitDefinition.SubType, out gid);
                        break;
                    case(Sort.Enemy):
                        m_EnemyEntitiesGid.TryGetValue(unitDefinition.SubType, out gid);
                        break;
                    case(Sort.Menu):
                        m_MenuEntitiesGid.TryGetValue(unitDefinition.SubType, out gid);
                        break;
                }
               
            }

            return gid;
        }


        #region Fields

        /// <summary>
        /// The m_entities gid.
        /// </summary>
        private Dictionary<EntityType, CCTileGidAndFlags> m_EntitiesGid;
        /// <summary>
        /// The m_enemy entities gid.
        /// </summary>
        private Dictionary<EntityType, CCTileGidAndFlags> m_EnemyEntitiesGid;
        /// <summary>
        /// The m_menu entities gid.
        /// </summary>
        private Dictionary<EntityType, CCTileGidAndFlags> m_MenuEntitiesGid;
        /// <summary>
        /// The m_terrains gid.
        /// </summary>
        private Dictionary<EntityType, CCTileGidAndFlags> m_TerrainsGid;

        #endregion
    }
}


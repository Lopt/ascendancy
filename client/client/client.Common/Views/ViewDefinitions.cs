namespace Client.Common.Views
{
    using System;
    using System.Collections.Generic;
    using Client.Common.Helper;
    using CocosSharp;
    using Core.Models;
    using Core.Models.Definitions;

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
        private static readonly Lazy<ViewDefinitions> Singleton =
            new Lazy<ViewDefinitions>(() => new ViewDefinitions());

        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <value>The instance.</value>
        public static ViewDefinitions Instance
        {
            get
            {
                return Singleton.Value;
            }
        }

        /// <summary>
        /// Sets the Definitions to tile GIDs.
        /// </summary>
        /// <returns>The tile GID.</returns>
        /// <param name="definition">Definition which GID is wanted.</param>
        /// <param name="sort">Sort, if it's a menu, enemy or normal.</param>
        public CCTileGidAndFlags DefinitionToTileGid(Definition definition, Sort sort = Sort.Normal)
        {
            CCTileGidAndFlags gid;
            gid = new CCTileGidAndFlags(Client.Common.Helper.TerrainGid.INVALID);

            if (definition.Category == Core.Models.Definitions.Category.Terrain)
            {
                var terrainDefinition = definition as TerrainDefinition;
                m_terrainsGid.TryGetValue(terrainDefinition.SubType, out gid);
            }
            else
            {
                var unitDefinition = definition as UnitDefinition;
                switch (sort)
                {
                    case Sort.Normal:
                        m_entitiesGid.TryGetValue(unitDefinition.SubType, out gid);
                        break;
                    case Sort.Enemy:
                        m_enemyEntitiesGid.TryGetValue(unitDefinition.SubType, out gid);
                        break;
                    case Sort.Menu:
                        m_menuEntitiesGid.TryGetValue(unitDefinition.SubType, out gid);
                        break;
                }
            }
            return gid;
        }

        /// <summary>
        /// Prevents a default instance of the <see cref="ViewDefinitions"/> class from being created.
        /// </summary>
        private ViewDefinitions()
        {
            InitTerrainsGid();
            InitEntitiesGid();
            InitMenuEntitiesGid();
            InitEnemyEntitiesGid();
        }

        /// <summary>
        /// Initialize the terrains GIDs.
        /// </summary>
        private void InitTerrainsGid()
        {
            m_terrainsGid = new Dictionary<EntityType,  CCTileGidAndFlags>();

            m_terrainsGid.Add(EntityType.Beach, new CCTileGidAndFlags(Client.Common.Helper.TerrainGid.BEACH));
            m_terrainsGid.Add(EntityType.Buildings, new CCTileGidAndFlags(Client.Common.Helper.TerrainGid.BUILDINGS));
            m_terrainsGid.Add(EntityType.Fields, new CCTileGidAndFlags(Client.Common.Helper.TerrainGid.FIELDS));
            m_terrainsGid.Add(EntityType.Forbidden, new CCTileGidAndFlags(Client.Common.Helper.TerrainGid.FORBIDDEN));
            m_terrainsGid.Add(EntityType.Glacier, new CCTileGidAndFlags(Client.Common.Helper.TerrainGid.GLACIER));
            m_terrainsGid.Add(EntityType.Grassland, new CCTileGidAndFlags(Client.Common.Helper.TerrainGid.GRASSLAND));
            m_terrainsGid.Add(EntityType.Invalid, new CCTileGidAndFlags(Client.Common.Helper.TerrainGid.INVALID));
            m_terrainsGid.Add(EntityType.NotDefined, new CCTileGidAndFlags(Client.Common.Helper.TerrainGid.NOTDEFINED));
            m_terrainsGid.Add(EntityType.Park, new CCTileGidAndFlags(Client.Common.Helper.TerrainGid.PARK));
            m_terrainsGid.Add(EntityType.Streets, new CCTileGidAndFlags(Client.Common.Helper.TerrainGid.STREETS));
            m_terrainsGid.Add(EntityType.Town, new CCTileGidAndFlags(Client.Common.Helper.TerrainGid.TOWN));
            m_terrainsGid.Add(EntityType.Water, new CCTileGidAndFlags(Client.Common.Helper.TerrainGid.WATER));
            m_terrainsGid.Add(EntityType.Woods, new CCTileGidAndFlags(Client.Common.Helper.TerrainGid.WOODS));
        }

        /// <summary>
        /// Initialize the entities GIDs.
        /// </summary>
        private void InitEntitiesGid()
        {
            m_entitiesGid = new Dictionary<EntityType, CCTileGidAndFlags>();

            m_entitiesGid.Add(EntityType.Hero, new CCTileGidAndFlags(Client.Common.Helper.UnitGid.HERO));
            m_entitiesGid.Add(EntityType.Warrior, new CCTileGidAndFlags(Client.Common.Helper.UnitGid.WARRIOR));
            m_entitiesGid.Add(EntityType.Mage, new CCTileGidAndFlags(Client.Common.Helper.UnitGid.MAGE));
            m_entitiesGid.Add(EntityType.Scout, new CCTileGidAndFlags(Client.Common.Helper.UnitGid.SCOUT));
            m_entitiesGid.Add(EntityType.Archer, new CCTileGidAndFlags(Client.Common.Helper.UnitGid.BOWMAN));
            m_entitiesGid.Add(EntityType.Unknown3, new CCTileGidAndFlags(Client.Common.Helper.UnitGid.UNKNOWN));
            m_entitiesGid.Add(EntityType.Headquarter, new CCTileGidAndFlags(Client.Common.Helper.BuildingGid.HEADQUARTER));
            m_entitiesGid.Add(EntityType.Outposts, new CCTileGidAndFlags(Client.Common.Helper.BuildingGid.HEADQUARTER));
            m_entitiesGid.Add(EntityType.Houses, new CCTileGidAndFlags(Client.Common.Helper.BuildingGid.HOUSE));
            m_entitiesGid.Add(EntityType.Wall, new CCTileGidAndFlags(Client.Common.Helper.BuildingGid.WALL1));
            m_entitiesGid.Add(EntityType.Barracks, new CCTileGidAndFlags(Client.Common.Helper.BuildingGid.GARNISION));
            m_entitiesGid.Add(EntityType.RessourceHarvester, new CCTileGidAndFlags(Client.Common.Helper.BuildingGid.FARM));
        }

        /// <summary>
        /// Initialize the enemy entities GIDs.
        /// </summary>
        private void InitEnemyEntitiesGid()
        {
            m_enemyEntitiesGid = new Dictionary<EntityType, CCTileGidAndFlags>();

            m_enemyEntitiesGid.Add(EntityType.Hero, new CCTileGidAndFlags(Client.Common.Helper.EnemyUnitGid.HERO));
            m_enemyEntitiesGid.Add(EntityType.Warrior, new CCTileGidAndFlags(Client.Common.Helper.EnemyUnitGid.WARRIOR));
            m_enemyEntitiesGid.Add(EntityType.Mage, new CCTileGidAndFlags(Client.Common.Helper.EnemyUnitGid.MAGE));
            m_enemyEntitiesGid.Add(EntityType.Scout, new CCTileGidAndFlags(Client.Common.Helper.EnemyUnitGid.SCOUT));
            m_enemyEntitiesGid.Add(EntityType.Archer, new CCTileGidAndFlags(Client.Common.Helper.EnemyUnitGid.BOWMAN));
            // m_EnemyEntitiesGid.Add(EntityType.Unknown3, new CCTileGidAndFlags(ClientConstants.ENEMYUNKNOWN_GID));
            m_enemyEntitiesGid.Add(EntityType.Headquarter, new CCTileGidAndFlags(Client.Common.Helper.EnemyBuildingGid.HEADQUARTER));
            m_enemyEntitiesGid.Add(EntityType.Outposts, new CCTileGidAndFlags(Client.Common.Helper.EnemyBuildingGid.HEADQUARTER));
            // m_EnemyEntitiesGid.Add(EntityType.Houses, new CCTileGidAndFlags(ClientConstants.ENEMYHOUSE_GID));
            // m_EnemyEntitiesGid.Add(EntityType.Wall, new CCTileGidAndFlags(ClientConstants.ENEMYWALL1_GID));
            // m_EnemyEntitiesGid.Add(EntityType.Barracks, new CCTileGidAndFlags(ClientConstants.ENEMYGARNISION_GID));
            // m_EnemyEntitiesGid.Add(EntityType.RessourceHarvester, new CCTileGidAndFlags(ClientConstants.ENEMYFARM_GID));
        }

        /// <summary>
        /// Initialize the menu entities GIDs.
        /// </summary>
        private void InitMenuEntitiesGid()
        {
            m_menuEntitiesGid = new Dictionary<EntityType, CCTileGidAndFlags>();

            m_menuEntitiesGid.Add(EntityType.Hero, new CCTileGidAndFlags(Client.Common.Helper.UnitMenuGid.HERO));
            m_menuEntitiesGid.Add(EntityType.Warrior, new CCTileGidAndFlags(Client.Common.Helper.UnitMenuGid.WARRIOR));
            m_menuEntitiesGid.Add(EntityType.Mage, new CCTileGidAndFlags(Client.Common.Helper.UnitMenuGid.MAGE));
            m_menuEntitiesGid.Add(EntityType.Scout, new CCTileGidAndFlags(Client.Common.Helper.UnitMenuGid.SCOUT));
            m_menuEntitiesGid.Add(EntityType.Archer, new CCTileGidAndFlags(Client.Common.Helper.UnitMenuGid.BOWMAN));
            m_menuEntitiesGid.Add(EntityType.Unknown3, new CCTileGidAndFlags(Client.Common.Helper.UnitMenuGid.UNKNOWN));

            m_menuEntitiesGid.Add(EntityType.Headquarter, new CCTileGidAndFlags(Client.Common.Helper.BuildingMenuGid.HEADQUARTER));
            m_menuEntitiesGid.Add(EntityType.Outposts, new CCTileGidAndFlags(Client.Common.Helper.BuildingMenuGid.HEADQUARTER));
            m_menuEntitiesGid.Add(EntityType.Houses, new CCTileGidAndFlags(Client.Common.Helper.BuildingMenuGid.FIRE));
            m_menuEntitiesGid.Add(EntityType.Wall, new CCTileGidAndFlags(Client.Common.Helper.BuildingMenuGid.EARTH));
            m_menuEntitiesGid.Add(EntityType.Barracks, new CCTileGidAndFlags(Client.Common.Helper.BuildingMenuGid.GOLD));
            m_menuEntitiesGid.Add(EntityType.RessourceHarvester, new CCTileGidAndFlags(Client.Common.Helper.BuildingMenuGid.AIR));
        }

        #region Fields

        /// <summary>
        /// The m_entities GID.
        /// </summary>
        private Dictionary<EntityType, CCTileGidAndFlags> m_entitiesGid;

        /// <summary>
        /// The m_enemy entities GID.
        /// </summary>
        private Dictionary<EntityType, CCTileGidAndFlags> m_enemyEntitiesGid;

        /// <summary>
        /// The m_menu entities GID.
        /// </summary>
        private Dictionary<EntityType, CCTileGidAndFlags> m_menuEntitiesGid;

        /// <summary>
        /// The m_terrains GID.
        /// </summary>
        private Dictionary<EntityType, CCTileGidAndFlags> m_terrainsGid;

        #endregion
    }
}

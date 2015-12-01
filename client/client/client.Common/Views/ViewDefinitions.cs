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
            gid = new CCTileGidAndFlags(Common.Constants.TerrainGid.INVALID);

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

            m_terrainsGid.Add(EntityType.Beach, new CCTileGidAndFlags(Common.Constants.TerrainGid.BEACH));
            m_terrainsGid.Add(EntityType.Buildings, new CCTileGidAndFlags(Common.Constants.TerrainGid.BUILDINGS));
            m_terrainsGid.Add(EntityType.Fields, new CCTileGidAndFlags(Common.Constants.TerrainGid.FIELDS));
            m_terrainsGid.Add(EntityType.Forbidden, new CCTileGidAndFlags(Common.Constants.TerrainGid.FORBIDDEN));
            m_terrainsGid.Add(EntityType.Glacier, new CCTileGidAndFlags(Common.Constants.TerrainGid.GLACIER));
            m_terrainsGid.Add(EntityType.Grassland, new CCTileGidAndFlags(Common.Constants.TerrainGid.GRASSLAND));
            m_terrainsGid.Add(EntityType.Invalid, new CCTileGidAndFlags(Common.Constants.TerrainGid.INVALID));
            m_terrainsGid.Add(EntityType.NotDefined, new CCTileGidAndFlags(Common.Constants.TerrainGid.NOTDEFINED));
            m_terrainsGid.Add(EntityType.Park, new CCTileGidAndFlags(Common.Constants.TerrainGid.PARK));
            m_terrainsGid.Add(EntityType.Streets, new CCTileGidAndFlags(Common.Constants.TerrainGid.STREETS));
            m_terrainsGid.Add(EntityType.Town, new CCTileGidAndFlags(Common.Constants.TerrainGid.TOWN));
            m_terrainsGid.Add(EntityType.Water, new CCTileGidAndFlags(Common.Constants.TerrainGid.WATER));
            m_terrainsGid.Add(EntityType.Woods, new CCTileGidAndFlags(Common.Constants.TerrainGid.WOODS));
        }

        /// <summary>
        /// Initialize the entities GIDs.
        /// </summary>
        private void InitEntitiesGid()
        {
            m_entitiesGid = new Dictionary<EntityType, CCTileGidAndFlags>();

            //units
            m_entitiesGid.Add(EntityType.Hero, new CCTileGidAndFlags(Common.Constants.UnitGid.HERO));
            m_entitiesGid.Add(EntityType.Warrior, new CCTileGidAndFlags(Common.Constants.UnitGid.WARRIOR));
            m_entitiesGid.Add(EntityType.Mage, new CCTileGidAndFlags(Common.Constants.UnitGid.MAGE));
            m_entitiesGid.Add(EntityType.Scout, new CCTileGidAndFlags(Common.Constants.UnitGid.SCOUT));
            m_entitiesGid.Add(EntityType.Archer, new CCTileGidAndFlags(Common.Constants.UnitGid.BOWMAN));
            m_entitiesGid.Add(EntityType.Unknown3, new CCTileGidAndFlags(Common.Constants.UnitGid.UNKNOWN));

            //build -> Military
            m_entitiesGid.Add(EntityType.Headquarter, new CCTileGidAndFlags(Common.Constants.BuildingGid.HEADQUARTER));
            m_entitiesGid.Add(EntityType.Attachment, new CCTileGidAndFlags(Common.Constants.BuildingMenuGid.WALL));
            m_entitiesGid.Add(EntityType.GuardTower, new CCTileGidAndFlags(Common.Constants.BuildingMenuGid.TOWER));
            m_entitiesGid.Add(EntityType.Barracks, new CCTileGidAndFlags(Common.Constants.BuildingMenuGid.BARRACKS));
            m_entitiesGid.Add(EntityType.Factory, new CCTileGidAndFlags(Common.Constants.BuildingMenuGid.FABRIK));
        }

        /// <summary>
        /// Initialize the enemy entities GIDs.
        /// </summary>
        private void InitEnemyEntitiesGid()
        {
            m_enemyEntitiesGid = new Dictionary<EntityType, CCTileGidAndFlags>();

            m_enemyEntitiesGid.Add(EntityType.Hero, new CCTileGidAndFlags(Common.Constants.EnemyUnitGid.HERO));
            m_enemyEntitiesGid.Add(EntityType.Warrior, new CCTileGidAndFlags(Common.Constants.EnemyUnitGid.WARRIOR));
            m_enemyEntitiesGid.Add(EntityType.Mage, new CCTileGidAndFlags(Common.Constants.EnemyUnitGid.MAGE));
            m_enemyEntitiesGid.Add(EntityType.Scout, new CCTileGidAndFlags(Common.Constants.EnemyUnitGid.SCOUT));
            m_enemyEntitiesGid.Add(EntityType.Archer, new CCTileGidAndFlags(Common.Constants.EnemyUnitGid.BOWMAN));

            // m_EnemyEntitiesGid.Add(EntityType.Unknown3, new CCTileGidAndFlags(ClientConstants.ENEMYUNKNOWN_GID));
            m_enemyEntitiesGid.Add(EntityType.Headquarter, new CCTileGidAndFlags(Common.Constants.EnemyBuildingGid.HEADQUARTER));
            //m_enemyEntitiesGid.Add(EntityType.Outposts, new CCTileGidAndFlags(Common.Constants.EnemyBuildingGid.HEADQUARTER));
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

            //units -> Melee
            m_menuEntitiesGid.Add(EntityType.Hero, new CCTileGidAndFlags(Common.Constants.UnitMenuGid.HERO));
            m_menuEntitiesGid.Add(EntityType.Warrior, new CCTileGidAndFlags(Common.Constants.UnitMenuGid.WARRIOR));
            m_menuEntitiesGid.Add(EntityType.Mage, new CCTileGidAndFlags(Common.Constants.UnitMenuGid.MAGE));
            m_menuEntitiesGid.Add(EntityType.Scout, new CCTileGidAndFlags(Common.Constants.UnitMenuGid.SCOUT));
            m_menuEntitiesGid.Add(EntityType.Unknown3, new CCTileGidAndFlags(Common.Constants.UnitMenuGid.UNKNOWN));

            //units -> Range
            m_menuEntitiesGid.Add(EntityType.Archer, new CCTileGidAndFlags(Common.Constants.UnitMenuGid.BOWMAN));

            //Buildings -> Militaer
            m_menuEntitiesGid.Add(EntityType.Headquarter, new CCTileGidAndFlags(Common.Constants.BuildingMenuGid.HEADQUARTER));
            m_menuEntitiesGid.Add(EntityType.Attachment, new CCTileGidAndFlags(Common.Constants.BuildingMenuGid.WALL));
            m_menuEntitiesGid.Add(EntityType.GuardTower, new CCTileGidAndFlags(Common.Constants.BuildingMenuGid.TOWER));
            m_menuEntitiesGid.Add(EntityType.Barracks, new CCTileGidAndFlags(Common.Constants.BuildingMenuGid.BARRACKS));
            m_menuEntitiesGid.Add(EntityType.Factory, new CCTileGidAndFlags(Common.Constants.BuildingMenuGid.FABRIK));

            //Buildings -> Zivil
            //m_menuEntitiesGid.Add(EntityType.Houses, new CCTileGidAndFlags(Common.Constants.BuildingMenuGid.FIRE));

            //Buildings -> Storage


            //Buildings -> Resourcen
            //m_menuEntitiesGid.Add(EntityType.RessourceHarvester, new CCTileGidAndFlags(Common.Constants.BuildingMenuGid.AIR));
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

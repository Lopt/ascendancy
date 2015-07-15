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
        public enum Sort
        {
            Normal,
            Enemy,
            Menu,
        }


        private static readonly Lazy<ViewDefinitions> m_singleton =
            new Lazy<ViewDefinitions>(() => new ViewDefinitions());

        public static ViewDefinitions Instance
        {
            get
            {
                return m_singleton.Value;
            }
        }

        private ViewDefinitions()
        {
            InitTerrainsGid();
            InitEntitiesGid();
            InitMenuEntitiesGid();
            InitEnemyEntitiesGid();
        }


        private void InitTerrainsGid()
        {
            m_TerrainsGid = new Dictionary<EntityType,  CCTileGidAndFlags>();

            m_TerrainsGid.Add(EntityType.Beach, new CCTileGidAndFlags(ClientConstants.BEACH_GID));
            m_TerrainsGid.Add(EntityType.Buildings, new CCTileGidAndFlags(ClientConstants.BUILDINGS_GID));
            m_TerrainsGid.Add(EntityType.Fields, new CCTileGidAndFlags(ClientConstants.FIELDS_GID));
            m_TerrainsGid.Add(EntityType.Forbidden, new CCTileGidAndFlags(ClientConstants.FORBIDDEN_GID));
            m_TerrainsGid.Add(EntityType.Glacier, new CCTileGidAndFlags(ClientConstants.GLACIER_GID));
            m_TerrainsGid.Add(EntityType.Grassland, new CCTileGidAndFlags(ClientConstants.GRASSLAND_GID));
            m_TerrainsGid.Add(EntityType.Invalid, new CCTileGidAndFlags(ClientConstants.INVALID_GID));
            m_TerrainsGid.Add(EntityType.NotDefined, new CCTileGidAndFlags(ClientConstants.NOTDEFINED_GID));
            m_TerrainsGid.Add(EntityType.Park, new CCTileGidAndFlags(ClientConstants.PARK_GID));
            m_TerrainsGid.Add(EntityType.Streets, new CCTileGidAndFlags(ClientConstants.STREETS_GID));
            m_TerrainsGid.Add(EntityType.Town, new CCTileGidAndFlags(ClientConstants.TOWN_GID));
            m_TerrainsGid.Add(EntityType.Water, new CCTileGidAndFlags(ClientConstants.WATER_GID));
            m_TerrainsGid.Add(EntityType.Woods, new CCTileGidAndFlags(ClientConstants.WOODS_GID));
        }


        private void InitEntitiesGid()
        {
            m_EntitiesGid = new Dictionary<EntityType, CCTileGidAndFlags>();

            m_EntitiesGid.Add(EntityType.Hero, new CCTileGidAndFlags(ClientConstants.HERO_GID));
            m_EntitiesGid.Add(EntityType.Warrior, new CCTileGidAndFlags(ClientConstants.WARRIOR_GID));
            m_EntitiesGid.Add(EntityType.Mage, new CCTileGidAndFlags(ClientConstants.MAGE_GID));
            m_EntitiesGid.Add(EntityType.Scout, new CCTileGidAndFlags(ClientConstants.SCOUT_GID));
            m_EntitiesGid.Add(EntityType.Archer, new CCTileGidAndFlags(ClientConstants.BOWMAN_GID));
            m_EntitiesGid.Add(EntityType.Unknown3, new CCTileGidAndFlags(ClientConstants.UNKNOWN_GID));
            m_EntitiesGid.Add(EntityType.Headquarter, new CCTileGidAndFlags(ClientConstants.HEADQUARTER_GID));
            m_EntitiesGid.Add(EntityType.Outposts, new CCTileGidAndFlags(ClientConstants.HEADQUARTER_GID));
            m_EntitiesGid.Add(EntityType.Houses, new CCTileGidAndFlags(ClientConstants.HOUSE_GID));
            m_EntitiesGid.Add(EntityType.Wall, new CCTileGidAndFlags(ClientConstants.WALL1_GID));
            m_EntitiesGid.Add(EntityType.Barracks, new CCTileGidAndFlags(ClientConstants.GARNISION_GID));
            m_EntitiesGid.Add(EntityType.RessourceHarvester, new CCTileGidAndFlags(ClientConstants.FARM_GID));

        }


        private void InitEnemyEntitiesGid()
        {
            m_EnemyEntitiesGid = new Dictionary<EntityType, CCTileGidAndFlags>();

            m_EnemyEntitiesGid.Add(EntityType.Hero, new CCTileGidAndFlags(ClientConstants.ENEMYHERO_GID));
            m_EnemyEntitiesGid.Add(EntityType.Warrior, new CCTileGidAndFlags(ClientConstants.ENEMYWARRIOR_GID));
            m_EnemyEntitiesGid.Add(EntityType.Mage, new CCTileGidAndFlags(ClientConstants.ENEMYMAGE_GID));
            m_EnemyEntitiesGid.Add(EntityType.Scout, new CCTileGidAndFlags(ClientConstants.ENEMYSCOUT_GID));
            m_EnemyEntitiesGid.Add(EntityType.Archer, new CCTileGidAndFlags(ClientConstants.ENEMYBOWMAN_GID));
            //m_EnemyEntitiesGid.Add(EntityType.Unknown3, new CCTileGidAndFlags(ClientConstants.ENEMYUNKNOWN_GID));
            m_EnemyEntitiesGid.Add(EntityType.Headquarter, new CCTileGidAndFlags(ClientConstants.ENEMYHEADQUARTER_GID));
            m_EnemyEntitiesGid.Add(EntityType.Outposts, new CCTileGidAndFlags(ClientConstants.ENEMYHEADQUARTER_GID));
            //            m_EnemyEntitiesGid.Add(EntityType.Houses, new CCTileGidAndFlags(ClientConstants.ENEMYHOUSE_GID));
            //m_EnemyEntitiesGid.Add(EntityType.Wall, new CCTileGidAndFlags(ClientConstants.ENEMYWALL1_GID));
            //m_EnemyEntitiesGid.Add(EntityType.Barracks, new CCTileGidAndFlags(ClientConstants.ENEMYGARNISION_GID));
            //m_EnemyEntitiesGid.Add(EntityType.RessourceHarvester, new CCTileGidAndFlags(ClientConstants.ENEMYFARM_GID));
        }




        private void InitMenuEntitiesGid()
        {
            m_MenuEntitiesGid = new Dictionary<EntityType, CCTileGidAndFlags>();

            m_MenuEntitiesGid.Add(EntityType.Hero, new CCTileGidAndFlags(ClientConstants.MENUEHERO_GID));
            m_MenuEntitiesGid.Add(EntityType.Warrior, new CCTileGidAndFlags(ClientConstants.MENUEWARRIOR_GID));
            m_MenuEntitiesGid.Add(EntityType.Mage, new CCTileGidAndFlags(ClientConstants.MENUEMAGE_GID));
            m_MenuEntitiesGid.Add(EntityType.Scout, new CCTileGidAndFlags(ClientConstants.MENUESCOUT_GID));
            m_MenuEntitiesGid.Add(EntityType.Archer, new CCTileGidAndFlags(ClientConstants.MENUEBOWMAN_GID));
            m_MenuEntitiesGid.Add(EntityType.Unknown3, new CCTileGidAndFlags(ClientConstants.MENUEUNKNOWN_GID));
            m_MenuEntitiesGid.Add(EntityType.Headquarter, new CCTileGidAndFlags(ClientConstants.MENUEGOLD_GID));
            m_MenuEntitiesGid.Add(EntityType.Outposts, new CCTileGidAndFlags(ClientConstants.MENUEFIRE_GID));
            m_MenuEntitiesGid.Add(EntityType.Houses, new CCTileGidAndFlags(ClientConstants.MENUEGOLD_GID));
            m_MenuEntitiesGid.Add(EntityType.Wall, new CCTileGidAndFlags(ClientConstants.MENUEAIR_GID));
            m_MenuEntitiesGid.Add(EntityType.Barracks, new CCTileGidAndFlags(ClientConstants.MENUEMANA_GID));
            m_MenuEntitiesGid.Add(EntityType.RessourceHarvester, new CCTileGidAndFlags(ClientConstants.MENUEWATER_GID));

        }



        public CCTileGidAndFlags DefinitionToTileGid(Definition definition, Sort sort=Sort.Normal)
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

        private Dictionary<EntityType, CCTileGidAndFlags> m_EntitiesGid;
        private Dictionary<EntityType, CCTileGidAndFlags> m_EnemyEntitiesGid;
        private Dictionary<EntityType, CCTileGidAndFlags> m_MenuEntitiesGid;
        private Dictionary<EntityType, CCTileGidAndFlags> m_TerrainsGid;

        #endregion
    }
}


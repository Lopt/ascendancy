using System;
using @base.model;
using client.Common.Helper;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using @base.control;
using CocosSharp;
using @base.model.definitions;
using System.Linq.Expressions;
using System.Collections.Generic;

namespace client.Common.Controllers
{
    public class TerrainController : TerrainManagerController
    {

        public TerrainController ()
        {
            m_Network = NetworkController.GetInstance;
            InitTerrainDefToTileGid ();

            TerrainDefinitionCount = 0;
        }

        #region Terrain

        public async Task LoadTerrainDefinitionsAsync ()
        {
            await m_Network.LoadTerrainTypesAsync (ClientConstants.TERRAIN_TYPES_SERVER_PATH);

            var json = m_Network.JsonTerrainTypeString;
            var terrainDefintions = JsonConvert.DeserializeObject<ObservableCollection<@base.model.definitions.TerrainDefinition>> (json);

            foreach (var terrain in terrainDefintions) {
                TerrainManager.AddTerrainDefinition (terrain);
                TerrainDefinitionCount++;
            }
        }

        public CCTileGidAndFlags TerrainDefToTileGid (TerrainDefinition terraindefinition)
        {
            CCTileGidAndFlags gid;

            if (!m_TerrainDefToTileGid.TryGetValue (terraindefinition.TerrainType, out gid))
                gid = new CCTileGidAndFlags (ClientConstants.INVALID_GID);
            
            return gid;
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

        #endregion

        #region public Properties

        public int TerrainDefinitionCount {
            get; 
            private set; 
        }

        #endregion

        #region private Fields

        private NetworkController m_Network;

        private Dictionary<TerrainDefinition.TerrainDefinitionType, CCTileGidAndFlags> m_TerrainDefToTileGid;

        #endregion
    }
}


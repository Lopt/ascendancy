using @base.control;
using @base.model;
using client.Common.Controllers;
using client.Common.Helper;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System;


namespace client.Common.Manager
{
    public class EntityManagerController : DefinitionManagerController
    {

        public EntityManagerController ()
        {
            m_Network = NetworkController.GetInstance;
            m_RegionManagerController = Controller.Instance.RegionManagerController as client.Common.Manager.RegionManagerController;
        }


        public async Task LoadTerrainDefinitionsAsync ()
        {
            await m_Network.LoadTerrainTypesAsync (ClientConstants.TERRAIN_TYPES_SERVER_PATH);

            var json = m_Network.JsonTerrainTypeString;
            var terrainDefintions = JsonConvert.DeserializeObject<ObservableCollection<@base.model.definitions.TerrainDefinition>> (json);

            foreach (var terrain in terrainDefintions) {
                DefinitionManager.AddDefinition (terrain);

            }
        }

        public async Task LoadEntityDefinitionsAsync ()
        {
            await m_Network.LoadTerrainTypesAsync (ClientConstants.ENTITY_TYPES_SERVER_PATH);

            var json = m_Network.JsonTerrainTypeString;
            var unitDefintions = JsonConvert.DeserializeObject<ObservableCollection<@base.model.definitions.UnitDefinition>> (json);

            foreach (var unitType in unitDefintions ) {
                DefinitionManager.AddDefinition (unitType);

            }
        }


        #region Entities

        public async Task LoadEntitiesAsync (Position currentGamePosition, RegionPosition centerRegionPosition)
        {
            var worldRegions = m_RegionManagerController.GetWorldNearRegionPositions (centerRegionPosition);
            var listRegions = new RegionPosition[25];
            int index = 0;
            foreach (var regionPosition in worldRegions) {
                listRegions [index] = regionPosition;
                ++index;
            }

            await LoadEntitiesAsync (currentGamePosition, listRegions);

        }

       
            
        public async Task LoadEntitiesAsync (Position currentGamePosition, RegionPosition[] listRegions)
        {

            var entities = await m_Network.LoadEntitiesAsync (currentGamePosition, listRegions);

            if (entities != null) {
                foreach (var regionEntities in entities) {
                    foreach (var entity in regionEntities) {
                        var region = m_RegionManagerController.GetRegion (entity.Position.RegionPosition);
                        region.AddEntity (DateTime.Now, entity);
                    }
                }
            }
        }

        #endregion


        #region private Fields

        private NetworkController m_Network;
        private RegionManagerController m_RegionManagerController;

        #endregion
    }
}


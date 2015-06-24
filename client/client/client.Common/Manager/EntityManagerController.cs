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
    public class EntityManagerController
    {

        #region Singelton

        private static readonly EntityManagerController m_instance = new EntityManagerController ();

        private EntityManagerController ()
        {
            m_Network = NetworkController.GetInstance;
            m_RegionManagerController = Controller.Instance.RegionStatesController.Curr as RegionManagerController;
            m_DefinitionManagerController = Controller.Instance.DefinitionManagerController as DefinitionManagerController;
        }

        public static EntityManagerController GetInstance {
            get {
                return m_instance; 
            }
        }

        #endregion

        public async Task LoadTerrainDefinitionsAsync ()
        {
            await m_Network.LoadTerrainTypesAsync (ClientConstants.TERRAIN_TYPES_SERVER_PATH);

            var json = m_Network.JsonTerrainTypeString;
            var terrainDefintions = JsonConvert.DeserializeObject<ObservableCollection<@base.model.definitions.TerrainDefinition>> (json);

            foreach (var terrain in terrainDefintions) {
                m_DefinitionManagerController.DefinitionManager.AddDefinition (terrain);

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
                        var region = Controller.Instance.RegionStatesController.Curr.GetRegion (entity.Position.RegionPosition);
                        region.AddEntity (DateTime.Now, entity);
                    }
                }
            }
        }

        #endregion


        #region private Fields

        private NetworkController m_Network;
        private RegionManagerController m_RegionManagerController;
        private DefinitionManagerController m_DefinitionManagerController;

        #endregion
    }
}


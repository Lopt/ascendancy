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
    public class DefinitionManagerController : @base.control.DefinitionManagerController
    {

        public DefinitionManagerController ()
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




        #region private Fields

        private NetworkController m_Network;
        private RegionManagerController m_RegionManagerController;

        #endregion
    }
}


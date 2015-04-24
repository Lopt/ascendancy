using System;
using client.Common.Controllers;
using @base.model;
using client.Common.helper;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using @base.control;

namespace client.Common.controller
{
	public class TerrainController : TerrainManagerController
	{

		public TerrainController ()
		{
			_network = NetworkController.GetInstance;
			TerrainDefinitionCount = 0;
			LoadTerrainDefinitionsAsync ();
		}

		#region Terrain

		private async Task LoadTerrainDefinitionsAsync ()
		{
			await _network.LoadTerrainTypesAsync (ClientConstants.TERRAIN_TYPES_SERVER_PATH);
			var json = _network.JsonTerrainTypeString;
			var terrainDefintions = JsonConvert.DeserializeObject<ObservableCollection<@base.model.definitions.TerrainDefinition>> (json);

			foreach (var terrain in terrainDefintions) {
				TerrainManager.AddTerrainDefinition (terrain);
				TerrainDefinitionCount++;
			}

		}


		#endregion

		#region public Properties

		public int TerrainDefinitionCount{ get; private set; }

		#endregion

		#region private Fields

		private NetworkController _network;

		#endregion
	}
}


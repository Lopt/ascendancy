using System;
using client.Common.Controllers;
using @base.model;
using client.Common.helper;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace client.Common.controller
{
	public sealed class TerrainController
	{
		#region Singelton

		private static readonly TerrainController _instance = new TerrainController ();

		private TerrainController ()
		{
			_network = NetworkController.GetInstance;
			_terrainManager	= World.Instance.TerrainManager;
			TerrainDefinitionCount = 0;
			LoadTerrainDefinitionsAsync ();
		}

		public static TerrainController GetInstance{ get { return _instance; } }

		#endregion

		#region Terrain

		private async Task LoadTerrainDefinitionsAsync ()
		{
			await _network.LoadTerrainTypesAsync (ClientConstants.TERRAIN_TYPES_SERVER_PATH);
			var json = _network.JsonTerrainTypeString;
			var terrainDefintions = JsonConvert.DeserializeObject<ObservableCollection<@base.model.definitions.TerrainDefinition>> (json);

			foreach (var terrain in terrainDefintions) {
				_terrainManager.AddTerrainDefinition (terrain);
				TerrainDefinitionCount++;
			}

		}


		#endregion

		#region public Properties

		public int TerrainDefinitionCount{ get; private set; }

		#endregion

		#region private Fields

		private NetworkController _network;
		private TerrainManager _terrainManager;

		#endregion
	}
}


using System;
using client.Common.Controllers;
using @base.model;
using client.Common.helper;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using @base.control;
using CocosSharp;
using @base.model.definitions;
using System.Linq.Expressions;

namespace client.Common.controller
{
	public class TerrainController : TerrainManagerController
	{

		public TerrainController ()
		{
			_network = NetworkController.GetInstance;
			TerrainDefinitionCount = 0;
			//LoadTerrainDefinitionsAsync ();
		}

		#region Terrain

		public async Task LoadTerrainDefinitionsAsync ()
		{
			// startet prüfen
			await _network.LoadTerrainTypesAsync (ClientConstants.TERRAIN_TYPES_SERVER_PATH);
			var json = _network.JsonTerrainTypeString;
			var terrainDefintions = JsonConvert.DeserializeObject<ObservableCollection<@base.model.definitions.TerrainDefinition>> (json);

			foreach (var terrain in terrainDefintions) {
				TerrainManager.AddTerrainDefinition (terrain);
				TerrainDefinitionCount++;
			}
		}

		public CCTileGidAndFlags TerrainDefToTileGid (TerrainDefinition terraindefinition)
		{
			CCTileGidAndFlags Gid;
			switch (terraindefinition.TerrainType) {
			case TerrainDefinition.TerrainDefinitionType.Beach:
				Gid = new CCTileGidAndFlags (ClientConstants.BEACH_GID);
				break;
			case TerrainDefinition.TerrainDefinitionType.Buildings:
				Gid = new CCTileGidAndFlags (ClientConstants.BUILDINGS_GID);
				break;
			case TerrainDefinition.TerrainDefinitionType.Fields:
				Gid = new CCTileGidAndFlags (ClientConstants.FIELDS_GID);
				break;
			case TerrainDefinition.TerrainDefinitionType.Forbidden:
				Gid = new CCTileGidAndFlags (ClientConstants.FORBIDDEN_GID);
				break;
			case TerrainDefinition.TerrainDefinitionType.Glacier:
				Gid = new CCTileGidAndFlags (ClientConstants.GLACIER_GID);
				break;
			case TerrainDefinition.TerrainDefinitionType.Grassland:
				Gid = new CCTileGidAndFlags (ClientConstants.GRASSLAND_GID);
				break;
			case TerrainDefinition.TerrainDefinitionType.Invalid:
				Gid = new CCTileGidAndFlags (ClientConstants.INVALID_GID);
				break;
			case TerrainDefinition.TerrainDefinitionType.NotDefined:
				Gid = new CCTileGidAndFlags (ClientConstants.NOTDEFINED_GID);
				break;
			case TerrainDefinition.TerrainDefinitionType.Park:
				Gid = new CCTileGidAndFlags (ClientConstants.PARK_GID);
				break;
			case TerrainDefinition.TerrainDefinitionType.Streets:
				Gid = new CCTileGidAndFlags (ClientConstants.STREETS_GID);
				break;
			case TerrainDefinition.TerrainDefinitionType.Town:
				Gid = new CCTileGidAndFlags (ClientConstants.TOWN_GID);
				break;
			case TerrainDefinition.TerrainDefinitionType.Water:
				Gid = new CCTileGidAndFlags (ClientConstants.WATER_GID);
				break;
			case TerrainDefinition.TerrainDefinitionType.Woods:
				Gid = new CCTileGidAndFlags (ClientConstants.WOODS_GID);
				break;
			}
			return Gid;
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


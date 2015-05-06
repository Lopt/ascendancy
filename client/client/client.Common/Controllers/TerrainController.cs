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

		public CCTileGidAndFlags TerrainDefToTileGid (TerrainDefinition terraindefinition)
		{
			switch (terraindefinition.TerrainType) {
			case TerrainDefinition.TerrainDefinitionType.Beach:
				return new CCTileGidAndFlags (ClientConstants.BEACH_GID);
			case TerrainDefinition.TerrainDefinitionType.Buildings:
				return new CCTileGidAndFlags (ClientConstants.BUILDINGS_GID);
			case TerrainDefinition.TerrainDefinitionType.Fields:
				return new CCTileGidAndFlags (ClientConstants.FIELDS_GID);
			case TerrainDefinition.TerrainDefinitionType.Forbidden:
				return new CCTileGidAndFlags (ClientConstants.FORBIDDEN_GID);
			case TerrainDefinition.TerrainDefinitionType.Glacier:
				return new CCTileGidAndFlags (ClientConstants.GLACIER_GID);
			case TerrainDefinition.TerrainDefinitionType.Grassland:
				return new CCTileGidAndFlags (ClientConstants.GRASSLAND_GID);
			case TerrainDefinition.TerrainDefinitionType.Invalid:
				return new CCTileGidAndFlags (ClientConstants.INVALID_GID);
			case TerrainDefinition.TerrainDefinitionType.NotDefined:
				return new CCTileGidAndFlags (ClientConstants.NOTDEFINED_GID);
			case TerrainDefinition.TerrainDefinitionType.Park:
				return new CCTileGidAndFlags (ClientConstants.PARK_GID);
			case TerrainDefinition.TerrainDefinitionType.Streets:
				return new CCTileGidAndFlags (ClientConstants.STREETS_GID);
			case TerrainDefinition.TerrainDefinitionType.Town:
				return new CCTileGidAndFlags (ClientConstants.TOWN_GID);
			case TerrainDefinition.TerrainDefinitionType.Water:
				return new CCTileGidAndFlags (ClientConstants.WATER_GID);
			case TerrainDefinition.TerrainDefinitionType.Woods:
				return new CCTileGidAndFlags (ClientConstants.WOODS_GID);
			}
			;
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


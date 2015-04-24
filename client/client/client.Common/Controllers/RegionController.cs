using System;
using @base.model;
using @base.control;
using client.Common.helper;
using System.Threading.Tasks;
using client.Common.controller;

namespace client.Common.Controllers
{
	public class RegionController : RegionManagerController
	{
		public RegionController ()
		{
			_networkController = NetworkController.GetInstance;
			_geolocation = Geolocation.GetInstance;
			_terrainController = Controller.Instance.TerrainManagerController as TerrainController;
			region = null;

		}

		public Region region{ get; private set; }

		#region Region

		public async Task LoadRegionAsync ()
		{
			var geolocationPosition = _geolocation.CurrentGamePosition;

			await LoadRegionAsync (geolocationPosition);
		}

		public async Task LoadRegionAsync (Position _gameWorldPosition)
		{
			RegionPosition regionPosition	= new RegionPosition (_gameWorldPosition);

			await LoadRegionAsync (regionPosition);
		}

		public async Task LoadRegionAsync (RegionPosition _regionPosition)
		{
			string path = ReplacePath (ClientConstants.REGION_SERVER_PATH, _regionPosition);

			await _networkController.LoadTerrainsAsync (path);
			if (_terrainController.TerrainDefinitionCount > 0)
				region = JsonToRegion (_networkController.JsonTerrainsString, _regionPosition);
		}

		public void AddRegion (Region _region)
		{
			RegionManager.AddRegion (_region);
		}

		public Region GetRegion (Position _gameWorldPosition)
		{
			return GetRegion (new RegionPosition (_gameWorldPosition));
		}

		public Region GetRegion (RegionPosition _regionPosition)
		{
			return RegionManager.GetRegion (_regionPosition);
		}

		#endregion

		#region private Fields

		private NetworkController _networkController;
		private Geolocation _geolocation;
		private TerrainController _terrainController;

		#endregion
	}
}


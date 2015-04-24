using System;
using @base.model;
using @base.control;
using client.Common.helper;
using System.Threading.Tasks;
using client.Common.controller;

namespace client.Common.Controllers
{
	public sealed class RegionController
	{
		#region Singelton

		private static readonly RegionController _instance = new RegionController ();

		private RegionController ()
		{
			_networkController = NetworkController.GetInstance;
			_geolocation = Geolocation.GetInstance;
			_regionManager = World.Instance.RegionManager;
			_terrainController = TerrainController.GetInstance;
			_controlRegionManager	= new @base.control.RegionManager ();
			region = null;


		}

		public static RegionController GetInstance{ get { return _instance; } }

		#endregion

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
			string path = _controlRegionManager.ReplacePath (ClientConstants.REGION_SERVER_PATH, _regionPosition);

			await _networkController.LoadTerrainsAsync (path);
			if (_terrainController.TerrainDefinitionCount > 0)
				region = _controlRegionManager.JsonToRegion (_networkController.JsonTerrainsString, _regionPosition);
		}

		public void AddRegion (Region _region)
		{
			_regionManager.AddRegion (_region);
		}

		public Region GetRegion (Position _gameWorldPosition)
		{
			return GetRegion (new RegionPosition (_gameWorldPosition));
		}

		public Region GetRegion (RegionPosition _regionPosition)
		{
			return _regionManager.GetRegion (_regionPosition);
		}

		#endregion

		#region private Fields

		private NetworkController _networkController;
		private Geolocation _geolocation;
		private @base.model.RegionManager _regionManager;
		private @base.control.RegionManager _controlRegionManager;
		private TerrainController _terrainController;

		#endregion
	}
}


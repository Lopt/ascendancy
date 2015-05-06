using System;
using @base.model;
using @base.control;
using client.Common.helper;
using System.Threading.Tasks;
using client.Common.controller;
using CocosSharp;

namespace client.Common.Controllers
{
	public class RegionController : RegionManagerController
	{
		public RegionController ()
		{
			_networkController = NetworkController.GetInstance;
			_geolocation = Geolocation.GetInstance;
			_terrainController = Controller.Instance.TerrainManagerController as TerrainController;
			_region = null;

		}


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
				_region = JsonToRegion (_networkController.JsonTerrainsString, _regionPosition);

			RegionManager.AddRegion (_region);
		}


		public void SetTilesInMap (CCTileMapLayer mapLayer, CCTileMapCoordinates mapUpperLeftCoordinate)
		{
			for (int x = 0; x < Constants.REGION_SIZE_X; x++) {
				for (int y = 0; y < Constants.REGION_SIZE_Y; y++) {
					SetTileInMap (mapLayer, new CellPosition (_region.RegionPosition.RegionX + x, _region.RegionPosition.RegionY + y)
						, new CCTileMapCoordinates (mapUpperLeftCoordinate.Column + x, mapUpperLeftCoordinate.Row + y));
				}
			}
		}

		public void SetTileInMap (CCTileMapLayer mapLayer, CellPosition cellPosition, CCTileMapCoordinates mapCoordinat)
		{
			var gid = _terrainController.TerrainDefToTileGid (_region.GetTerrain (cellPosition));
			mapLayer.SetTileGID (gid, mapCoordinat);
		}

		#endregion

		#region private Fields

		private NetworkController _networkController;
		private Geolocation _geolocation;
		private TerrainController _terrainController;
		private Region _region;

		#endregion
	}
}


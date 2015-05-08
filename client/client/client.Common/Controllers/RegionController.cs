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
			m_networkController = NetworkController.GetInstance;
			m_geolocation = Geolocation.GetInstance;
			m_terrainController = Controller.Instance.TerrainManagerController as TerrainController;

		}


		#region Region

		public async Task LoadRegionAsync ()
		{
			var geolocationPosition = m_geolocation.CurrentGamePosition;

			await LoadRegionAsync (geolocationPosition);
		}

		public async Task LoadRegionAsync (Position gameWorldPosition)
		{
			RegionPosition regionPosition	= new RegionPosition (gameWorldPosition);

			await LoadRegionAsync (regionPosition);
		}

		public async Task LoadRegionAsync (RegionPosition regionPosition)
		{
			string path = ReplacePath (ClientConstants.REGION_SERVER_PATH, regionPosition);
			Region region = null;

			await m_networkController.LoadTerrainsAsync (path);
			if (m_terrainController.TerrainDefinitionCount > 12)
				region = JsonToRegion (m_networkController.JsonTerrainsString, regionPosition);

			if (region != null)
				RegionManager.AddRegion (region);
		}


		public void SetTilesInMap (CCTileMapLayer mapLayer, CCTileMapCoordinates mapUpperLeftCoordinate, Region region)
		{
			for (int x = 0; x < Constants.REGION_SIZE_X; x++) {
				for (int y = 0; y < Constants.REGION_SIZE_Y; y++) {
					SetTileInMap (mapLayer, new CellPosition (region.RegionPosition.RegionX + x, region.RegionPosition.RegionY + y)
						, MapCellPosToTilePos ((mapUpperLeftCoordinate.Column + x), (mapUpperLeftCoordinate.Row + y)), region);
				}
			}
		}

		public void SetTileInMap (CCTileMapLayer mapLayer, CellPosition cellPosition, CCTileMapCoordinates mapCoordinat, Region region)
		{
			var gid = m_terrainController.TerrainDefToTileGid (region.GetTerrain (cellPosition));
			mapLayer.SetTileGID (gid, mapCoordinat);
		}

						
		public CCTileMapCoordinates MapCellPosToTilePos (int x, int y)
		{
			return new CCTileMapCoordinates (x / 2, (y * 2) + (x % 2));			
		}

		public CellPosition TilePosToMapCellPos (CCTileMapCoordinates tileMapCoordinates)
		{
			var x = tileMapCoordinates.Column;
			var y = tileMapCoordinates.Row;
			return new CellPosition ((x * 2) + (y % 2), y / 2);
		}

		public override Region GetRegion (RegionPosition regionPosition)
		{
			var region = RegionManager.GetRegion (regionPosition);
			if(!region.Exist)
			{
				await LoadRegionAsync (regionPosition);
			}
		}

		#endregion

		#region private Fields

		private NetworkController m_networkController;
		private Geolocation m_geolocation;
		private TerrainController m_terrainController;

		#endregion
	}
}


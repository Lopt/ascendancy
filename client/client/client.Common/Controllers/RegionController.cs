using System;
using @base.model;
using @base.control;
using client.Common.helper;
using System.Threading.Tasks;
using client.Common.controller;
using CocosSharp;
using @base.model.definitions;
using client.Common.Helper;

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


		#region Regions

		public Region GetRegionByGeolocator ()
		{
			var geolocationPosition = m_geolocation.CurrentGamePosition;
			return GetRegionByGamePosition (geolocationPosition);
		}

		public Region GetRegionByGamePosition (Position gameWorldPosition)
		{
			RegionPosition regionPosition	= new RegionPosition (gameWorldPosition);
			return GetRegion (regionPosition);
		}

		public override Region GetRegion (RegionPosition regionPosition)
		{

			var WorldRegions = GetWorldNearRegionPositions (regionPosition);

			foreach (var RegionPosition in WorldRegions) {
				var region = RegionManager.GetRegion (RegionPosition);
				if (!region.Exist) {
					LoadRegionAsync (region);
				}
			}

			return RegionManager.GetRegion (regionPosition);
		}

		private async Task LoadRegionAsync (Region region)
		{
			string path = ReplacePath (ClientConstants.REGION_SERVER_PATH, region.RegionPosition);
			TerrainDefinition[,] terrain = null;

			await m_networkController.LoadTerrainsAsync (path);
			if (m_terrainController.TerrainDefinitionCount > 12)
				terrain = JsonToTerrain (m_networkController.JsonTerrainsString);

			if (terrain != null)
				region.AddTerrain (terrain);
			RegionManager.AddRegion (region);
		}

		#endregion

		#region TileMap

		public void SetTilesINMap160 (CCTileMapLayer mapLayer, Region region)
		{
			var WorldRegionPositions = GetWorldNearRegionPositions (region.RegionPosition);
			int OffsetX = 0;
			int OffsetY = 0;

			for (int x = 0; x < 5; x++) {
				for (int y = 0; y < 5; y++) {
					var Region = GetRegion (WorldRegionPositions [x, y]);
					SetTilesInMap32 (mapLayer, new CCTileMapCoordinates (OffsetX, OffsetY), Region);
					OffsetY += 32;
				}
				OffsetX += 32;
				OffsetY = 0;
			}
		}

		public void SetTilesInMap32 (CCTileMapLayer mapLayer, CCTileMapCoordinates mapUpperLeftCoordinate, Region region)
		{
			for (int x = 0; x < Constants.REGION_SIZE_X; x++) {
				for (int y = 0; y < Constants.REGION_SIZE_Y; y++) {
					SetTileInMap (mapLayer, new CellPosition (region.RegionPosition.RegionX + x, region.RegionPosition.RegionY + y)
						, Modify.MapCellPosToTilePos ((mapUpperLeftCoordinate.Column + x), (mapUpperLeftCoordinate.Row + y)), region);
				}
			}
		}

		public void SetTileInMap (CCTileMapLayer mapLayer, CellPosition cellPosition, CCTileMapCoordinates mapCoordinat, Region region)
		{
			var gid = m_terrainController.TerrainDefToTileGid (region.GetTerrain (cellPosition));
			mapLayer.SetTileGID (gid, mapCoordinat);
		}

		public void SetEntitysInMap (CCTileMapLayer mapLayer, CCTileMapCoordinates mapUpperLeftCoordinate, Region region)
		{
			for (int x = 0; x < Constants.REGION_SIZE_X; x++) {
				for (int y = 0; y < Constants.REGION_SIZE_Y; y++) {
					SetEntityInMap (mapLayer, new CellPosition (region.RegionPosition.RegionX + x, region.RegionPosition.RegionY + y)
						, Modify.MapCellPosToTilePos ((mapUpperLeftCoordinate.Column + x), (mapUpperLeftCoordinate.Row + y)), region);
				}
			}
		}

		public void SetEntityInMap (CCTileMapLayer mapLayer, CellPosition cellPosition, CCTileMapCoordinates mapCoordinat, Region region)
		{
			// TODO build EntityController and EntityDefToEntityGid
			//var gid = m_terrainController.TerrainDefToTileGid (region.GetEntity (cellPosition));
			//mapLayer.SetTileGID (gid, mapCoordinat);

			throw new NotImplementedException ();
		}

		public CCTileMapCoordinates GetCurrentTileInMap (Position position)
		{
			var RegionPosition = new RegionPosition (position);
			var CellPosition = new CellPosition (position);
			var WorldRegions = GetWorldNearRegionPositions (RegionPosition);
			int OffsetX = 0;
			int OffsetY = 0;
			int MapCellX = -1;
			int MapCellY = -1;
			for (int x = 0; x < 5; x++) {
				for (int y = 0; y < 5; y++) {
					if (RegionPosition.Equals (WorldRegions [x, y])) {
						MapCellX = CellPosition.CellX + OffsetX;
						MapCellY = CellPosition.CellY + OffsetY;
					}
					OffsetY += 32;
				}
				OffsetX += 32;
				OffsetY = 0;
			}

			return Modify.MapCellPosToTilePos (MapCellX, MapCellY);
		}

		#endregion


		#region RegionPositions

		public RegionPosition[,] GetWorldNearRegionPositions (RegionPosition regionPosition)
		{
			// TODO to set the true offset must talk with Bernd
			int OffsetX = -2;
			int OffsetY = -2;

			RegionPosition[,] WorldRegions = new RegionPosition[5, 5];
			for (int x = 0; x < 5; x++) {
				for (int y = 0; y < 5; y++) {
					WorldRegions [x, y] = new RegionPosition (regionPosition.RegionX + OffsetX, regionPosition.RegionY + OffsetY);
					OffsetY += 1;
				}
									 
				OffsetX += 1;
				OffsetY = -2;
			}

			return WorldRegions;
		}

		#endregion

		#region private Fields

		private NetworkController m_networkController;
		private Geolocation m_geolocation;
		private TerrainController m_terrainController;

		#endregion
	}
}


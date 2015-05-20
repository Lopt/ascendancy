using System;
using @base.model;
using @base.control;
using client.Common.helper;
using System.Threading.Tasks;
using client.Common.controller;
using CocosSharp;
using @base.model.definitions;
using client.Common.Helper;
using client.Common.Models;

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
			var region = RegionManager.GetRegion (regionPosition);
			if (!region.Exist) {
				LoadRegionAsync (region);
			}
				
			return region;
		}


		private async Task LoadRegionAsync (Region region)
		{
			string path = ReplacePath (ClientConstants.REGION_SERVER_PATH, region.RegionPosition);
			TerrainDefinition[,] terrain = null;

			await m_networkController.LoadTerrainsAsync (path);

			if (GameAppDelegate.m_Loading >= GameAppDelegate.Loading.TerrainTypeLoaded)
				terrain = JsonToTerrain (m_networkController.JsonTerrainsString);

			if (terrain != null)
				region.AddTerrain (terrain);
			RegionManager.AddRegion (region);
		}

		public async Task LoadRegionsAsync ()
		{
			await LoadRegionsAsync (m_geolocation.CurrentRegionPosition);
		}

		public async Task LoadRegionsAsync (RegionPosition regionPosition)
		{
			var WorldRegions = GetWorldNearRegionPositions (regionPosition);

			foreach (var RegionPosition in WorldRegions) {
				var region = RegionManager.GetRegion (RegionPosition);
				if (!region.Exist) {
					await LoadRegionAsync (region);
				}
			}
		}

		#endregion

		#region TileMap

		public void SetTilesINMap160 (CCTileMapLayer mapLayer, Region region)
		{
			var WorldRegionPositions = GetWorldNearRegionPositions (region.RegionPosition);
			int OffsetX = 0;
			int OffsetY = 0;

			for (int y = 0; y < 5; y++) {
				for (int x = 0; x < 5; x++) {
					var Region = GetRegion (WorldRegionPositions [x, y]);
					SetTilesInMap32 (mapLayer, new CCTileMapCoordinates (OffsetX, OffsetY), Region);
					OffsetX += 32;
				}
				OffsetY += 32;
				OffsetX = 0;
			}
		}

		public void SetTilesInMap32 (CCTileMapLayer mapLayer, CCTileMapCoordinates mapUpperLeftCoordinate, Region region)
		{
			for (int y = 0; y < Constants.REGION_SIZE_Y; y++) {
				for (int x = 0; x < Constants.REGION_SIZE_X; x++) {
					var NewCellPosition = new CellPosition (x, y);
					var MapCellPosition = new MapCellPosition ((mapUpperLeftCoordinate.Column + x), (mapUpperLeftCoordinate.Row + y));
					SetTileInMap (mapLayer, NewCellPosition, MapCellPosition.GetTileMapCoordinates (), region);		
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
			//TODO set function
			throw new NotImplementedException ();
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
			var RegionPos = new RegionPosition (position);
			var CellPos = new CellPosition (position);
			var WorldRegions = GetWorldNearRegionPositions (RegionPos);
			int OffsetX = 0;
			int OffsetY = 0;
			int MapCellX = -1;
			int MapCellY = -1;
			for (int x = 0; x < 5; x++) {
				for (int y = 0; y < 5; y++) {
					if (RegionPos.Equals (WorldRegions [x, y])) {
						MapCellX = CellPos.CellX + OffsetX;
						MapCellY = CellPos.CellY + OffsetY;
					}
					OffsetY += 32;
				}
				OffsetX += 32;
				OffsetY = 0;
			}

			var MapCellPosition = new MapCellPosition (MapCellX, MapCellY);
			return MapCellPosition.GetTileMapCoordinates ();
		}

		#endregion


		#region RegionPositions

		public RegionPosition[,] GetWorldNearRegionPositions (RegionPosition regionPosition)
		{
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


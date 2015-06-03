using System;
using @base.model;
using @base.control;
using client.Common.Helper;
using System.Threading.Tasks;
using client.Common.Controllers;
using CocosSharp;
using @base.model.definitions;
using client.Common.Helper;
using client.Common.Models;

namespace client.Common.Controllers
{
    public class RegionController : RegionManagerController
    {
        public RegionController (RegionManager regionManager)
            : base (null, regionManager)
        {
            m_networkController = NetworkController.GetInstance;
            m_geolocation = Geolocation.GetInstance;
            m_terrainController = Controller.Instance.DefinitionManagerController as TerrainController;

        }


        #region Regions

        public Region GetRegionByGeolocator ()
        {
            var geolocationPosition = m_geolocation.CurrentGamePosition;
            return GetRegionByGamePosition (geolocationPosition);
        }

        public Region GetRegionByGamePosition (Position gameWorldPosition)
        {
            RegionPosition regionPosition = new RegionPosition (gameWorldPosition);
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

            if (GameAppDelegate.LoadingState >= GameAppDelegate.Loading.TerrainTypeLoaded)
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
            var worldRegionPositions = GetWorldNearRegionPositions (region.RegionPosition);

            for (int y = 0; y < 5; y++) {
                for (int x = 0; x < 5; x++) {
                    var Region = GetRegion (worldRegionPositions [x, y]);
                    SetTilesInMap32 (mapLayer, new CCTileMapCoordinates (x * Constants.REGION_SIZE_X, y * Constants.REGION_SIZE_Y), Region);
                }
            }
        }

        public void SetTilesInMap32 (CCTileMapLayer mapLayer, CCTileMapCoordinates mapUpperLeftCoordinate, Region region)
        {
            for (int y = 0; y < Constants.REGION_SIZE_Y; y++) {
                for (int x = 0; x < Constants.REGION_SIZE_X; x++) {
                    var newCellPosition = new CellPosition (x, y);
                    var mapCellPosition = new MapCellPosition ((mapUpperLeftCoordinate.Column + x), (mapUpperLeftCoordinate.Row + y));
                    SetTileInMap (mapLayer, newCellPosition, mapCellPosition.GetTileMapCoordinates (), region);		
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
            var regionPos = new RegionPosition (position);
            var cellPos = new CellPosition (position);
            var worldRegions = GetWorldNearRegionPositions (regionPos);

            int mapCellX = -1;
            int mapCellY = -1;

            for (int x = 0; x < 5; x++) {
                for (int y = 0; y < 5; y++) {
                    if (regionPos.Equals (worldRegions [x, y])) {
                        mapCellX = cellPos.CellX + (x * Constants.REGION_SIZE_X);
                        mapCellY = cellPos.CellY + (y * Constants.REGION_SIZE_Y);
                    }

                }

            }

            var MapCellPosition = new MapCellPosition (mapCellX, mapCellY);
            return MapCellPosition.GetTileMapCoordinates ();
        }

        #endregion


        #region RegionPositions

        public RegionPosition[,] GetWorldNearRegionPositions (RegionPosition regionPosition)
        {
            int offsetX = -2;
            int offsetY = -2;

            RegionPosition[,] worldRegion = new RegionPosition[5, 5];
            for (int x = 0; x < 5; x++) {
                for (int y = 0; y < 5; y++) {
                    worldRegion [x, y] = new RegionPosition (regionPosition.RegionX + offsetX, regionPosition.RegionY + offsetY);
                    offsetY += 1;
                }
									 
                offsetX += 1;
                offsetY = -2;
            }

            return worldRegion;
        }

        #endregion

        #region private Fields

        private NetworkController m_networkController;
        private Geolocation m_geolocation;
        private TerrainController m_terrainController;

        #endregion
    }
}


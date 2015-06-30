using System;
using CocosSharp;
using @base.model;
using client.Common.Manager;
using client.Common.Models;
using client.Common.Helper;
using System.Linq.Expressions;

namespace client.Common.Views
{
    public class RegionView
    {
        public RegionView ()
        {
            m_RegionManagerController = @base.control.Controller.Instance.RegionStatesController.Curr as client.Common.Manager.RegionManagerController;
            m_ViewDefinition = new ViewDefinitions ();
        }

        public void SetTilesInMap160 (CCTileMapLayer mapLayer, @base.model.Region region)
        {
            var worldRegionPositions = m_RegionManagerController.GetWorldNearRegionPositions (region.RegionPosition);

            for (int y = 0; y < 5; y++) {
                for (int x = 0; x < 5; x++) {
                    var Region = m_RegionManagerController.GetRegion (worldRegionPositions [x, y]);
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
                    switch (mapLayer.LayerName) {
                    case ClientConstants.LAYER_TERRAIN:
                        SetTerrainTileInMap (mapLayer, newCellPosition, mapCellPosition.GetTileMapCoordinates (), region); 
                        break;
                    case ClientConstants.LAYER_UNIT:
                        SetUnitTileInMap (mapLayer, newCellPosition, mapCellPosition.GetTileMapCoordinates (), region); 
                        break;
                    case ClientConstants.LAYER_BUILDING:
                        SetUnitTileInMap (mapLayer, newCellPosition, mapCellPosition.GetTileMapCoordinates (), region); 
                        break;
                    }

                }
            }
        }

        public void SetTerrainTileInMap (CCTileMapLayer mapLayer, CellPosition cellPosition, CCTileMapCoordinates mapCoordinat, Region region)
        {
            var gid = m_ViewDefinition.DefinitionToTileGid (region.GetTerrain (cellPosition));
            mapLayer.SetTileGID (gid, mapCoordinat);
        }


        public void SetUnitTileInMap (CCTileMapLayer mapLayer, CellPosition cellPosition, CCTileMapCoordinates mapCoordinat, Region region)
        {
            var entity = region.GetEntity (cellPosition);
            if (entity != null) {
                var gid = new CCTileGidAndFlags (60); //m_ViewDefinition.DefinitionToTileGid (entity.Definition);
                mapLayer.SetTileGID (gid, mapCoordinat);
            } else {
                //mapLayer.RemoveTile (mapCoordinat);
            }
        }

        public CCTileMapCoordinates GetCurrentTileInMap (Position position)
        {
            var regionPos = new RegionPosition (position);
            var cellPos = new CellPosition (position);
            var worldRegions = m_RegionManagerController.GetWorldNearRegionPositions (regionPos);

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


        public Position GetCurrentGamePosition (MapCellPosition mapCellPosition, RegionPosition currentCenterRegion)
        {
            var worldRegions = m_RegionManagerController.GetWorldNearRegionPositions (currentCenterRegion);

            var cellX = mapCellPosition.CellX % Constants.REGION_SIZE_X;
            var cellY = mapCellPosition.CellY % Constants.REGION_SIZE_Y;
            CellPosition cellPosition = new CellPosition (cellX, cellY);

            var x = mapCellPosition.CellX / Constants.REGION_SIZE_X;
            var y = mapCellPosition.CellY / Constants.REGION_SIZE_Y;
            var regionPosition = worldRegions [x, y];

            return new Position (regionPosition, cellPosition);

        }

        public bool IsCellInOutsideRegion (MapCellPosition mapCellPosition)
        {
            if (mapCellPosition.CellX < Constants.REGION_SIZE_X || mapCellPosition.CellX > Constants.REGION_SIZE_X * 4)
                return true;
            if (mapCellPosition.CellY < Constants.REGION_SIZE_Y || mapCellPosition.CellY > Constants.REGION_SIZE_Y * 4)
                return true;

            return false;
        }

        #region Fields

        client.Common.Manager.RegionManagerController m_RegionManagerController;
        ViewDefinitions m_ViewDefinition;

        #endregion
    }
}


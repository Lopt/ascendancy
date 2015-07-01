using System;
using CocosSharp;
using @base.model;
using client.Common.Manager;
using client.Common.Models;
using client.Common.Helper;
using System.Linq.Expressions;
using System.Threading;

namespace client.Common.Views
{
    public class RegionView
    {
        public RegionView ()
        {
            m_RegionManagerController = @base.control.Controller.Instance.RegionManagerController as client.Common.Manager.RegionManagerController;
            m_ViewDefinition = new ViewDefinitions ();
        }

        public void SetTilesInMap160 (@base.model.Region region)
        {

            var worldRegionPositions = m_RegionManagerController.GetWorldNearRegionPositions (region.RegionPosition);

            for (int y = 0; y < 5; y++) {
                for (int x = 0; x < 5; x++) {
                    var Region = m_RegionManagerController.GetRegion (worldRegionPositions [x, y]);
                    SetTilesInMap32 (new CCTileMapCoordinates (x * Constants.REGION_SIZE_X, y * Constants.REGION_SIZE_Y), Region);
                }
            }
        }

        public void SetTilesInMap32 (CCTileMapCoordinates mapUpperLeftCoordinate, Region region)
        {
            for (int y = 0; y < Constants.REGION_SIZE_Y; y++)
            {
                for (int x = 0; x < Constants.REGION_SIZE_X; x++)
                {
                    var newCellPosition = new CellPosition (x, y);
                    var mapCellPosition = new MapCellPosition ((mapUpperLeftCoordinate.Column + x), (mapUpperLeftCoordinate.Row + y));

                    SetTerrainTileInMap (newCellPosition, mapCellPosition.GetTileMapCoordinates (), region); 
                    SetUnitTileInMap (newCellPosition, mapCellPosition.GetTileMapCoordinates (), region); 
                    //SetBuildingTileInMap (mapLayer, newCellPosition, mapCellPosition.GetTileMapCoordinates (), region); 
                
                }
            }
        }

        public void SetTerrainTileInMap (CellPosition cellPosition, CCTileMapCoordinates mapCoordinat, Region region)
        {
            var gid = m_ViewDefinition.DefinitionToTileGid (region.GetTerrain (cellPosition));
            TerrainLayer.SetTileGID (gid, mapCoordinat);
        }

        public void SetUnit(CCTileMapCoordinates mapCoordinat, Entity unit)
        {
            if (unit == null)
            {
                UnitLayer.RemoveTile (mapCoordinat);
            }
            else
            {
                var gid = m_ViewDefinition.DefinitionToTileGid (unit.Definition);
                UnitLayer.SetTileGID (gid, mapCoordinat);
            }
        }

        public void SetUnitTileInMap (CCTileMapLayer mapLayer, CellPosition cellPosition, CCTileMapCoordinates mapCoordinat, Region region)
        {   
            var entity = region.GetEntity (cellPosition);
            if (entity != null) {
                var gid = m_ViewDefinition.DefinitionToTileGid (entity.Definition);
                mapLayer.SetTileGID (gid, mapCoordinat);
            }
            else
            {
                mapLayer.RemoveTile (mapCoordinat);
            }
        }

        public void SetUnitTileInMap (CellPosition cellPosition, CCTileMapCoordinates mapCoordinat, Region region)
        {   
            var entity = region.GetEntity (cellPosition);
            SetUnit (mapCoordinat, entity);
        }

        public void SetBuildingTileInMap (CellPosition cellPosition, CCTileMapCoordinates mapCoordinat, Region region)
        {
            //var gid = m_ViewDefinition.DefinitionToTileGid (region.GetEntity (cellPosition).Definition);
            //m_buildingLayer.SetTileGID (gid, mapCoordinat);
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
            if (mapCellPosition.CellX < (Constants.REGION_SIZE_X * 1.5f) || mapCellPosition.CellX > (Constants.REGION_SIZE_X * 3.5f))
                return true;
            if (mapCellPosition.CellY < (Constants.REGION_SIZE_Y * 1.5f) || mapCellPosition.CellY > (Constants.REGION_SIZE_Y * 3.5f))
                return true;

            return false;
        }

        /*
        public CCTileMapCoordinates PositionIToMapCoordinates(PositionI position)
        {
            //var new new PositionI (currentRegionPositon, new CellPosition (0, 0));
        }

        public void SetUnit(PositionI position, Entity unit = null)
        {
            var mapCoordinat = PositionIToMapCoordinates (position);
            SetUnit(position, unit);
        }

        public void SetUnit(PositionI position, CCTileMapCoordinates mapCoordinat, Entity entity = null)
        {
            if (unit == null)
            {
                m_unitLayer.RemoveTile (mapCoordinat);
            }
            else
            {
                var gid = m_ViewDefinition.DefinitionToTileGid (entity.Definition);
                m_unitLayer.SetTileGID (gid, mapCoordinat);
            }
        }
*/
        #region Fields

        client.Common.Manager.RegionManagerController m_RegionManagerController;
        ViewDefinitions m_ViewDefinition;

        public CCTileMapLayer TerrainLayer;
        public CCTileMapLayer BuildingLayer;
        public CCTileMapLayer UnitLayer;
        public CCTileMapLayer MenuLayer;


        public WorldLayer WorldLayer
        {
            private set;
            get;
        }


        #endregion
    }
}


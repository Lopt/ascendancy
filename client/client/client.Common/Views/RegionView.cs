﻿using System;
using CocosSharp;
using Core.Models;
using client.Common.Manager;
using client.Common.Models;
using client.Common.Helper;
using System.Linq.Expressions;
using System.Threading;

namespace client.Common.Views
{
    public class RegionView
    {
        public RegionView()
        {
            m_RegionManagerController = Core.Controllers.Controller.Instance.RegionManagerController as client.Common.Manager.RegionManagerController;
        }

        public void SetTilesInMap160(Core.Models.Region region)
        {

            var worldRegionPositions = m_RegionManagerController.GetWorldNearRegionPositions(region.RegionPosition);

            for (int y = 0; y < ClientConstants.DRAW_REGIONS_X; y++)
            {
                for (int x = 0; x < ClientConstants.DRAW_REGIONS_Y; x++)
                {
                    var Region = m_RegionManagerController.GetRegion(worldRegionPositions[x, y]);
                    SetTilesInMap32(new CCTileMapCoordinates(x * Constants.REGION_SIZE_X, y * Constants.REGION_SIZE_Y), Region);
                }
            }
        }

        public void SetTilesInMap32(CCTileMapCoordinates mapUpperLeftCoordinate, Region region)
        {
            for (int y = 0; y < Constants.REGION_SIZE_Y; y++)
            {
                for (int x = 0; x < Constants.REGION_SIZE_X; x++)
                {
                    var newCellPosition = new CellPosition(x, y);
                    var mapCellPosition = new MapCellPosition((mapUpperLeftCoordinate.Column + x), (mapUpperLeftCoordinate.Row + y));

                    SetTerrainTileInMap(newCellPosition, mapCellPosition.GetTileMapCoordinates(), region); 
                    SetEntityTileInMap(newCellPosition, mapCellPosition.GetTileMapCoordinates(), region); 
                
                }
            }
        }

        public void SetTerrainTileInMap(CellPosition cellPosition, CCTileMapCoordinates mapCoordinat, Region region)
        {
            var gid = client.Common.Views.ViewDefinitions.Instance.DefinitionToTileGid(region.GetTerrain(cellPosition));
            TerrainLayer.SetTileGID(gid, mapCoordinat);
        }

        public void SetUnit(CCTileMapCoordinates mapCoordinat, Entity unit)
        {
            if (unit == null)
            {
                UnitLayer.SetTileGID(CCTileGidAndFlags.EmptyTile, mapCoordinat);//RemoveTile (mapCoordinat);
            }
            else
            {
                var sort = ViewDefinitions.Sort.Normal;
                if (GameAppDelegate.Account != unit.Account)
                {
                    sort = ViewDefinitions.Sort.Enemy;
                }
                var gid = ViewDefinitions.Instance.DefinitionToTileGid(unit.Definition, sort);
                UnitLayer.SetTileGID(gid, mapCoordinat);
            }
        }

        public void SetBuilding(CCTileMapCoordinates mapCoordinat, Entity building)
        {
            if (building == null)
            {
                BuildingLayer.SetTileGID(CCTileGidAndFlags.EmptyTile, mapCoordinat);//RemoveTile (mapCoordinat);               
            }
            else
            {
                var sort = ViewDefinitions.Sort.Normal;
                if (GameAppDelegate.Account != building.Account)
                {
                    sort = ViewDefinitions.Sort.Enemy;
                }
                var gid = ViewDefinitions.Instance.DefinitionToTileGid(building.Definition, sort);
                BuildingLayer.SetTileGID(gid, mapCoordinat);
            }
        }


        public void SetEntityTileInMap(CellPosition cellPosition, CCTileMapCoordinates mapCoordinat, Region region)
        {   
            
            var entity = region.GetEntity(cellPosition);
            if (entity != null)
            {
				if (entity.Definition.Category == Core.Models.Definitions.Category.Unit)
                {
                    SetUnit(mapCoordinat, entity);
                }
				if (entity.Definition.Category == Core.Models.Definitions.Category.Building)
                {
                    SetBuilding(mapCoordinat, entity);
                }
            }
            else
            {
                UnitLayer.SetTileGID(CCTileGidAndFlags.EmptyTile, mapCoordinat);
                BuildingLayer.SetTileGID(CCTileGidAndFlags.EmptyTile, mapCoordinat);
            }
          

        }



        public CCTileMapCoordinates GetCurrentTileInMap(Position position)
        {
            var regionPos = new RegionPosition(position);
            var cellPos = new CellPosition(position);
            var worldRegions = m_RegionManagerController.GetWorldNearRegionPositions(regionPos);

            int mapCellX = -1;
            int mapCellY = -1;

            for (int x = 0; x < ClientConstants.DRAW_REGIONS_X; x++)
            {
                for (int y = 0; y < ClientConstants.DRAW_REGIONS_Y; y++)
                {
                    if (regionPos.Equals(worldRegions[x, y]))
                    {
                        mapCellX = cellPos.CellX + (x * Constants.REGION_SIZE_X);
                        mapCellY = cellPos.CellY + (y * Constants.REGION_SIZE_Y);
                    } 

                }

            }

            var MapCellPosition = new MapCellPosition(mapCellX, mapCellY);
            return MapCellPosition.GetTileMapCoordinates();
        }


        public Position GetCurrentGamePosition(MapCellPosition mapCellPosition, RegionPosition currentCenterRegion)
        {
            var worldRegions = m_RegionManagerController.GetWorldNearRegionPositions(currentCenterRegion);

            var cellX = mapCellPosition.CellX % Constants.REGION_SIZE_X;
            var cellY = mapCellPosition.CellY % Constants.REGION_SIZE_Y;
            CellPosition cellPosition = new CellPosition(cellX, cellY);

            var x = mapCellPosition.CellX / Constants.REGION_SIZE_X;
            var y = mapCellPosition.CellY / Constants.REGION_SIZE_Y;

            if (x >= 0 && x < ClientConstants.DRAW_REGIONS_X &&
                y >= 0 && y < ClientConstants.DRAW_REGIONS_Y)
            {
                     
                var regionPosition = worldRegions[x, y];

                return new Position(regionPosition, cellPosition);
            }
            return null;
        }

        public bool IsCellInOutsideRegion(MapCellPosition mapCellPosition)
        {
            if (mapCellPosition.CellX < ClientConstants.REDRAW_REGIONS_START_X || mapCellPosition.CellX > ClientConstants.REDRAW_REGIONS_END_X)
                return true;
            if (mapCellPosition.CellY < ClientConstants.REDRAW_REGIONS_START_Y || mapCellPosition.CellY > ClientConstants.REDRAW_REGIONS_END_Y)
                return true;

            return false;
        }

        public void DrawMenu(CCTileMapCoordinates mapCoordinat, Entity unit)
        {
            
        }

        #region Fields

        client.Common.Manager.RegionManagerController m_RegionManagerController;

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


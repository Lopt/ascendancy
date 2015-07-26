using System;
using CocosSharp;
using Core.Models;
using Client.Common.Manager;
using Client.Common.Models;
using Client.Common.Helper;
using System.Linq.Expressions;
using System.Threading;

namespace Client.Common.Views
{
    /// <summary>
    /// Region view to set the content of the tile map.
    /// </summary>
    public class RegionView
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Client.Common.Views.RegionView"/> class.
        /// </summary>
        public RegionView()
        {
            m_RegionManagerController = Core.Controllers.Controller.Instance.RegionManagerController as Client.Common.Manager.RegionManagerController;
        }

        /// <summary>
        /// Sets the tiles in map 160x160.
        /// </summary>
        /// <param name="region">Region.</param>
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

        /// <summary>
        /// Sets the tiles in map 32x32.
        /// </summary>
        /// <param name="mapUpperLeftCoordinate">Map upper left coordinate.</param>
        /// <param name="region">Region.</param>
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

        /// <summary>
        /// Sets the terrain tile in map.
        /// </summary>
        /// <param name="cellPosition">Cell position.</param>
        /// <param name="mapCoordinat">Map coordinat.</param>
        /// <param name="region">Region.</param>
        public void SetTerrainTileInMap(CellPosition cellPosition, CCTileMapCoordinates mapCoordinat, Region region)
        {
            var gid = Client.Common.Views.ViewDefinitions.Instance.DefinitionToTileGid(region.GetTerrain(cellPosition));
            TerrainLayer.SetTileGID(gid, mapCoordinat);
        }

        /// <summary>
        /// Sets the unit in the map.
        /// </summary>
        /// <param name="mapCoordinat">Map coordinat.</param>
        /// <param name="unit">Unit.</param>
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

        /// <summary>
        /// Sets the building in the map.
        /// </summary>
        /// <param name="mapCoordinat">Map coordinat.</param>
        /// <param name="building">Building.</param>
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

        /// <summary>
        /// Sets the entity tile in in the map.
        /// </summary>
        /// <param name="cellPosition">Cell position.</param>
        /// <param name="mapCoordinat">Map coordinat.</param>
        /// <param name="region">Region.</param>
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

        /// <summary>
        /// Gets the current tile in map at the position. Converts the position to the tile in the map.
        /// </summary>
        /// <returns>The current tile in map.</returns>
        /// <param name="position">Position.</param>
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

        /// <summary>
        /// Gets the current game position at the map cell position. Convert the map cell to the game position. 
        /// </summary>
        /// <returns>The current game position.</returns>
        /// <param name="mapCellPosition">Map cell position.</param>
        /// <param name="currentCenterRegion">Current center region.</param>
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

        /// <summary>
        /// Determines whether this instance is cell in outside region the specified mapCellPosition.
        /// </summary>
        /// <returns><c>true</c> if this instance is cell in outside region the specified mapCellPosition; otherwise, <c>false</c>.</returns>
        /// <param name="mapCellPosition">Map cell position.</param>
        public bool IsCellInOutsideRegion(MapCellPosition mapCellPosition)
        {
            if (mapCellPosition.CellX < ClientConstants.REDRAW_REGIONS_START_X || mapCellPosition.CellX > ClientConstants.REDRAW_REGIONS_END_X)
                return true;
            if (mapCellPosition.CellY < ClientConstants.REDRAW_REGIONS_START_Y || mapCellPosition.CellY > ClientConstants.REDRAW_REGIONS_END_Y)
                return true;

            return false;
        }

        #region Fields

        /// <summary>
        /// The m_region manager controller.
        /// </summary>
        Client.Common.Manager.RegionManagerController m_RegionManagerController;

        /// <summary>
        /// The terrain layer.
        /// </summary>
        public CCTileMapLayer TerrainLayer;
        /// <summary>
        /// The building layer.
        /// </summary>
        public CCTileMapLayer BuildingLayer;
        /// <summary>
        /// The unit layer.
        /// </summary>
        public CCTileMapLayer UnitLayer;
        /// <summary>
        /// The menu layer.
        /// </summary>
        public CCTileMapLayer MenuLayer;

        /// <summary>
        /// Gets the world layer.
        /// </summary>
        /// <value>The world layer.</value>
        public WorldLayer WorldLayer
        {
            private set;
            get;
        }


        #endregion
    }
}


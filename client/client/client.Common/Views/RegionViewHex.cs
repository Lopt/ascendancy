namespace Client.Common.Views
{
    using System;
    using System.Linq.Expressions;
    using System.Threading;
    using Client.Common.Constants;
    using Client.Common.Helper;
    using Client.Common.Manager;
    using Client.Common.Models;
    using CocosSharp;
    using Core.Models;
    using Core.Views;
    using SQLitePCL;
    using System.Threading.Tasks;

    /// <summary>
    /// Region view with hexagonal tile map to set the content of the tile map.
    /// </summary>
    public class RegionViewHex : ViewEntity
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="Client.Common.Views.RegionViewHex"/> class.
        /// </summary>
        /// <param name="region">The Region to draw.</param>
        public RegionViewHex(Region region)
            : base(region)
        {
            
            var tileMapInfo = new CCTileMapInfo(Common.Constants.ClientConstants.TILEMAP_FILE_HEX);
            m_tileMap = new CCTileMap(tileMapInfo);
             
            m_terrainLayer = m_tileMap.LayerNamed(ClientConstants.LAYER_TERRAIN);
            m_buildingLayer = m_tileMap.LayerNamed(ClientConstants.LAYER_BUILDING);
            m_unitLayer = m_tileMap.LayerNamed(ClientConstants.LAYER_UNIT);
            m_menueLayer = m_tileMap.LayerNamed(ClientConstants.LAYER_MENU);
            Init();
            LoadRegionViewAsync();

        }

        /// <summary>
        /// Gets the tile map.
        /// </summary>
        /// <returns>The tile map.</returns>
        public CCTileMap GetTileMap()
        {
            return m_tileMap;
        }

        /// <summary>
        /// Sets the unit in the map.
        /// </summary>
        /// <param name="mapCoordinat">Map coordinate.</param>
        /// <param name="unit">Unit which should be drawn (or null if it should be erased).</param>
        public void SetUnit(CCTileMapCoordinates mapCoordinat, Entity unit)
        {
            if (unit == null)
            {
                m_unitLayer.SetTileGID(CCTileGidAndFlags.EmptyTile, mapCoordinat);
            }
            else
            {
                var sort = ViewDefinitions.Sort.Normal;
                if (GameAppDelegate.Account != unit.Owner)
                {
                    sort = ViewDefinitions.Sort.Enemy;
                }
                var gid = ViewDefinitions.Instance.DefinitionToTileGid(unit.Definition, sort);
                m_unitLayer.SetTileGID(gid, mapCoordinat);
            }
        }

        /// <summary>
        /// Sets the building in the map.
        /// </summary>
        /// <param name="mapCoordinat">Map coordinate.</param>
        /// <param name="building">Building which should be drawn (or null if it should be erased).</param>
        public void SetBuilding(CCTileMapCoordinates mapCoordinat, Entity building)
        {
            if (building == null)
            {
                m_buildingLayer.SetTileGID(CCTileGidAndFlags.EmptyTile, mapCoordinat);               
            }
            else
            {
                var sort = ViewDefinitions.Sort.Normal;
                if (GameAppDelegate.Account != building.Owner)
                {
                    sort = ViewDefinitions.Sort.Enemy;
                }
                var gid = ViewDefinitions.Instance.DefinitionToTileGid(building.Definition, sort);
                m_buildingLayer.SetTileGID(gid, mapCoordinat);
            }
        }


        /// <summary>
        /// Loads the region view hex dictionary with all regions (5x5) arround the currentPosition.
        /// </summary>
        /// <returns>The region view hex dic.</returns>
        private async Task LoadRegionViewAsync()
        {
            var region = (Region)this.Model;
            var regionPosition = region.RegionPosition;
            var regionManagerController = Core.Controllers.Controller.Instance.RegionManagerController as Client.Common.Manager.RegionManagerController;
 
            await regionManagerController.LoadTerrainsAsync(regionPosition);

            await EntityManagerController.Instance.LoadEntitiesAsync(regionPosition);

            SetTilesInMap32();
        }

        private void SetWorldPosition()
        {
            var position = PositionHelper.RegionViewHexToWorldPoint(this);
            m_tileMap.TileLayersContainer.Position = position;
        }


        /// <summary>
        /// Update this instance.
        /// </summary>
        private void Init()
        {
            ClearLayers();
            SetWorldPosition();           
        }

        /// <summary>
        /// Clears the Layers for initialization.
        /// </summary>
        private void ClearLayers()
        {
            var coordHelper = new CCTileMapCoordinates(0, 0);
            m_buildingLayer.RemoveTile(coordHelper);
            m_unitLayer.RemoveTile(coordHelper);
//            m_menueLayer.RemoveTile(coordHelper);
        }

        /// <summary>
        /// Sets the tiles in map 32x32.
        /// </summary>
        private void SetTilesInMap32()
        {
            for (int y = 0; y < Constants.REGION_SIZE_Y; y++)
            {
                for (int x = 0; x < Constants.REGION_SIZE_X; x++)
                {
                    var newCellPosition = new CellPosition(x, y);

                    SetTerrainTileInMap(newCellPosition); 
                    SetEntityTileInMap(newCellPosition); 
                }
            }
        }

        /// <summary>
        /// Sets the terrain tile in map.
        /// </summary>
        /// <param name="cellPosition">Cell position.</param>
        private void SetTerrainTileInMap(CellPosition cellPosition)
        {
            var region = (Region)this.Model;
            var gid = Client.Common.Views.ViewDefinitions.Instance.DefinitionToTileGid(region.GetTerrain(cellPosition));
            m_terrainLayer.SetTileGID(gid, new CCTileMapCoordinates(cellPosition.CellX, cellPosition.CellY));
        }

        /// <summary>
        /// Sets the entity tile in in the map.
        /// </summary>
        /// <param name="cellPosition">Cell position.</param>
        private void SetEntityTileInMap(CellPosition cellPosition)
        {   
            var region = (Region)this.Model;
            var entity = region.GetEntity(cellPosition);
            var mapCoordinat = new CCTileMapCoordinates(cellPosition.CellX, cellPosition.CellY);
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
                m_unitLayer.SetTileGID(CCTileGidAndFlags.EmptyTile, mapCoordinat);
                m_buildingLayer.SetTileGID(CCTileGidAndFlags.EmptyTile, mapCoordinat);
            }
        }

        #region Fields

        /// <summary>
        /// The terrain layer.
        /// </summary>
        private CCTileMapLayer m_terrainLayer;

        /// <summary>
        /// The building layer.
        /// </summary>
        private CCTileMapLayer m_buildingLayer;

        /// <summary>
        /// The unit layer.
        /// </summary>
        private CCTileMapLayer m_unitLayer;

        /// <summary>
        /// The menue layer.
        /// </summary>
        private CCTileMapLayer m_menueLayer;

        /// <summary>
        /// The tile map.
        /// </summary>
        private CCTileMap m_tileMap;

        #endregion
    }
}

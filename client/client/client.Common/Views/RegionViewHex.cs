namespace Client.Common.Views
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading;
    using System.Threading.Tasks;
    using Client.Common.Constants;
    using Client.Common.Helper;
    using Client.Common.Manager;
    using Client.Common.Models;
    using CocosSharp;
    using Core.Models;
    using Core.Views;
    using SQLitePCL;

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
            m_tileMap.TileLayersContainer.AnchorPoint = new CCPoint(0.0f, 1.0f);
             
            m_terrainLayer = m_tileMap.LayerNamed(ClientConstants.LAYER_TERRAIN);
            m_buildingLayer = m_tileMap.LayerNamed(ClientConstants.LAYER_BUILDING);
            m_unitLayer = m_tileMap.LayerNamed(ClientConstants.LAYER_UNIT);
            m_menueLayer = m_tileMap.LayerNamed(ClientConstants.LAYER_MENU);
            m_indicatorLayer = m_tileMap.LayerNamed(ClientConstants.LAYER_INDICATOR);

            m_drawNodes = new Dictionary<Account, CCDrawNode>();

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
        /// Make a ugle draw (moves the tile layers container a little bit).
        /// </summary>
        public void UglyDraw()
        {
            var container = m_tileMap.TileLayersContainer;
            float offset = 0.001f;
            CCPoint position1 = new CCPoint(container.Position.X + offset, container.Position.Y + offset);
            container.Position = position1;
            CCPoint position2 = new CCPoint(container.Position.X - offset, container.Position.Y - offset);
            container.Position = position2;
        }

        /// <summary>
        /// Draws the unit.
        /// </summary>
        /// <param name="unit"> Unit </param>
        public void DrawUnit(Entity unit)
        {
            var regionM = (Region)Model;
            var unitV = (UnitView)unit.View;

            if (unitV == null)
            {
                unitV = new UnitView(unit);
            }

            if (unitV.DrawRegion == regionM.RegionPosition)
            {
                unitV.Node.RemoveFromParent();
                m_tileMap.TileLayersContainer.AddChild(unitV.Node);
                unitV.Node.Position = unitV.DrawPoint;
            }
            else
            {
                m_tileMap.TileLayersContainer.RemoveChild(unitV.Node);
            }
        }

        /// <summary>
        /// Removes the unit.
        /// </summary>
        /// <param name="unit">Unit</param>
        public void RemoveUnit(Entity unit)
        {
            var unitV = (UnitView)unit.View;
            m_tileMap.TileLayersContainer.RemoveChild(unitV.Node);
        }

        /// <summary>
        /// Removes the building.
        /// </summary>
        /// <param name="building">Building.</param>
        public void RemoveBuilding(Entity building)
        {
            m_buildingLayer.RemoveTile(new CCTileMapCoordinates(building.Position.CellPosition.CellX, building.Position.CellPosition.CellY));
        }

        /// <summary>
        /// Sets the menu tile.
        /// </summary>
        /// <param name="cellPos">Cell position.</param>
        /// <param name="definition">Definition.</param>
        public void SetMenuTile(CellPosition cellPos, CCTileGidAndFlags definition)
        {
            m_menueLayer.SetTileGID(definition, new CCTileMapCoordinates(cellPos.CellX, cellPos.CellY));
        }

        /// <summary>
        /// Removes the menu tile.
        /// </summary>
        /// <param name="cellPos">Cell position.</param>
        public void RemoveMenuTile(CellPosition cellPos)
        {
            m_menueLayer.RemoveTile(new CCTileMapCoordinates(cellPos.CellX, cellPos.CellY));
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
        /// Sets the indicator gid.
        /// </summary>
        /// <returns>The indicator gid.</returns>
        /// <param name="cellPos">Cell position.</param>
        /// <param name="gid">Gid.</param>
        public CCSprite SetIndicatorGid(CellPosition cellPos, CCTileGidAndFlags gid)
        {            
            m_indicatorLayer.SetTileGID(gid, new CCTileMapCoordinates(cellPos.CellX, cellPos.CellY));
            var sprite = m_indicatorLayer.ExtractTile(new CCTileMapCoordinates(cellPos.CellX, cellPos.CellY), true);
            sprite.Opacity = HelperSpritesGid.INDICATOR_OPACITY;
            return sprite;
        }

        /// <summary>
        /// Draws the border.
        /// </summary>
        /// <param name="borderPositions">Border positions.</param>
        /// <param name="color">Color.</param>
        public void DrawBorder(HashSet<PositionI> borderPositions, CCColor4B color)
        {
            var region = (Region)this.Model;
            var position = borderPositions.ToList<PositionI>().First();
            var fieldOwner = region.GetClaimedTerritory(position);

            RemoveBorder(fieldOwner);
            var border = new CCDrawNode();

            m_tileMap.TileLayersContainer.AddChild(border);
            m_drawNodes.Add(fieldOwner, border);

            var halfwidth = ClientConstants.TILE_HEX_IMAGE_WIDTH / 2.0f;
            var halfhight = ClientConstants.TILE_HEX_IMAGE_HEIGHT / 2.0f;

            // zentrieren der Grenzpunkte und zeichnen dieser
            foreach (var positionI in borderPositions)
            {
                var centerPos = PositionHelper.CellToTile(positionI.CellPosition);
                centerPos.X += halfwidth;
                centerPos.Y -= halfhight;
                border.DrawSolidCircle(centerPos, ClientConstants.TERRATORRY_BUILDING_BORDER_SIZE, color);
            }
        }

        /// <summary>
        /// Removes the border.
        /// </summary>
        /// <param name="owner">Owner.</param>
        public void RemoveBorder(Account owner)
        {
            CCDrawNode border;
            if (m_drawNodes.TryGetValue(owner, out border))
            {
                m_tileMap.TileLayersContainer.RemoveChild(border);
                m_drawNodes.Remove(owner);
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
 
            var taskTerrain = regionManagerController.LoadTerrainAsync(region);
            var taskEntities = EntityManagerController.Instance.LoadEntitiesAsync(regionPosition);

            await taskTerrain;
            await taskEntities;

            SetTilesInMap32();
        }

        /// <summary>
        /// Sets the world position.
        /// </summary>
        private void SetWorldPosition()
        {
            var position = PositionHelper.RegionToWorldspace(this);
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
            m_menueLayer.RemoveTile(coordHelper);
            m_indicatorLayer.RemoveTile(coordHelper);
        }

        /// <summary>
        /// Sets the tiles in map 32x32.
        /// </summary>
        private void SetTilesInMap32()
        {
            LoadEntities(); 

            for (int y = 0; y < Constants.REGION_SIZE_Y; y++)
            {
                for (int x = 0; x < Constants.REGION_SIZE_X; x++)
                {
                    var newCellPosition = new CellPosition(x, y);

                    SetTerrainTileInMap(newCellPosition); 
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
        private void LoadEntities()
        {
            var regionM = (Region)Model;
            foreach (var unit in regionM.GetEntities().Entities)
            {
                DrawUnit(unit);
            }
        }

        #region Fields

        /// <summary>
        /// The terrain layer.
        /// </summary>
        private CCTileMapLayer m_terrainLayer;

        /// <summary>
        /// The terrain layer.
        /// </summary>
        private CCTileMapLayer m_indicatorLayer;

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

        /// <summary>
        /// The draw nodes.
        /// </summary>
        private Dictionary<Account, CCDrawNode> m_drawNodes;

        #endregion
    }
}

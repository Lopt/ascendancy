namespace Client.Common.Views.Effects
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using CocosSharp;
    using Core.Models;

    /// <summary>
    /// Indicator view.
    /// </summary>
    public class IndicatorView
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Client.Common.Views.Effects.IndicatorView"/> class.
        /// </summary>
        /// <param name="worldlayer">The world layer.</param>
        public IndicatorView(WorldLayerHex worldlayer)
        {
            m_worldLayer = worldlayer;
            m_sprites = new List<CCSprite>();
            m_areaIndicators = new Dictionary<TileTouchHandler.Area, CCTileGidAndFlags>();
            m_areaIndicators.Add(TileTouchHandler.Area.Movement, new CCTileGidAndFlags(Client.Common.Constants.HelperSpritesGid.WHITEINDICATOR));
            m_areaIndicators.Add(TileTouchHandler.Area.OwnTerritory, new CCTileGidAndFlags(Client.Common.Constants.HelperSpritesGid.GREENINDICATOR));
            m_areaIndicators.Add(TileTouchHandler.Area.EnemyTerritory, new CCTileGidAndFlags(Client.Common.Constants.HelperSpritesGid.REDINDICATOR));
            m_areaIndicators.Add(TileTouchHandler.Area.AllyTerritory, new CCTileGidAndFlags(Client.Common.Constants.HelperSpritesGid.BLUEINDICATOR));

            m_surroundedPositions = new HashSet<PositionI>();
        }

        /// <summary>
        /// Finalizes an instance of the <see cref="IndicatorView"/> class.
        /// </summary>
        ~IndicatorView()
        {
            m_areaIndicators.Clear();
            RemoveIndicator();
        }

        /// <summary>
        /// Draws the indicator on the Map
        /// </summary>
        /// <param name="coord">The Center Position.</param>
        /// <param name="range">The Range.</param>
        /// <param name="area">The Area Type.</param>
        public void ShowUnitIndicator(PositionI coord, int range, TileTouchHandler.Area area)
        {
            var gid = new CCTileGidAndFlags(Client.Common.Constants.HelperSpritesGid.GREENINDICATOR);

            m_areaIndicators.TryGetValue(area, out gid);
         
            if (area == TileTouchHandler.Area.Movement)
            {
                var indi = new Core.Controllers.AStar_Indicator.Indicator(coord, range, GameAppDelegate.Account.ID); 
                m_surroundedPositions = indi.FindPossiblePositions();
            }
   
            AddSprites(gid);
        }

        /// <summary>
        /// Shows the building indicator.
        /// </summary>
        /// <param name="coord">The positionI.</param>
        /// <param name="area">The ownership of the area.</param>
        public void ShowBuildingIndicator(PositionI coord, TileTouchHandler.Area area)
        {
            var gid = new CCTileGidAndFlags(Client.Common.Constants.HelperSpritesGid.GREENINDICATOR);

            m_areaIndicators.TryGetValue(area, out gid);

            var buildings = Core.Controllers.Controller.Instance.RegionManagerController.GetRegion(coord.RegionPosition)
                                .GetEntity(coord.CellPosition).Owner.TerritoryBuildings.Keys;

            int range;
            foreach (var building in buildings)
            {
                var entity = Core.Controllers.Controller.Instance.RegionManagerController.GetRegion(building.RegionPosition).GetEntity(building.CellPosition);
                if (entity.Definition.SubType == Core.Models.Definitions.EntityType.Headquarter)
                {
                    range = Core.Models.Constants.HEADQUARTER_TERRITORY_RANGE;
                }
                else
                {
                    range = Core.Models.Constants.GUARDTOWER_TERRITORY_RANGE;
                }

                var surroundedPositions = LogicRules.GetSurroundedPositions(building, range);
                m_surroundedPositions.UnionWith(surroundedPositions);
            }

            AddSprites(gid);
        }

        /// <summary>
        /// Removes the indicator.
        /// </summary>
        public void RemoveIndicator()
        {     
            foreach (var sprite in m_sprites)
            {
                sprite.Parent.RemoveChild(sprite);
            }
            m_sprites.Clear();
        }

        /// <summary>
        /// Adds the sprites to the region view.
        /// </summary>
        /// <param name="gid">The Gid.</param>
        private void AddSprites(CCTileGidAndFlags gid)
        {
            foreach (var tile in m_surroundedPositions)
            {
                m_sprites.Add(m_worldLayer.GetRegionViewHex(tile.RegionPosition).SetIndicatorGid(tile.CellPosition, gid));
            }
        }

        /// <summary>
        /// The world layer.
        /// </summary>
        private WorldLayerHex m_worldLayer;

        /// <summary>
        /// The sprites.
        /// </summary>
        private List<CCSprite> m_sprites;

        /// <summary>
        /// The area indicators.
        /// </summary>
        private Dictionary<TileTouchHandler.Area, CCTileGidAndFlags> m_areaIndicators;

        /// <summary>
        /// The surrounded tile set.
        /// </summary>
        private HashSet<PositionI> m_surroundedPositions;
    }
}

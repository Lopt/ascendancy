
namespace Client.Common.Views.Effects
{
    using System;
    using System.Collections.Generic;
    using CocosSharp;
    using Core.Models;
    using System.Collections;

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
        }

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
        public void ShowIndicator(PositionI coord, int range, TileTouchHandler.Area area)
        {

            var gid = new CCTileGidAndFlags(Client.Common.Constants.HelperSpritesGid.GREENINDICATOR);

            m_areaIndicators.TryGetValue(area, out gid);
         
            if (area == TileTouchHandler.Area.Movement)
            {
                var indi = new Core.Controllers.AStar_Indicator.Indicator(coord, range, GameAppDelegate.Account.ID); 
                m_surroundedPositions = indi.FindPossiblePositions();
            }
            else
            {
                m_surroundedPositions = LogicRules.GetSurroundedPositions(coord, range);
            }

            foreach (var tile in m_surroundedPositions)
            {
                m_sprites.Add(m_worldLayer.GetRegionViewHex(tile.RegionPosition).SetIndicatorGid(tile.CellPosition, gid));
            }

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
        private Dictionary<TileTouchHandler.Area,CCTileGidAndFlags> m_areaIndicators;

        /// <summary>
        /// The surrounded tile set.
        /// </summary>
        private HashSet<PositionI> m_surroundedPositions;
    }
}

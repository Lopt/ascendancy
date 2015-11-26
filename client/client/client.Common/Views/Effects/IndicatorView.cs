namespace Client.Common.Views.Effects
{
    using System;
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
        /// <param name="worldlayer">The Worldlayer.</param>
        public IndicatorView(WorldLayer worldlayer)
        {
            m_worldLayer = worldlayer;
        }
            
        /// <summary>
        /// Draws the indicator on the Map
        /// </summary>
        /// <param name="coord">The Center Position.</param>
        /// <param name="range">The Range.</param>
        /// <param name="type">The Indicator Type.</param>
        public void ShowIndicator(PositionI coord, int range, int type)
        {
            var gid = new CCTileGidAndFlags(74);
            switch (type)
            {
                // influence range
                case 1:
                    gid = new CCTileGidAndFlags(84);
                    break;

                // attack range
                case 2:
                    gid = new CCTileGidAndFlags(85);
                    break;

                // movement range
                case 3:
                    gid = new CCTileGidAndFlags(86);
                    break;
            }

            m_surroundedPositions = LogicRules.GetSurroundedPositions(coord, range);

            foreach (var tile in m_surroundedPositions)
            {
                var mapCoordinate = Helper.PositionHelper.PositionToTileMapCoordinates(m_worldLayer.CenterPosition, tile);
                m_worldLayer.IndicatorLayer.SetTileGID(gid, mapCoordinate);
            }
        }

        /// <summary>
        /// Removes the indicator.
        /// </summary>
        public void RemoveIndicator()
        {
            foreach (var item in m_surroundedPositions)
            {
                var mapCoordinate = Helper.PositionHelper.PositionToTileMapCoordinates(m_worldLayer.CenterPosition, item);
                m_worldLayer.IndicatorLayer.RemoveTile(mapCoordinate); 
            }
        }

        /// <summary>
        /// The m world layer.
        /// </summary>
        private WorldLayer m_worldLayer;

        /// <summary>
        /// The surrounded tile set.
        /// </summary>
        private HashSet<PositionI> m_surroundedPositions;
    }
}

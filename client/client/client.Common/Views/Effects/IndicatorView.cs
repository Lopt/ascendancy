namespace Client.Common.Views.Effects
{
    using CocosSharp;
    /// <summary>
    /// Indicator view.
    /// </summary>
    public class IndicatorView
    {
        public IndicatorView(CCTileMapCoordinates center, CCTileMapLayer layer)
        {
            m_center = center;
            m_layer = layer;

        }

        /// <summary>
        /// The center point for the indicator
        /// </summary>
        private CCTileMapCoordinates m_center;

        /// <summary>
        /// The layer which should be colored as indicator.
        /// </summary>
        private CCTileMapLayer m_layer;

    }
}

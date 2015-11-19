using Client.Common.Constants;

namespace Client.Common.Helper
{
    using System;
    using Client.Common.Models;
    using Client.Common.Views;
    using Core.Models;
    using CocosSharp;

    /// <summary>
    /// Position helper convert map and tile positions.
    /// </summary>
    public class PositionHelper
    {
        public static CCPoint RegionViewHexToWorldPosition(RegionViewHex regionViewHex)
        {
            return RegionPositionToWorldPosition(((Region)regionViewHex.Model).RegionPosition, 
                regionViewHex.GetTileMap().TileLayersContainer.ScaledContentSize);
        }

        public static CCPoint RegionPositionToWorldPosition(RegionPosition regionPosition, CCSize scaledContentSize)
        {
            return new CCPoint(regionPosition.RegionX * scaledContentSize.Width,
                -regionPosition.RegionY * scaledContentSize.Height);
        }
    }
}
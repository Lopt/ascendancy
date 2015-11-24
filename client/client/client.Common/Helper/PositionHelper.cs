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
            return RegionPositionToWorldPosition(((Region)regionViewHex.Model).RegionPosition);
        }

        public static CCPoint RegionPositionToWorldPosition(RegionPosition regionPosition)
        {
            return new CCPoint(regionPosition.RegionX * ClientConstants.TILEMAP_HEX_CONTENTSIZE_WIDTH,
                -regionPosition.RegionY * ClientConstants.TILEMAP_HEX_CONTENTSIZE_HEIGTH);
        }

        public static RegionPosition WorldPositionToRegionPosition(CCPoint point)
        {
            return new RegionPosition((int)(point.X / ClientConstants.TILEMAP_HEX_CONTENTSIZE_WIDTH),
                (int)(-point.Y / ClientConstants.TILEMAP_HEX_CONTENTSIZE_HEIGTH));
        }

        public static PositionI WorldPositionToGamePositionI(CCPoint point)
        {
            var positionX = (int)point.X / ClientConstants.TILEMAP_HEX_CONTENTSIZE_WIDTH;

            if ((positionX % 2) == 1)
            {
                point.Y -= ClientConstants.TILE_HEX_IMAGE_HEIGHT / 2;
            }

            var positionY = (int)-point.Y / ClientConstants.TILEMAP_HEX_CONTENTSIZE_HEIGTH;
            return new PositionI((int)positionX, (int)positionY);
        }

        public static CCPoint GamePositionIToWorldPosition(PositionI positionI)
        {
            var pointX = (float)positionI.X * ClientConstants.TILEMAP_HEX_CONTENTSIZE_WIDTH;
            var pointY = (float)positionI.Y * ClientConstants.TILEMAP_HEX_CONTENTSIZE_HEIGTH;
            if ((positionI.X % 2) == 1)
            {
                pointY += ClientConstants.TILE_HEX_IMAGE_HEIGHT / 2;   
            }
            return new CCPoint(pointX, pointY);
        }
        
    }
}
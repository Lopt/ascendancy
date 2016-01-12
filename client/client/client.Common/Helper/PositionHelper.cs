using Client.Common.Constants;
using Xamarin.Forms;

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
        public static CCPoint RegionViewHexToWorldPoint(RegionViewHex regionViewHex)
        {
            return RegionPositionToWorldPoint(((Region)regionViewHex.Model).RegionPosition);
        }

        public static CCPoint RegionPositionToWorldPoint(RegionPosition regionPosition)
        {
            var firstRegion = Geolocation.Instance.FirstGamePosition.RegionPosition;
            return new CCPoint((regionPosition.RegionX - firstRegion.RegionX) * ClientConstants.TILEMAP_HEX_CONTENTSIZE_WIDTH,
                -(regionPosition.RegionY - firstRegion.RegionY) * ClientConstants.TILEMAP_HEX_CONTENTSIZE_HEIGHT);
        }

        public static RegionPosition WorldPointToRegionPosition(CCPoint point)
        {
            var firstRegion = Geolocation.Instance.FirstGamePosition.RegionPosition;
            return new RegionPosition((int)(point.X / ClientConstants.TILEMAP_HEX_CONTENTSIZE_WIDTH) + firstRegion.RegionX,
                (int)(-point.Y / ClientConstants.TILEMAP_HEX_CONTENTSIZE_HEIGHT) + firstRegion.RegionY);
        }

        public static PositionI WorldPointToGamePositionI(CCPoint point)
        {
            var positionX = (int)point.X / ClientConstants.TILEMAP_HEX_CONTENTSIZE_WIDTH;

            if ((positionX % 2) == 1)
            {
                point.Y -= ClientConstants.TILE_HEX_IMAGE_HEIGHT / 2;
            }

            var positionY = (int)-point.Y / ClientConstants.TILEMAP_HEX_CONTENTSIZE_HEIGHT;
            var regionPos = new RegionPosition((int)positionX, (int)positionY);
            var cellPos = new CellPosition((int)((positionX - (int)positionX) * (ClientConstants.TILEMAP_HEX_WIDTH - 1)),
                              (int)((positionY - (int)positionY) * (ClientConstants.TILEMAP_HEX_HEIGHT - 1)));
            return new PositionI(regionPos, cellPos);
        }

        public static CCPoint GamePositionIToWorldPoint(PositionI positionI)
        {
            var pointX = (float)positionI.RegionPosition.RegionX * ClientConstants.TILEMAP_HEX_CONTENTSIZE_WIDTH;
            var pointY = (float)positionI.RegionPosition.RegionY * ClientConstants.TILEMAP_HEX_CONTENTSIZE_HEIGHT;
            pointX += (float)positionI.CellPosition.CellX / (ClientConstants.TILEMAP_HEX_WIDTH - 1) * ClientConstants.TILEMAP_HEX_CONTENTSIZE_WIDTH;
            pointY += (float)positionI.CellPosition.CellY / (ClientConstants.TILEMAP_HEX_HEIGHT - 1) * ClientConstants.TILEMAP_HEX_CONTENTSIZE_HEIGHT;
            if ((positionI.X % 2) == 1)
            {
                pointY += ClientConstants.TILE_HEX_IMAGE_HEIGHT / 2;   
            }
            return new CCPoint(pointX, -pointY);
        }

        public static Position WorldPointToGamePosition(CCPoint point)
        {
            var firstRegion = Geolocation.Instance.FirstGamePosition.RegionPosition;

            var positionX = point.X / ClientConstants.TILEMAP_HEX_CONTENTSIZE_WIDTH;
            var positionY = -point.Y / ClientConstants.TILEMAP_HEX_CONTENTSIZE_HEIGHT;
            var regionPos = new RegionPosition((int)positionX + firstRegion.RegionX, (int)positionY + firstRegion.RegionY);
            var cellPosX = (positionX - (int)positionX) * (ClientConstants.TILEMAP_HEX_WIDTH - 1);
            var cellPosY = (positionY - (int)positionY) * (ClientConstants.TILEMAP_HEX_HEIGHT - 1);
            return new Position(regionPos.RegionX, regionPos.RegionY, cellPosX, cellPosY);
        }

        public static CCPoint GamePositionToWorldPoint(Position position)
        {
            var firstRegion = Geolocation.Instance.FirstGamePosition.RegionPosition;
            
            var pointX = (float)(position.RegionPosition.RegionX - firstRegion.RegionX) * ClientConstants.TILEMAP_HEX_CONTENTSIZE_WIDTH;
            var pointY = (float)(position.RegionPosition.RegionY - firstRegion.RegionY) * ClientConstants.TILEMAP_HEX_CONTENTSIZE_HEIGHT;
            pointX += (float)position.CellPosition.CellX / (float)(ClientConstants.TILEMAP_HEX_WIDTH - 1) * ClientConstants.TILEMAP_HEX_CONTENTSIZE_WIDTH;
            pointY += (float)position.CellPosition.CellY / (float)(ClientConstants.TILEMAP_HEX_HEIGHT - 1) * ClientConstants.TILEMAP_HEX_CONTENTSIZE_HEIGHT;

            return new CCPoint(pointX, -pointY);
        }
            
    }
}
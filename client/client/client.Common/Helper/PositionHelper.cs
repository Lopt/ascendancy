using Client.Common.Constants;
using System.Collections.Generic;
using Xamarin.Forms;
using System.Diagnostics;

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

        public static CCPoint RegionToWorldspace(RegionViewHex regionViewHex)
        {
            return RegionToWorldspace(((Region)regionViewHex.Model).RegionPosition);
        }

        public static CCPoint RegionToWorldspace(RegionPosition regionPosition)
        {
            var firstRegion = Geolocation.Instance.FirstGamePosition.RegionPosition;
            return new CCPoint((regionPosition.RegionX - firstRegion.RegionX) * ClientConstants.TILEMAP_HEX_CONTENTSIZE_WIDTH,
                -(regionPosition.RegionY - firstRegion.RegionY) * ClientConstants.TILEMAP_HEX_CONTENTSIZE_HEIGHT);
        }

        public static RegionPosition WorldspaceToRegion(CCPoint point)
        {
            var firstRegion = Geolocation.Instance.FirstGamePosition.RegionPosition;
            return new RegionPosition((int)(point.X / ClientConstants.TILEMAP_HEX_CONTENTSIZE_WIDTH) + firstRegion.RegionX,
                (int)(-point.Y / ClientConstants.TILEMAP_HEX_CONTENTSIZE_HEIGHT) + firstRegion.RegionY);
        }





        public static CCPoint CellToTile(CellPosition cell)
        {
            var regionSize = new CCSize(
                                 ClientConstants.TILEMAP_HEX_CONTENTSIZE_WIDTH,
                                 ClientConstants.TILEMAP_HEX_CONTENTSIZE_HEIGHT);
            var tilePercent = new CCPoint(
                                  ((float)cell.CellX / (Core.Models.Constants.REGION_SIZE_X)),
                                  ((Core.Models.Constants.REGION_SIZE_Y - (float)cell.CellY) / (Core.Models.Constants.REGION_SIZE_Y)));

            var tileCoord = tilePercent * regionSize;
            // if it is an odd tile, it is an half tile below the even tiles
            if ((cell.CellX % 2) == 1)
            {
                var tileHeight = regionSize.Height / ClientConstants.TILEMAP_HEX_HEIGHT;
                tileCoord.Y -= tileHeight / 2;   
            }

            return tileCoord;
        }






        public static PositionI WorldspaceToPositionI(CCPoint point, WorldLayerHex worldLayer)
        {
            var regionPosition = WorldspaceToRegion(point);
            var regionView = worldLayer.GetRegionViewHex(regionPosition);

            if (regionView != null)
            {
                var tileCoord = regionView.GetTileMap().LayerNamed(ClientConstants.LAYER_TERRAIN).ClosestTileCoordAtNodePosition(point);
                return new PositionI(regionPosition, new CellPosition(tileCoord.Column, tileCoord.Row));
            }
            return null;
        }

        public static Position WorldspaceToPosition(CCPoint point)
        {
            var firstRegion = Geolocation.Instance.FirstGamePosition.RegionPosition;

            var positionX = point.X / ClientConstants.TILEMAP_HEX_CONTENTSIZE_WIDTH;
            var positionY = -point.Y / ClientConstants.TILEMAP_HEX_CONTENTSIZE_HEIGHT;
            var regionPos = new RegionPosition((int)positionX + firstRegion.RegionX, (int)positionY + firstRegion.RegionY);
            var cellPosX = (positionX - (int)positionX) * (ClientConstants.TILEMAP_HEX_WIDTH - 1);
            var cellPosY = (positionY - (int)positionY) * (ClientConstants.TILEMAP_HEX_HEIGHT - 1);
            return new Position(regionPos.RegionX, regionPos.RegionY, cellPosX, cellPosY);
        }

        public static CCPoint PositionToWorldspace(Position position)
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
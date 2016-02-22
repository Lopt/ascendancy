namespace Client.Common.Helper
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using Client.Common.Constants;
    using Client.Common.Models;
    using Client.Common.Views;
    using CocosSharp;
    using Core.Models;
    using Xamarin.Forms;

    /// <summary>
    /// Position helper convert map and tile positions.
    /// </summary>
    public class PositionHelper
    {
        /// <summary>
        /// Converts a region(position) to a world space position
        /// </summary>
        /// <returns>The world space position of the given region.</returns>
        /// <param name="regionViewHex">View of a region.</param>
        public static CCPoint RegionToWorldspace(RegionViewHex regionViewHex)
        {
            return RegionToWorldspace(((Region)regionViewHex.Model).RegionPosition);
        }

        /// <summary>
        /// Converts a region position to a world space position
        /// </summary>
        /// <returns>The world space position of the given region position.</returns>
        /// <param name="regionPosition">region position.</param>
        public static CCPoint RegionToWorldspace(RegionPosition regionPosition)
        {
            var firstRegion = Geolocation.Instance.FirstGamePosition.RegionPosition;
            return new CCPoint(
                (regionPosition.RegionX - firstRegion.RegionX) * ClientConstants.TILEMAP_HEX_CONTENTSIZE_WIDTH,
                -(regionPosition.RegionY - firstRegion.RegionY) * ClientConstants.TILEMAP_HEX_CONTENTSIZE_HEIGHT);
        }

        /// <summary>
        /// Converts a world space position to a region position
        /// </summary>
        /// <returns>The region position which is at the given world space position.</returns>
        /// <param name="point">World space Position.</param>
        public static RegionPosition WorldspaceToRegion(CCPoint point)
        {
            var firstRegion = Geolocation.Instance.FirstGamePosition.RegionPosition;
            return new RegionPosition(
                (int)Math.Floor((point.X / ClientConstants.TILEMAP_HEX_CONTENTSIZE_WIDTH) + firstRegion.RegionX),
                (int)Math.Floor((-point.Y / ClientConstants.TILEMAP_HEX_CONTENTSIZE_HEIGHT) + firstRegion.RegionY));
        }

        /// <summary>
        /// Converts a cell position to a tile position (which is a position relative to the region position)
        /// </summary>
        /// <returns>The tile position.</returns>
        /// <param name="cell">Cell position.</param>
        public static CCPoint CellToTile(CellPosition cell)
        {
            var regionSize = new CCSize(
                                 ClientConstants.TILEMAP_HEX_CONTENTSIZE_WIDTH,
                                 ClientConstants.TILEMAP_HEX_CONTENTSIZE_HEIGHT);
            var tilePercent = new CCPoint(
                (float)cell.CellX / Core.Models.Constants.REGION_SIZE_X,
                (Core.Models.Constants.REGION_SIZE_Y - (float)cell.CellY) / Core.Models.Constants.REGION_SIZE_Y);

            var tileCoord = tilePercent * regionSize;
            // if it is an odd tile, it is an half tile below the even tiles
            if ((cell.CellX % 2) == 1)
            {
                var tileHeight = regionSize.Height / ClientConstants.TILEMAP_HEX_HEIGHT;
                tileCoord.Y -= tileHeight / 2;   
            }

            return tileCoord;
        }

        /// <summary>
        /// Converts the given world space position to a PositionI
        /// </summary>
        /// <returns>PositionI of the given world space.</returns>
        /// <param name="point">Point which should be converted.</param>
        /// <param name="worldLayer">World layer.</param>
        public static PositionI WorldspaceToPositionI(CCPoint point, WorldLayerHex worldLayer)
        {
            var regionPos = WorldspaceToRegion(point);
            var startRegionPoint = RegionToWorldspace(regionPos);
            var regionView = worldLayer.GetRegionViewHex(regionPos);

            if (regionView != null)
            {
                var tileCoord = regionView.GetTileMap().LayerNamed(ClientConstants.LAYER_TERRAIN).ClosestTileCoordAtNodePosition(point - startRegionPoint);
                tileCoord.Row = Math.Max(tileCoord.Row, -tileCoord.Row);
                tileCoord.Column = Math.Max(tileCoord.Column, -tileCoord.Column);
                var cellPos = new CellPosition(
                    tileCoord.Column % Constants.REGION_SIZE_X,
                    tileCoord.Row % Constants.REGION_SIZE_Y);
                
                return new PositionI(regionPos, cellPos);
            }
            return null;
        }

        /// <summary>
        /// Converts the world spaces position to a game position.
        /// WARNING: Do not convert the position to an PositionI or similar - it is not guaranteed to be equal.
        /// If you need PositionI, better use the World space to PositionI converter
        /// </summary>
        /// <returns>The game position.</returns>
        /// <param name="point">world space position.</param>
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

        /// <summary>
        /// Converts the Position to a world space position
        /// </summary>
        /// <returns>The world space position.</returns>
        /// <param name="position">game position.</param>
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
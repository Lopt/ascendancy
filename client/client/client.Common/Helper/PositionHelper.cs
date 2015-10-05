namespace Client.Common.Helper
{
    using System;
    using Client.Common.Models;
    using Client.Common.Views;
    using Core.Models;

    /// <summary>
    /// Position helper convert map and tile positions.
    /// </summary>
    public class PositionHelper
    {
        /// <summary>
        /// Positions to tile map coordinates.
        /// </summary>
        /// <returns>The tile map coordinates.</returns>
        /// <param name="centerPosition">Current center position of the drawn world.</param>
        /// <param name="position">Position which should be transformed to a CCTileMapCoordinates.</param>
        public static CocosSharp.CCTileMapCoordinates PositionToTileMapCoordinates(Position centerPosition, PositionI position)
        {
            return PositionToMapCellPosition(centerPosition, position).GetTileMapCoordinates();
        }

        /// <summary>
        /// Positions to map cell position.
        /// </summary>
        /// <returns>The map cell position.</returns>
        /// <param name="centerPosition">Current center position of the drawn world.</param>
        /// <param name="position">Position which should be transformed to a MapPosition.</param>
        public static MapCellPosition PositionToMapCellPosition(Position centerPosition, PositionI position)
        {
            var cellPos = centerPosition.CellPosition;
            int halfRegionX = (int)Common.Constants.ClientConstants.DRAW_REGIONS_X / 2;
            int halfRegionY = (int)Common.Constants.ClientConstants.DRAW_REGIONS_Y / 2;

            var x = (Constants.REGION_SIZE_X * halfRegionX) + cellPos.CellX;
            var y = (Constants.REGION_SIZE_Y * halfRegionY) + cellPos.CellY;

            var relativeCenter = new PositionI(x, y);
                       
            var upperLeftPos = new PositionI((int)centerPosition.X, (int)centerPosition.Y) - relativeCenter;
            var mapCellPosition = position - upperLeftPos;
            var mapPosition = new MapCellPosition(mapCellPosition.X, mapCellPosition.Y);

            return mapPosition;
        }
    }
}
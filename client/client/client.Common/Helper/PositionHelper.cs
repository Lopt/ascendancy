using System;

using Client.Common.Views;
using Client.Common.Models;
using Core.Models;

namespace Client.Common.Helper
{
    /// <summary>
    /// Position helper convert map and tile positions.
    /// </summary>
    public class PositionHelper
    {
        /// <summary>
        /// Positions to tile map coordinates.
        /// </summary>
        /// <returns>The tile map coordinates.</returns>
        /// <param name="centerPosition">Center position.</param>
        /// <param name="position">Position.</param>
        static public CocosSharp.CCTileMapCoordinates PositionToTileMapCoordinates(Position centerPosition, PositionI position)
        {
            return PositionToMapCellPosition(centerPosition, position).GetTileMapCoordinates();
        }

        /// <summary>
        /// Positions to map cell position.
        /// </summary>
        /// <returns>The map cell position.</returns>
        /// <param name="centerPosition">Center position.</param>
        /// <param name="position">Position.</param>
        static public MapCellPosition PositionToMapCellPosition(Position centerPosition, PositionI position)
        {
            var cellPos = centerPosition.CellPosition;
            int halfRegionX = (int)ClientConstants.DRAW_REGIONS_X / 2;
            int halfRegionY = (int)ClientConstants.DRAW_REGIONS_Y / 2;

            var relativeCenter = new PositionI((Constants.REGION_SIZE_X * halfRegionX) + cellPos.CellX,
                                     (Constants.REGION_SIZE_Y * halfRegionY + cellPos.CellY));

            var upperLeftPos = new PositionI((int)centerPosition.X, (int)centerPosition.Y) - relativeCenter;
            var mapCellPosition = position - upperLeftPos;
            var MapPosition = new MapCellPosition(mapCellPosition.X, mapCellPosition.Y);

            return MapPosition;//.GetTileMapCoordinates();
        }

    
    }
}


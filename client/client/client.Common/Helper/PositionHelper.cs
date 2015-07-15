using System;

using client.Common.Views;
using client.Common.Models;
using Core.Models;

namespace client.Common.Helper
{
    public class PositionHelper
    {
        static public CocosSharp.CCTileMapCoordinates PositionToTileMapCoordinates(Position centerPosition, PositionI position)
        {
            return PositionToMapCellPosition(centerPosition, position).GetTileMapCoordinates();
        }

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


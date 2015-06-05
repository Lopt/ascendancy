using System;
using System.Reflection;
using CocosSharp;

namespace client.Common.Models
{
    public class MapCellPosition
    {
        public MapCellPosition (int cellX, int cellY)
        {
            m_CellX = cellX;
            m_CellY = cellY;
        }

        public MapCellPosition (CCTileMapCoordinates tileMapCoordinates)
        {
            var x = tileMapCoordinates.Column;
            var y = 320 - tileMapCoordinates.Row;

            m_CellX = (x * 2) + (y % 2);
            m_CellY = (y / 2);
        }

        public int CellX {
            get {
                return m_CellX;
            }
        }

        public int CellY {
            get {
                return m_CellY;
            }
        }

        public CCTileMapCoordinates GetTileMapCoordinates ()
        {
            return new CCTileMapCoordinates (m_CellX / 2, (m_CellY * 2) + (m_CellX % 2));
        }

        public CCPoint GetAnchor ()
        {

            float x = (m_CellX) / 159.0f;
            float y = (m_CellY) / 159.0f;

            return new CCPoint (x, y / 2);
        }

        private readonly int m_CellX;
        private readonly int m_CellY;
    }
}


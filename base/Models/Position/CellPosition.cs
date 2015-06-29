using System;
using System.Collections.Concurrent;
using Newtonsoft.Json.Linq;

namespace @base.model
{
    public class CellPosition : Object
    {
        public CellPosition(int cellX, int cellY)
        {
            m_cellX = ((int) cellX % Constants.REGION_SIZE_X);
            m_cellY = ((int) cellY % Constants.REGION_SIZE_Y);
        }

        public CellPosition(Position position)
        {
            m_cellX = ((int) position.X % Constants.REGION_SIZE_X);
            m_cellY = ((int) position.Y % Constants.REGION_SIZE_Y);
        }

        public CellPosition(PositionI position)
        {
            m_cellX = (position.X % Constants.REGION_SIZE_X);
            m_cellY = (position.Y % Constants.REGION_SIZE_Y);
        }

        public CellPosition(JContainer obj)
        {
            m_cellX = (int) obj.SelectToken("CellX");
            m_cellY = (int) obj.SelectToken("CellY");
        }

        public int CellX
        {
            get { return m_cellX; }
        }

        public int CellY
        {
            get { return m_cellY; }
        }

        public override bool Equals(Object obj)
        {
            var cellPosition = (CellPosition) obj;
            return (cellPosition.m_cellX == m_cellX && cellPosition.m_cellY == m_cellY);
        }

        public static bool operator ==(CellPosition obj, CellPosition obj2)
        {
            return obj.CellX == obj2.CellX && obj.CellY == obj2.CellY;        
        }

        public static bool operator !=(CellPosition obj, CellPosition obj2)
        {
            return obj.CellX != obj2.CellX || obj.CellY != obj2.CellY;        
        }

        public override int GetHashCode()
        {
            return m_cellX * Constants.REGION_SIZE_Y + m_cellY;
        }

        private readonly int m_cellX;
        private readonly int m_cellY;
    }
}


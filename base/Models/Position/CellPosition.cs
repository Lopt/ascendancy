using System;

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

        public override int GetHashCode()
        {
            return m_cellX * Constants.REGION_SIZE_Y + m_cellY;
        }

        private readonly int m_cellX;
        private readonly int m_cellY;
    }
}


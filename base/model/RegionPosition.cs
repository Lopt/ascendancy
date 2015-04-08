using System;

namespace @base.model
{
    public class RegionPosition : Object
    {
        public RegionPosition(int regionX, int regionY)
        {
            m_regionX = regionX;
            m_regionY = regionY;
        }

        public Position AsPosition()
        {
            Position(m_regionX * Constants.regionSizeX, m_regionY * Constants.regionSizeY);
        }

        public int RegionX
        {
            get { return m_regionX; }
        }

        public int RegionY
        {
            get { return m_regionY; }
        }

        public override bool Equals(RegionPosition obj)
        {
            return (obj.m_regionX == m_regionX && obj.m_regionY == m_regionY);
        }

        public override int GetHashCode()
        {
            return m_regionX * Constants.regionSizeY + m_regionY;
        }

        private readonly int m_regionX;
        private readonly int m_regionY;
    }
}


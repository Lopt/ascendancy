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

        public RegionPosition(Position position)
        {
            m_regionX = (int) (position.X / Constants.regionSizeX);
            m_regionY = (int) (position.Y / Constants.regionSizeY);
        }

        public int RegionX
        {
            get { return m_regionX; }
        }

        public int RegionY
        {
            get { return m_regionY; }
        }

        public override bool Equals(Object obj)
        {
            var regionPosition = (RegionPosition) obj;
            return (regionPosition.m_regionX == m_regionX && regionPosition.m_regionY == m_regionY);
        }

        public override int GetHashCode()
        {
            return m_regionX * Constants.regionSizeY + m_regionY;
        }

        private readonly int m_regionX;
        private readonly int m_regionY;
    }
}


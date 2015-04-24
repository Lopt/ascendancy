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
            m_regionX = (int)(position.X / Constants.REGION_SIZE_X);
            m_regionY = (int)(position.Y / Constants.REGION_SIZE_Y);
        }

        public int RegionX
        {
            get { return m_regionX; }
        }

        public int RegionY
        {
            get { return m_regionY; }
        }

        public int MajorX
        {
            get { return m_regionX / Constants.MAJOR_REGION_SIZE_X; }
        }

        public int MajorY
        {
            get { return m_regionY / Constants.MAJOR_REGION_SIZE_Y; }
        }


        public override bool Equals(Object obj)
        {
            var regionPosition = (RegionPosition)obj;
            return (regionPosition.m_regionX == m_regionX && regionPosition.m_regionY == m_regionY);
        }

        public override int GetHashCode()
        {
            return m_regionX * m_regionY;
        }

        private readonly int m_regionX;
        private readonly int m_regionY;
    }
}


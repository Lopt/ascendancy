using System;
using Newtonsoft.Json;

namespace @base.model
{
	public class Position
	{
        [JsonConstructor]
        public Position (double x, double y)
		{
			m_x = x;
			m_y = y;
		}

        public Position(LatLon latLon)
        {
            var zoom = Constants.EARTH_CIRCUMFERENCE / Constants.CELL_SIZE;
            m_x = (float)((latLon.Lon + 180.0) / 360.0 * zoom);
            m_y = (float)((1.0 - Math.Log(Math.Tan(latLon.Lat * Math.PI / 180.0) +
                1.0 / Math.Cos(latLon.Lat * Math.PI / 180.0)) / Math.PI) / 2.0 * zoom);
        }

        public Position(RegionPosition regionPosition, CellPosition cellPosition)
        {
            m_x = regionPosition.RegionX * Constants.REGION_SIZE_X + cellPosition.CellX;
            m_y = regionPosition.RegionY * Constants.REGION_SIZE_Y + cellPosition.CellY;
        }

		public double X
		{
			get { return this.m_x; }
		}

        public double Y
		{
			get { return this.m_y; }
		}

        private readonly double m_x;
        private readonly double m_y;
	}
}


using System;

namespace @base.model
{
	public class LatLon
	{
		public LatLon (double lat, double lon)
		{
            m_lat = lat;
            m_lon = lon;
		}

        public LatLon (Position position)
        {
            var zoom = Constants.EARTHCIRCUMFERENCE / Constants.CELLSIZE;
            var n = Math.PI - ((2.0 * Math.PI * position.Y) / zoom);

            m_lon = (float)((position.X / zoom * 360.0) - 180.0);
            m_lat = (float)(180.0 / Math.PI * Math.Atan(Math.Sinh(n)));
        }

		public double Lat
		{
            //set { this.m_lat = value; }
            get { return this.m_lat; }
		}

		public double Lon
		{
            //set { this.m_lon = value; }
            get { return this.m_lon; }
		}

		private double m_lat;
		private double m_lon;
	}
}


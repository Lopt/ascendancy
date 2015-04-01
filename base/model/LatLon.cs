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

		public Position AsPosition ()
		{
			var zoom = 15;
			var x = (float)((lon + 180.0) / 360.0 * (1 << zoom));
			var y = (float)((1.0 - Math.Log(Math.Tan(lat * Math.PI / 180.0) + 
				1.0 / Math.Cos(lat * Math.PI / 180.0)) / Math.PI) / 2.0 * (1 << zoom));

			return new CPosition (x, y);
		}

		public double Lat
		{
            set { this.m_lat = value; }
            get { return this.m_lat; }
		}

		public double Lon
		{
            set { this.m_lon = value; }
            get { return this.m_lon; }
		}

		private double m_lat;
		private double m_lon;

		private readonly double degreesToRadiansRatio = 180d / Math.PI;
		private readonly double radiansToDegreesRatio = Math.PI / 180d;

	}
}


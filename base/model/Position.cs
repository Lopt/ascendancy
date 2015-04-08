using System;

namespace @base.model
{
	public class Position
	{
		public Position (int x, int y)
		{
			m_x = x;
			m_y = y;
		}

		public Position (LatLon latLon)
		{
			m_x = 0;
			m_y = 0;
		}

		public int AsRegionPosition ()
		{
			return RegionPosition (m_x % Constants.regionSizeX, m_y % Constants.regionSizeY);
		}

		public LatLon AsLatLon ()
        {            
            cell_size = 4;
            // earth range / cell size
            zoom = 40075036 / cell_size;
			var n = Math.PI - ((2.0 * Math.PI * m_y) / Math.Pow(2.0, zoom));

			var lat = (float)((m_x / Math.Pow(2.0, zoom) * 360.0) - 180.0);
			var lon = (float)(180.0 / Math.PI * Math.Atan(Math.Sinh(n)));

			return new LatLon (lat, lon);
		}

		public int X
		{
			get { return this.m_x; }
		}

		public int Y
		{
			get { return this.m_y; }
		}

		private readonly int m_x;
		private readonly int m_y;
	}
}


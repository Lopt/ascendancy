using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace @base.model
{
    public class PositionI
	{
        [JsonConstructor]
        public PositionI (int x, int y)
		{
			m_x = x;
			m_y = y;
		}

        public PositionI (JContainer obj)
        {
            m_x = (int) obj.SelectToken("X");
            m_y = (int) obj.SelectToken("Y");
        }

        public PositionI(LatLon latLon)
        {
            var zoom = Constants.EARTH_CIRCUMFERENCE / Constants.CELL_SIZE;
            m_x = (int)((latLon.Lon + 180.0) / 360.0 * zoom);
            m_y = (int)((1.0 - Math.Log(Math.Tan(latLon.Lat * Math.PI / 180.0) +
                1.0 / Math.Cos(latLon.Lat * Math.PI / 180.0)) / Math.PI) / 2.0 * zoom);
        }

        public PositionI(RegionPosition regionPosition, CellPosition cellPosition)
        {
            m_x = regionPosition.RegionX * Constants.REGION_SIZE_X + cellPosition.CellX;
            m_y = regionPosition.RegionY * Constants.REGION_SIZE_Y + cellPosition.CellY;
        }

        public static PositionI operator +(PositionI first, PositionI second)
        {
            return new PositionI(first.X + second.X, first.Y + second.Y);
        }

        [JsonIgnore]
        public RegionPosition RegionPosition
        {
            get { return new RegionPosition(this); }
        }

        [JsonIgnore]
        public CellPosition CellPosition
        {
            get { return new CellPosition(this); }
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


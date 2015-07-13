using System;

namespace Core.Models
{
    public class LatLon
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="base.model.LatLon"/> class.
        /// A LatLon is an Latitude/Longitude Position of (spheric) earth.
        /// </summary>
        /// <param name="lat">Latitude</param>
        /// <param name="lon">Longitude</param>
        public LatLon(double lat, double lon)
        {
            m_lat = lat;
            m_lon = lon;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="base.model.LatLon"/> class.
        /// </summary>
        /// <param name="position">Position which should be converted to a LatLon Position.</param>
        public LatLon(Position position)
        {
            var zoom = Constants.EARTH_CIRCUMFERENCE / Constants.CELL_SIZE;
            var n = Math.PI - ((2.0 * Math.PI * position.Y) / zoom);

            m_lon = (float)((position.X / zoom * 360.0) - 180.0);
            m_lat = (float)(180.0 / Math.PI * Math.Atan(Math.Sinh(n)));
        }

        public double Lat
        {
            //set { this.m_lat = value; }
            get
            {
                return this.m_lat;
            }
        }

        public double Lon
        {
            //set { this.m_lon = value; }
            get
            {
                return this.m_lon;
            }
        }

        private double m_lat;
        private double m_lon;
    }
}


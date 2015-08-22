namespace Core.Models
{
    using System;

    /// <summary>
    /// Latitude and Longitude of the real world.
    /// </summary>
    public class LatLon
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Core.Models.LatLon"/> class.
        /// Latitude/Longitude Position of earth.
        /// </summary>
        /// <param name="lat">Latitude of earth</param>
        /// <param name="lon">Longitude of earth</param>
        public LatLon(double lat, double lon)
        {
            Lat = lat;
            Lon = lon;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Core.Models.LatLon"/> class.
        /// </summary>
        /// <param name="position">Position which should be converted to a <see cref="Core.Models.LatLon"/>  Position.</param>
        public LatLon(Position position)
        {
            var zoom = Constants.EARTH_CIRCUMFERENCE / Constants.CELL_SIZE;
            var n = Math.PI - ((2.0 * Math.PI * position.Y) / zoom);

            Lon = (float)((position.X / zoom * 360.0) - 180.0);
            Lat = (float)(180.0 / Math.PI * Math.Atan(Math.Sinh(n)));
        }

        /// <summary>
        /// Gets the latitude
        /// </summary>
        /// <value>The latitude.</value>
        public double Lat
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the longitude.
        /// </summary>
        /// <value>The longitude.</value>
        public double Lon
        {
            get;
            private set;
        }
    }
}
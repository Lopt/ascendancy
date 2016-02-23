namespace Core.Models
{
    using System;
    using Newtonsoft.Json;

    /// <summary>
    /// Position in the GameWorld.
    /// </summary>
    public class Position
    {
        /// <summary>
        /// Adds two positions.
        /// </summary>
        /// <returns>Resulted Position</returns>
        /// <param name="first">First Position.</param>
        /// <param name="second">Second Position.</param>
        public static Position operator +(Position first, Position second)
        {
            return new Position(first.X + second.X, first.Y + second.Y);
        }

        /// <summary>
        /// Subtract second position from first position (first-second).
        /// </summary>
        /// <returns>Resulted Position</returns>
        /// <param name="first">First Position.</param>
        /// <param name="second">Second Position.</param>
        public static Position operator -(Position first, Position second)
        {
            return new Position(first.X - second.X, first.Y - second.Y);
        }

        /// <summary>
        /// Tests two positions if they are the same.
        /// </summary>
        /// <returns>boolean if both position are the same. Otherwise false.</returns>
        /// <param name="first">First Position.</param>
        /// <param name="second">Second Position.</param>
        public static bool operator ==(Position first, Position second)
        {
            if (object.ReferenceEquals(first, second))
            {
                return true;
            }

            // If one is null, but not both, return false.
            if (((object)first == null) || ((object)second == null))
            {
                return false;
            }

            return first.X == second.X && first.Y == second.Y;
        }

        /// <summary>
        /// Tests two positions if they are NOT the same.
        /// </summary>
        /// <returns>boolean if both position are NOT the same. Otherwise false.</returns>
        /// <param name="first">First position.</param>
        /// <param name="second">Second position.</param>
        public static bool operator !=(Position first, Position second)
        {
            if (object.ReferenceEquals(first, second))
            {
                return false;
            }

            // If one is null, but not both, return false.
            if (((object)first == null) || ((object)second == null))
            {
                return true;
            }

            return first.X != second.X || first.Y != second.Y;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Core.Models.Position"/> class.
        /// </summary>
        /// <param name="x">The x coordinate.</param>
        /// <param name="y">The y coordinate.</param>
        [JsonConstructor]
        public Position(double x, double y)
        {
            X = x;
            Y = y;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Core.Models.Position"/> class.
        /// </summary>
        /// <param name="position">Position in X and Y.</param>
        public Position(PositionI position)
        {
            X = position.X;
            Y = position.Y;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Core.Models.Position"/> class.
        /// </summary>
        /// <param name="latLon">Latitude longitude.</param>
        public Position(LatLon latLon)
        {
            var zoom = Constants.EARTH_CIRCUMFERENCE / Constants.CELL_SIZE;
            X = (float)((latLon.Lon + 180.0) / 360.0 * zoom);
            Y = (float)((1.0 - Math.Log(Math.Tan((latLon.Lat * Math.PI) / 180.0) +
                1.0 / Math.Cos((latLon.Lat * Math.PI) / 180.0)) / Math.PI) / 2.0 * zoom);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Core.Models.Position"/> class.
        /// </summary>
        /// <param name="regionPosition">Region position.</param>
        public Position(RegionPosition regionPosition)
        {
            X = regionPosition.RegionX * Constants.REGION_SIZE_X;
            Y = regionPosition.RegionY * Constants.REGION_SIZE_Y;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Core.Models.Position"/> class.
        /// </summary>
        /// <param name="regionPosition">Region position.</param>
        /// <param name="cellPosition">Cell position.</param>
        public Position(RegionPosition regionPosition, CellPosition cellPosition)
        {
            X = (regionPosition.RegionX * Constants.REGION_SIZE_X) + cellPosition.CellX;
            Y = (regionPosition.RegionY * Constants.REGION_SIZE_Y) + cellPosition.CellY;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Core.Models.Position"/> class.
        /// </summary>
        /// <param name="regionPositionX">Region position x.</param>
        /// <param name="regionPositionY">Region position y.</param>
        /// <param name="cellPositionX">Cell position x.</param>
        /// <param name="cellPositionY">Cell position y.</param>
        public Position(int regionPositionX, int regionPositionY, float cellPositionX, float cellPositionY)
        {
            X = (regionPositionX * Constants.REGION_SIZE_X) + cellPositionX;
            Y = (regionPositionY * Constants.REGION_SIZE_Y) + cellPositionY;
        }

        /// <summary>
        /// Gets the x.
        /// </summary>
        /// <value>The x.</value>
        public double X
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the y.
        /// </summary>
        /// <value>The y.</value>
        public double Y
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the region position.
        /// </summary>
        /// <value>The region position.</value>
        [JsonIgnore]
        public RegionPosition RegionPosition
        {
            get
            {
                return new RegionPosition(this);
            }
        }

        /// <summary>
        /// Gets the cell position.
        /// </summary>
        /// <value>The cell position.</value>
        [JsonIgnore]
        public CellPosition CellPosition
        {
            get
            {
                return new CellPosition(this);
            }
        }

        /// <summary>
        /// Tests two positions if they are the same.
        /// </summary>
        /// <returns>boolean if both position are the same. Otherwise false.</returns>
        /// <param name="obj">The <see cref="System.Object"/> to compare with the current <see cref="Core.Models.Position"/>.</param>
        public override bool Equals(object obj)
        {
            var pos = (Position)obj;
            return this == pos;
        }

        /// <summary>
        /// Distance from this to the specific position
        /// Warning: NOT ROOTED.
        /// </summary>
        /// <returns>Distance from this to the specific position.</returns>
        /// <param name="position">Other Position.</param>
        public double Distance(Position position)
        {
            var distanceX = position.X - X;
            var distanceY = position.Y - Y;
            return (distanceX * distanceX) + (distanceY * distanceY);
        }

        /// <summary>
        /// Distance from this to the specific position
        /// Warning: NOT ROOTED.
        /// </summary>
        /// <returns>Distance from this to the specific position.</returns>
        /// <param name="position">Other Position.</param>
        public double Distance(PositionI position)
        {
            var distanceX = position.X - X;
            var distanceY = position.Y - Y;
            return (distanceX * distanceX) + (distanceY * distanceY);
        }
    }
}

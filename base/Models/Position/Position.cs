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
        /// Gets the x.
        /// </summary>
        /// <value>The x.</value>
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
        /// Bestimmt, ob das angegebene Objekt mit dem aktuellen Objekt identisch ist.
        /// </summary>
        /// <returns>true, wenn das angegebene Objekt und das aktuelle Objekt gleich sind, andernfalls false.</returns>
        /// <param name="obj">The <see cref="System.Object"/> to compare with the current <see cref="Core.Models.Position"/>.</param>
        /// <filterpriority>2</filterpriority>
        public override bool Equals(Object obj)
        {
            var pos = (Position)obj;
            return this == pos;
        }

        /// <param name="first">First.</param>
        /// <param name="second">Second.</param>
        public static Position operator +(Position first, Position second)
        {
            return new Position(first.X + second.X, first.Y + second.Y);
        }

        /// <param name="first">First.</param>
        /// <param name="second">Second.</param>
        public static Position operator -(Position first, Position second)
        {
            return new Position(first.X - second.X, first.Y - second.Y);
        }

        /// <param name="first">First.</param>
        /// <param name="second">Second.</param>
        public static bool operator == (Position first, Position second)
        {
            if (System.Object.ReferenceEquals(first, second))
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

        /// <param name="first">First.</param>
        /// <param name="second">Second.</param>
        public static bool operator !=(Position first, Position second)
        {
            if (System.Object.ReferenceEquals(first, second))
            {
                return false;
            }

            // If one is null, but not both, return false.
            if (((object)first == null) || ((object)second == null))
            {
                return true;
            }

            return (first.X != second.X || first.Y != second.Y);
        }

        /// <summary>
        /// Distance the specified position.
        /// </summary>
        /// <param name="position">Position.</param>
        public double Distance(Position position)
        {
            var xDistance = (position.X - X);
            var yDistance = (position.Y - Y);
            return xDistance * xDistance + yDistance * yDistance;
        }

        /// <summary>
        /// Distance the specified position.
        /// </summary>
        /// <param name="position">Position.</param>
        public double Distance(PositionI position)
        {
            var xDistance = (position.X - X);
            var yDistance = (position.Y - Y);
            return xDistance * xDistance + yDistance * yDistance;
        }
    }
}


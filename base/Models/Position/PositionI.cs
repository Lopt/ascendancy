namespace Core.Models
{
    using System;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    /// <summary>
    /// Position in the Game world as Integer (so it is discrete).
    /// Can be used to identify one field in the World.
    /// </summary>
    public class PositionI
    {
        /// <summary>
        /// Adds two positions.
        /// </summary>
        /// <returns>Resulted Position</returns>
        /// <param name="first">First Position.</param>
        /// <param name="second">Second Position.</param>
        public static PositionI operator +(PositionI first, PositionI second)
        {
            return new PositionI(first.X + second.X, first.Y + second.Y);
        }

        /// <summary>
        /// Subtract second position from first position (first-second).
        /// </summary>
        /// <returns>Resulted Position</returns>
        /// <param name="first">First Position.</param>
        /// <param name="second">Second Position.</param>
        public static PositionI operator -(PositionI first, PositionI second)
        {
            return new PositionI(first.X - second.X, first.Y - second.Y);
        }

        /// <summary>
        /// Tests two positions if they are the same.
        /// </summary>
        /// <returns>boolean if both position are the same. Otherwise false.</returns>
        /// <param name="first">First Position.</param>
        /// <param name="second">Second Position.</param>
        public static bool operator ==(PositionI first, PositionI second)
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
        public static bool operator !=(PositionI first, PositionI second)
        {
            return !(first == second);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Core.Models.PositionI"/> class.
        /// </summary>
        /// <param name="x">The x coordinate.</param>
        /// <param name="y">The y coordinate.</param>
        [JsonConstructor]
        public PositionI(int x, int y)
        {
            X = x;
            Y = y;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Core.Models.PositionI"/> class.
        /// </summary>
        /// <param name="obj">JSON Object.</param>
        public PositionI(JContainer obj)
        {
            X = (int)obj.SelectToken("X");
            Y = (int)obj.SelectToken("Y");
        }
            
        /// <summary>
        /// Initializes a new instance of the <see cref="Core.Models.PositionI"/> class.
        /// </summary>
        /// <param name="regionPosition">Region position.</param>
        /// <param name="cellPosition">Cell position.</param>
        public PositionI(RegionPosition regionPosition, CellPosition cellPosition)
        {
            X = (regionPosition.RegionX * Constants.REGION_SIZE_X) + cellPosition.CellX;
            Y = (regionPosition.RegionY * Constants.REGION_SIZE_Y) + cellPosition.CellY;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Core.Models.PositionI"/> class.
        /// </summary>
        /// <param name="position">Position which should be copied.</param>
        public PositionI(Position position)
        {
            X = (int)position.X;
            Y = (int)position.Y;
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
        /// Gets the x.
        /// </summary>
        /// <value>The x.</value>
        public int X
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the y.
        /// </summary>
        /// <value>The y.</value>
        public int Y
        {
            get;
            private set;
        }

        /// <summary>
        /// Standard hash function
        /// </summary>
        /// <returns>Hash code.</returns>
        public override int GetHashCode()
        {
            return (X * 1000000) + Y;
        }

        /// <summary>
        /// tests if the given object is equal to this object.
        /// </summary>
        /// <returns>true, if both objects are equal, otherwise false.</returns>
        /// <param name="obj">The <see cref="System.Object"/> to compare with the current <see cref="Core.Models.PositionI"/>.</param>
        public override bool Equals(object obj)
        {
            var pos = (PositionI)obj;
            return this == pos;
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
    }
}

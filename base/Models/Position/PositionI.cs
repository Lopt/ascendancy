namespace Core.Models
{
    using System;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    /// <summary>
    /// Position in the Gameworld as Integer (so it is discrete).
    /// Can be used to identify one field in the World.
    /// </summary>
    public class PositionI
    {
        /// <summary>
        /// Adds one positionI to another positionI.
        /// </summary>
        /// <returns>The new positionI.</returns>
        /// <param name="first">First positionI.</param>
        /// <param name="second">Second positionI.</param>
        public static PositionI operator +(PositionI first, PositionI second)
        {
            return new PositionI(first.X + second.X, first.Y + second.Y);
        }

        /// <summary>
        /// Minus one positionI to another positionI.
        /// </summary>
        /// <returns>The new positionI.</returns>
        /// <param name="first">First positionI.</param>
        /// <param name="second">Second positionI.</param>
        public static PositionI operator -(PositionI first, PositionI second)
        {
            return new PositionI(first.X - second.X, first.Y - second.Y);
        }

        /// <summary>
        /// Tests if the first positionI is equal to the second positionI.
        /// </summary>
        /// <returns>true, if it is equal, otherwise false.</returns>
        /// <param name="first">First positionI.</param>
        /// <param name="second">Second positionI.</param>
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
        /// Tests if the first positionI is unequal to the second positionI.
        /// </summary>
        /// <returns>true, if it is equal, otherwise false.</returns>
        /// <param name="first">First positionI.</param>
        /// <param name="second">Second positionI.</param>
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
        /// <param name="obj">The JContainer object.</param>
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
        /// <param name="position">The position.</param>
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
        /// Serves as a hash function for a <see cref="Core.Models.PositionI"/> object.
        /// </summary>
        /// <returns>A hash code for this instance that is suitable for use in hashing algorithms and data structures such as a
        /// hash table.</returns>
        public override int GetHashCode()
        {
            return (X * 1000000) + Y;
        }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object"/> is equal to the current <see cref="Core.Models.PositionI"/>.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object"/> to compare with the current <see cref="Core.Models.PositionI"/>.</param>
        /// <returns><c>true</c> if the specified <see cref="System.Object"/> is equal to the current
        /// <see cref="Core.Models.PositionI"/>; otherwise, <c>false</c>.</returns>
        public override bool Equals(object obj)
        {
            var pos = (PositionI)obj;
            return this == pos;
        }

        /// <summary>
        /// Sqare distance to the specified positionI.
        /// </summary>
        /// <returns>>The Square distance</returns>
        /// <param name="position">The positionI.</param>
        public double Distance(PositionI position)
        {
            var distanceX = position.X - X;
            var distanceY = position.Y - Y;
            return (distanceX * distanceX) + (distanceY * distanceY);
        }

        /// <summary>
        /// Sqare distance to the specified position.
        /// </summary>
        /// <returns>>The Square distance</returns>
        /// <param name="position">The position.</param>
        public double Distance(Position position)
        {
            var distanceX = position.X - X;
            var distanceY = position.Y - Y;
            return (distanceX * distanceX) + (distanceY * distanceY);
        }
    }
}

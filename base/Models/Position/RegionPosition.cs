namespace Core.Models
{
    using System;
    using System.Collections.Concurrent;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    /// <summary>
    /// Represents a position of a region.
    /// </summary>
    public class RegionPosition : object
    {
        /// <summary>
        /// Tests if the first region position is equal to the second region position.
        /// </summary>
        /// <returns>true, if it is equal, otherwise false.</returns>
        /// <param name="first">First region position.</param>
        /// <param name="second">Second region position.</param>
        public static bool operator ==(RegionPosition first, RegionPosition second)
        {
            if (object.ReferenceEquals(first, second))
            {
                return true;
            }

            // If one is null, but not both, return false.
            if (((object)first == null) || ((object)second == null))
            {
                return first == second;
            }

            return first.RegionX == second.RegionX && first.RegionY == second.RegionY;
        }

        /// <summary>
        /// Tests if the first region position is unequal to the second region position.
        /// </summary>
        /// <returns>true, if it is equal, otherwise false.</returns>
        /// <param name="first">First region position.</param>
        /// <param name="second">Second region position.</param>
        public static bool operator !=(RegionPosition first, RegionPosition second)
        {
            return !(first == second);
        }

        /// <summary>
        /// Adds one region position to another region position
        /// </summary>
        /// <returns>The new region position.</returns>
        /// <param name="first">First region position.</param>
        /// <param name="second">Second region position.</param>
        public static RegionPosition operator +(RegionPosition first, RegionPosition second)
        {
            return new RegionPosition(first.RegionX + second.RegionX, first.RegionY + second.RegionY);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Core.Models.RegionPosition"/> class.
        /// </summary>
        /// <param name="regionX">Region x.</param>
        /// <param name="regionY">Region y.</param>
        [JsonConstructor]
        public RegionPosition(int regionX, int regionY)
        {
            RegionX = regionX;
            RegionY = regionY;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Core.Models.RegionPosition"/> class.
        /// </summary>
        /// <param name="position">Game Position.</param>
        public RegionPosition(Position position)
        {
            RegionX = (int)(position.X / Constants.REGION_SIZE_X);
            RegionY = (int)(position.Y / Constants.REGION_SIZE_Y);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Core.Models.RegionPosition"/> class.
        /// </summary>
        /// <param name="position">Game Position Integer.</param>
        public RegionPosition(PositionI position)
        {
            RegionX = (int)(position.X / Constants.REGION_SIZE_X);
            RegionY = (int)(position.Y / Constants.REGION_SIZE_Y);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Core.Models.RegionPosition"/> class.
        /// </summary>
        /// <param name="obj">JContainer Object.</param>
        public RegionPosition(JContainer obj)
        {
            RegionX = (int)obj.SelectToken("RegionX");
            RegionY = (int)obj.SelectToken("RegionY");
        }

        /// <summary>
        /// Gets the region x.
        /// </summary>
        /// <value>The region x.</value>
        public int RegionX
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the region y.
        /// </summary>
        /// <value>The region y.</value>
        public int RegionY
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the major x.
        /// </summary>
        /// <value>The major x.</value>
        [JsonIgnore]
        public int MajorX
        {
            get
            {
                return RegionX / Constants.MAJOR_REGION_SIZE_X;
            }
        }

        /// <summary>
        /// Gets the major y.
        /// </summary>
        /// <value>The major y.</value>
        [JsonIgnore]
        public int MajorY
        {
            get
            {
                return RegionY / Constants.MAJOR_REGION_SIZE_Y;
            }
        }

        /// <summary>
        /// tests if the given object is equal to this object
        /// </summary>
        /// <returns>true, if it is equal, otherwise false.</returns>
        /// <param name="obj">The <see cref="System.Object"/> to compare with the current <see cref="Core.Models.RegionPosition"/>.</param>
        /// <filterpriority>2</filterpriority>
        public override bool Equals(object obj)
        {
            var regionPosition = (RegionPosition)obj;
            return this == regionPosition;
        }

        /// <summary>
        /// standard hash function
        /// </summary>
        /// <returns>hash code.</returns>
        /// <filterpriority>2</filterpriority>
        public override int GetHashCode()
        {
            return (RegionX * 1000000) + RegionY;
        }
    }
}
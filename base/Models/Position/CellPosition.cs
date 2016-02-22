namespace Core.Models
{
    using System;
    using System.Collections.Concurrent;
    using Newtonsoft.Json.Linq;

    /// <summary>
    /// Represents the cellPosition in a region. Can be only as large as the region.
    /// </summary>
    public class CellPosition : object
    {
        /// <summary>
        /// tests if two objects are equal
        /// </summary>
        /// <param name="obj">First Object.</param>
        /// <param name="obj2">Second Object.</param>
        /// <returns>true if both objects are equal</returns>
        public static bool operator ==(CellPosition obj, CellPosition obj2)
        {
            return obj.CellX == obj2.CellX && obj.CellY == obj2.CellY;        
        }

        /// <summary>
        /// tests if two objects are unequal
        /// </summary>
        /// <param name="obj">First Object.</param>
        /// <param name="obj2">Second Object.</param>
        /// <returns>true if both objects are unequal</returns>
        public static bool operator !=(CellPosition obj, CellPosition obj2)
        {
            return obj.CellX != obj2.CellX || obj.CellY != obj2.CellY;        
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Core.Models.CellPosition"/> class.
        /// </summary>
        /// <param name="cellX">Cell X Position.</param>
        /// <param name="cellY">Cell Y Position.</param>
        public CellPosition(int cellX, int cellY)
        {
            // if the number is to big (>= RegionSize or to small (<= - RegionSize), normalize it
            // if the number is negative (e.g. -1) then add the RegionSize (-1 + 32 = 31) and then normalize it again
            // normalizing it again is needed, if the number is positive (1 + 32 = 33 -> which is cell 1) 
            CellX = (int)(Constants.REGION_SIZE_X + (cellX % Constants.REGION_SIZE_X)) % Constants.REGION_SIZE_X;
            CellY = (int)(Constants.REGION_SIZE_Y + (cellY % Constants.REGION_SIZE_Y)) % Constants.REGION_SIZE_Y;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Core.Models.CellPosition"/> class.
        /// </summary>
        /// <param name="position">GameWorld Position.</param>
        public CellPosition(Position position)
            : this(
                (int)position.X, 
                (int)position.Y)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Core.Models.CellPosition"/> class.
        /// </summary>
        /// <param name="position">GameWorld Integer Position.</param>
        public CellPosition(PositionI position)
            : this(
                position.X,
                position.Y)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Core.Models.CellPosition"/> class.
        /// </summary>
        /// <param name="obj">JContainer Object.</param>
        public CellPosition(JContainer obj)
        : this(
            (int)obj.SelectToken("CellX"),
            (int)obj.SelectToken("CellY"))
        {
        }

        /// <summary>
        /// Gets the cell x.
        /// </summary>
        /// <value>The cell x.</value>
        public int CellX
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the cell y.
        /// </summary>
        /// <value>The cell y.</value>
        public int CellY
        {
            get;
            private set;
        }

        /// <summary>
        /// tests if the given object is equal to this object
        /// </summary>
        /// <returns>true, if it is equal, otherwise false.</returns>
        /// <param name="obj">The <see cref="System.Object"/> to compare with the current <see cref="Core.Models.CellPosition"/>.</param>
        /// <filterpriority>2</filterpriority>
        public override bool Equals(object obj)
        {
            var cellPosition = (CellPosition)obj;
            return cellPosition.CellX == CellX && cellPosition.CellY == CellY;
        }

        /// <summary>
        /// standard hash function
        /// </summary>
        /// <returns>hash code of object.</returns>
        /// <filterpriority>2</filterpriority>
        public override int GetHashCode()
        {
            return (CellX * Constants.REGION_SIZE_Y) + CellY;
        }
    }
}
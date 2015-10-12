namespace Server.DB.Models
{
    using System;
    using SQLite;

    /// <summary>
    /// Table building.
    /// </summary>
    [Table("Building")]
    public class TableBuilding
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        [PrimaryKey, Column("Id")]
        public int Id { get; set; }     

        /// <summary>
        /// Gets or sets the position x.
        /// </summary>
        /// <value>The position x.</value> 
        public int PositionX { get; set; }  

        /// <summary>
        /// Gets or sets the position y.
        /// </summary>
        /// <value>The position y.</value>
        public int PositionY { get; set; }
    }
}

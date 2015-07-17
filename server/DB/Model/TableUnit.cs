using System;
using SQLite;

namespace Server.DB.Models
{
    /// <summary>
    /// Represantation from the databank table Unit. 
    /// </summary>
    [Table("Unit")]
    class TableUnit
    {
        /// <summary>
        /// Set and get for column Id, as primary key.
        /// </summary>
        [PrimaryKey, Column("Id")]
        public int Id { get; set; }
        /// <summary>
        /// Set and get for column PositionX.
        /// </summary>
        public int PositionX { get; set; }
        /// <summary>
        /// Set and get for column PositionY.
        /// </summary> 
        public int PositionY { get; set; }
    }
}

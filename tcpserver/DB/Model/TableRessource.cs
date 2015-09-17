using System;
using SQLite;

namespace Server.DB.Models
{
    /// <summary>
    /// Represantation from the databank table Ressource. 
    /// </summary>
    [Table("Ressources")]
    class TableRessource
    {
        /// <summary>
        /// Set and get for column Id, as primary key.
        /// </summary>
        [PrimaryKey, Column("Id")]
        public int Id { get; set; }
        /// <summary>
        /// Set and get for column Fire.
        /// </summary>
        public int Fire { get; set; }
        /// <summary>
        /// Set and get for column Earth.
        /// </summary>
        public int Earth { get; set; }
        /// <summary>
        /// Set and get for column Water.
        /// </summary>
        public int Water { get; set; }
        /// <summary>
        /// Set and get for column Air.
        /// </summary>
        public int Air { get; set; }
        /// <summary>
        /// Set and get for column Magic.
        /// </summary>
        public int Magic { get; set; }
        /// <summary>
        /// Set and get for column Gold.
        /// </summary>
        public int Gold { get; set; }
    }
}

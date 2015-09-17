using System;
using SQLite;

namespace Server.DB.Models
{
    /// <summary>
    /// Represantation from the databank table Account. 
    /// </summary>
    [Table("Account")]
    class TableAccount
    {
        /// <summary>
        /// Set and get for column id, as primary key.
        /// </summary>
        [PrimaryKey, Column("Id")]
        public int Id{ get; set;}

        /// <summary>
        /// Set and get for column username, maximal length is 25 signs.
        /// </summary>
        [MaxLength(25), Unique]
        public string UserName{ get; set; }

        /// <summary>
        /// Set and get for column password, maximal length is 50 signs.
        /// </summary>
        [MaxLength(50)]
        public string Password { get; set; }

        /// <summary>
        /// Set and get for column salt, for verifieng the user password. Maximal length is 50.
        /// </summary>
        [MaxLength(50)]
        public string Salt { get; set; }
    }
}

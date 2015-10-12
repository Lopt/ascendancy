namespace Server.DB.Models
{
    using System;
    using SQLite;

    /// <summary>
    /// Representation  from the databank table Account. 
    /// </summary>
    [Table("Account")]
    public class TableAccount
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        [PrimaryKey, Column("Id")]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the name of the user.
        /// </summary>
        /// <value>The name of the user.</value>
        [MaxLength(25), Unique]
        public string UserName { get; set; }

        /// <summary>
        /// Gets or sets the password.
        /// </summary>
        /// <value>The password.</value>
        [MaxLength(50)]
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets the salt.
        /// </summary>
        /// <value>The salt.</value>
        [MaxLength(50)]
        public string Salt { get; set; }
    }
}

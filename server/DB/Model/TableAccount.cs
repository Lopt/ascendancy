using System;
using SQLite;

namespace server.DB.Model
{
    [Table("Account")]
    class TableAccount
    {
        [PrimaryKey, AutoIncrement, Column("Id")]
        public int Id{ get; set;}

        [MaxLength(20), Unique]
        public string UserName{ get; set; }

        [MaxLength(50)]
        public string Password { get; set; }

        [MaxLength(50)]
        public string Salt { get; set; }
    }
}

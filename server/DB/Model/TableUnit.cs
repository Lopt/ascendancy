using System;
using SQLite;

namespace server.DB.Model
{
    [Table("Unit")]
    class TableUnit
    {
        [PrimaryKey, Column("Id")]
        public int Id { get; set; }

        public int PositionX { get; set; }
        public int PositionY { get; set; }
    }
}

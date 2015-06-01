using System;
using SQLite;

namespace server.DB.Model
{
    [Table("Building")]
    class TableBuilding
    {
        [PrimaryKey, Column("Id")]
        public int Id { get; set; }
                
        public int PositionX { get; set; }               
        public int PositionY { get; set; }
    }
}

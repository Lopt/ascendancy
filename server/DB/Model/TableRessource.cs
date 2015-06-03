using System;
using SQLite;

namespace server.DB.Model
{
    [Table("Ressources")]
    class TableRessource
    {
        [PrimaryKey, Column("Id")]
        public int Id { get; set; }

        public int Fire { get; set; }
        public int Earth { get; set; }
        public int Water { get; set; }
        public int Air { get; set; }
        public int Magic { get; set; }
        public int Gold { get; set; }
    }
}

namespace Server.DB
{
    using System;
    using Core.Models;
    using Server.DB.Models;
    using Server.Models;
    using SQLite;

    /// <summary>
    /// DB units.
    /// </summary>
    public class DBUnits
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Server.DB.DBUnits"/> class.
        /// </summary>
        /// <param name="con">Connection to the DB.</param>
        public DBUnits(SQLiteConnection con)           
        {
            db = con;
            db.CreateTable<TableUnit>();
        }

        /// <summary>
        /// News the unit.
        /// </summary>
        /// <param name="unitEntity">Unit entity.</param>
        /// <param name="id">Identifier for the account.</param>
        public void NewUnit(Entity unitEntity, int id)
        {
                var newData = new TableUnit();
                newData.Id = id;
                newData.PositionX = unitEntity.Position.X;
                newData.PositionY = unitEntity.Position.Y;

                db.InsertOrReplace(newData);
        }

        /// <summary>
        /// The DB.
        /// </summary>
        private SQLiteConnection db;
    }
}

namespace Server.DB
{
    using System;
    using Core.Models;
    using Server.DB.Models;
    using SQLite;

    /// <summary>
    /// DB buildings.
    /// </summary>
    public class DBBuildings
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Server.DB.DBBuildings"/> class.
        /// </summary>
        /// <param name="con">Connection to the DB.</param>
        public DBBuildings(SQLiteConnection con)
        {
            db = con;
            db.CreateTable<TableBuilding>();
        }

        /// <summary>
        /// Insert a building into the database, with the current position.
        /// </summary>
        /// <param name="buildingEntity">Building entity.</param>
        /// <param name="id">Identifier for the entry.</param>
        public void NewBuildings(Entity buildingEntity, int id)
        {
            var newData = new TableBuilding();
            newData.Id = id;
            newData.PositionX = buildingEntity.Position.X;
            newData.PositionY = buildingEntity.Position.Y;

            db.InsertOrReplace(newData);           
        }

        /// <summary>
        /// The DB.
        /// </summary>
        private SQLiteConnection db;
    }
}

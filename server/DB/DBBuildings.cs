using System;
using SQLite;
using Core.Models;
using @server.DB.Model;

namespace server.DB
{
	/// <summary>
	/// DB buildings.
	/// </summary>
    class DBBuildings
    {
		/// <summary>
		/// Initializes a new instance of the <see cref="server.DB.DBBuildings"/> class.
		/// </summary>
		/// <param name="con">Con.</param>
        public DBBuildings(SQLiteConnection con)           
        {
            m_db = con;
            m_db.CreateTable<TableBuilding>();
        }
		/// <summary>
		/// Insert a building into the database, with the current position.
		/// </summary>
		/// <param name="buildingEntity">Building entity.</param>
		/// <param name="id">Identifier.</param>
        public void NewBuildings(Entity buildingEntity, int id)
        {
                var newData = new TableBuilding();
                newData.Id = id;
                newData.PositionX = buildingEntity.Position.X;
                newData.PositionY = buildingEntity.Position.Y;

                m_db.InsertOrReplace(newData);
           
        }

        private SQLiteConnection m_db;   
    }
}

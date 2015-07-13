using Core.Models;
using SQLite;
using System;
using @server.model;
using @server.DB.Model;


namespace server.DB
{
	/// <summary>
	/// DB units.
	/// </summary>
    class DBUnits
    {
		/// <summary>
		/// Initializes a new instance of the <see cref="server.DB.DBUnits"/> class.
		/// </summary>
		/// <param name="con">Con.</param>
        public DBUnits(SQLiteConnection con)           
        {
            m_db = con;
            m_db.CreateTable<TableUnit>();
        }
		/// <summary>
		/// Insert a new unit into the databank.
		/// </summary>
		/// <param name="unitEntity">Unit entity.</param>
		/// <param name="id">Identifier.</param>
        public void NewUnit(Entity unitEntity, int id)
        {
                var newData = new TableUnit();
                newData.Id = id;
                newData.PositionX = unitEntity.Position.X;
                newData.PositionY = unitEntity.Position.Y;

                m_db.InsertOrReplace(newData);
        }

        private SQLiteConnection m_db;

    }
}

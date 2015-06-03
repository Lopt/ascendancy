using @base.model;
using SQLite;
using System;
using @server.model;
using @server.DB.Model;


namespace server.DB
{
    class DBUnits
    {
        public DBUnits(SQLiteConnection con)           
        {
            m_db = con;
            m_db.CreateTable<TableUnit>();
        }

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

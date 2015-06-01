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
            if (m_db.Table<TableUnit>().Count() == 0)
            {
                var newData = new TableUnit();
                newData.ID = id;
                newData.PositionX = unitEntity.Position.X;
                newData.PositionY = unitEntity.Position.Y;               
            }

        }

        private SQLiteConnection m_db;

    }
}

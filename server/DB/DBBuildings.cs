using System;
using SQLite;
using @base.model;
using @server.DB.Model;

namespace server.DB
{
    class DBBuildings
    {

        public DBBuildings(SQLiteConnection con)           
        {
            m_db = con;
            m_db.CreateTable<TableBuilding>();
        }

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

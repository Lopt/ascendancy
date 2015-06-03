using System;
<<<<<<< HEAD
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
=======
using SQLite;
using @base.model;
using @server.DB.Model;
>>>>>>> d741c7fafad5bbf531fdb127801a8575f7c0add4

namespace server.DB
{
    class DBBuildings
    {
<<<<<<< HEAD
=======

        public DBBuildings(SQLiteConnection con)           
        {
            m_db = con;
            m_db.CreateTable<TableBuilding>();
        }

        public void NewBuildings(Entity buildingEntity, int id)
        {

            if (m_db.Table<TableBuilding>().Count() == 0)
            {
                var newData = new TableBuilding();
                newData.Id = id;
                newData.PositionX = buildingEntity.Position.X;
                newData.PositionY = buildingEntity.Position.Y;               
            }
        }

        private SQLiteConnection m_db;   
>>>>>>> d741c7fafad5bbf531fdb127801a8575f7c0add4
    }
}

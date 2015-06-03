using @base.model;
using SQLite;
using System;
<<<<<<< HEAD
using System.Collections.Generic;
=======
using @server.model;
using @server.DB.Model;
>>>>>>> d741c7fafad5bbf531fdb127801a8575f7c0add4


namespace server.DB
{
    class DBUnits
    {
<<<<<<< HEAD
        public void TableUnits(Entity unitEntity)
        {
            var db = new SQLiteConnection(model.ServerConstants.DB_PATH);

            db.CreateCommand("CREATE TABLE IF NOT EXISTS [Units] ([Id] INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL, [PositonX] INTEGER NOT NULL, [PositonY] INTEGER NOT NULL, CONSTRAINT [PK_ContentItems] PRIMARY KEY ([Id]))");

            

            db.CreateCommand("INSERT INTO [Units] (PositionX,PositonY) VALUES('')");

        }

=======
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

>>>>>>> d741c7fafad5bbf531fdb127801a8575f7c0add4
    }
}

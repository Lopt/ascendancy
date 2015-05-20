using @base.model;
using SQLite;
using System;
using System.Collections.Generic;


namespace server.DB
{
    class DBUnits
    {
        public void TableUnits(Entity unitEntity)
        {
            var db = new SQLiteConnection(model.ServerConstants.DB_PATH);

            db.CreateCommand("CREATE TABLE IF NOT EXISTS [Units] ([Id] INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL, [PositonX] INTEGER NOT NULL, [PositonY] INTEGER NOT NULL, CONSTRAINT [PK_ContentItems] PRIMARY KEY ([Id]))");

            

            db.CreateCommand("INSERT INTO [Units] (PositionX,PositonY) VALUES('')");

        }

    }
}

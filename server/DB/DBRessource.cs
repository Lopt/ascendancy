using System;
using SQLite;
using @server.DB.Model;

namespace server.DB
{
    class DBRessource
    {
        public DBRessource(SQLiteConnection con)           
        {
            m_db = con;
            m_db.CreateTable<TableRessource>();
        }

        public void TableRessource(int ressourceFire, int ressourceEarth, int ressourceWater, int ressourceAir, int ressourceMagic, int ressourceGold, int id)
        {
                var newData = new TableRessource();
                newData.Id = id;
                newData.Fire = ressourceFire;
                newData.Earth = ressourceEarth;
                newData.Water = ressourceWater;
                newData.Air = ressourceAir;
                newData.Gold = ressourceGold;
                newData.Magic = ressourceMagic;

                m_db.InsertOrReplace(newData);    
        }

        private SQLiteConnection m_db;
    }
}

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

            if (m_db.Table<TableRessource>().Count() == 0)
            {
                var newData = new TableRessource();
                newData.ID = id;
                newData.Fire = ressourceFire;
                newData.Earth = ressourceEarth;
                newData.Water = ressourceWater;
                newData.Air = ressourceAir;
                newData.Gold = ressourceGold;
                newData.Magic = ressourceMagic;
            }

        }

        private SQLiteConnection m_db;
    }
}

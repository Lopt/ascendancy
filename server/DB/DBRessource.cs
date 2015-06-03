using System;
<<<<<<< HEAD
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
=======
using SQLite;
using @server.DB.Model;
>>>>>>> d741c7fafad5bbf531fdb127801a8575f7c0add4

namespace server.DB
{
    class DBRessource
    {
<<<<<<< HEAD
=======
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
>>>>>>> d741c7fafad5bbf531fdb127801a8575f7c0add4
    }
}

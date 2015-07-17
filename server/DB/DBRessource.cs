using System;
using SQLite;
using Server.DB.Models;

namespace Server.DB
{
	/// <summary>
	/// DB ressource.
	/// </summary>
    class DBRessource
    {
		/// <summary>
		/// Initializes a new instance of the <see cref="server.DB.DBRessource"/> class.
		/// </summary>
		/// <param name="con">Con.</param>
        public DBRessource(SQLiteConnection con)           
        {
            m_db = con;
            m_db.CreateTable<TableRessource>();
        }
		/// <summary>
		/// Insert all ressources into the databank.
		/// </summary>
		/// <param name="ressourceFire">Ressource fire.</param>
		/// <param name="ressourceEarth">Ressource earth.</param>
		/// <param name="ressourceWater">Ressource water.</param>
		/// <param name="ressourceAir">Ressource air.</param>
		/// <param name="ressourceMagic">Ressource magic.</param>
		/// <param name="ressourceGold">Ressource gold.</param>
		/// <param name="id">Identifier.</param>
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

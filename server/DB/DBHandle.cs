using @base.model;
using SQLite;
using System;
using System.Collections.Generic;
using server.model;
using System.Collections;
using server.DB.Model;

namespace server.DB
{
	/// <summary>
	/// DB handle.
	/// </summary>
    public class DBHandle
    {
       private static DBHandle instance;

       private DBHandle() { }

	   /// <summary>
	   /// Gets the instance.
	   /// </summary>
	   /// <value>The instance.</value>
       public static DBHandle Instance
       {
           get
           {
               if (instance == null)
               {
                   instance = new DBHandle();                                   
               }
               return instance;
           }
       }
		/// <summary>
		/// Creates the new DB account.
		/// </summary>
		/// <param name="account">Account.</param>
	   	/// <param name="password">Password.</param>
       public void CreateNewDBAccount(Account account, string password)
       {
           SQLiteConnection con = new SQLiteConnection(ServerConstants.DB_PATH);  

           DBAccount dbacc = new DBAccount(con);

           dbacc.CreateAccount(account, password);
       }
		/// <summary>
		/// Gets the account data via DB login.
		/// </summary>
		/// <returns>The account data via DB login.</returns>
		/// <param name="account">Account.</param>
		/// <param name="password">Password.</param>
       public TableData GetAccountDataViaDBLogin(Account account, string password)
       {
           SQLiteConnection con = new SQLiteConnection(ServerConstants.DB_PATH);  

           DBAccount dbacc = new DBAccount(con);
           TableData tb = new TableData();

           if (dbacc.Login(account.UserName, password))
           {
               var data = con.Query<TableAccount>("SELECT Id FROM Account WHERE UserName = ? LIMIT 1", account.UserName);
               tb.Id = data[0].Id;
               tb.m_units = con.Query<TableUnit>("SELECT * FROM Unit WHERE Id = ?", tb.Id);
               tb.m_buildings = con.Query<TableBuilding>("SELECT * FROM Building WHERE Id = ?", tb.Id);
               tb.m_ressources = con.Query<TableRessource>("SELECT * FROM Ressources WHERE Id = ?", tb.Id);               
           }
           
           return tb;
       }
		/// <summary>
		/// Gets the account data via ID.
		/// </summary>
		/// <returns>The account data via ID.</returns>
		/// <param name="Id">Identifier.</param>
       public TableData GetAccountDataViaID(int Id)
       {
           SQLiteConnection con = new SQLiteConnection(ServerConstants.DB_PATH);
                      
           TableData tb = new TableData();
           
           tb.m_units = con.Query<TableUnit>("SELECT * FROM Unit WHERE Id = ?", Id);
           tb.m_buildings = con.Query<TableBuilding>("SELECT * FROM Building WHERE Id = ?", Id);
           tb.m_ressources = con.Query<TableRessource>("SELECT * FROM Ressources WHERE Id = ?", Id);          

           return tb;
       }
		/// <summary>
		/// Inserts the unitv into the databank.
		/// </summary>
		/// <param name="id">Identifier.</param>
		/// <param name="unitEntity">Unit entity.</param>
       public void InsertIntoUnit(int id, Entity unitEntity)
       {
           SQLiteConnection con = new SQLiteConnection(ServerConstants.DB_PATH); 

           DBUnits dbu = new DBUnits(con);
           dbu.NewUnit(unitEntity, id);  
       }
		/// <summary>
		/// Inserts the building into the databank.
		/// </summary>
		/// <param name="id">Identifier.</param>
		/// <param name="buildingEntity">Building entity.</param>
       public void InsertIntoBuilding(int id, Entity buildingEntity)
       {
           SQLiteConnection con = new SQLiteConnection(ServerConstants.DB_PATH); 

           DBBuildings dbb = new DBBuildings(con);
           dbb.NewBuildings(buildingEntity, id);
       }
		/// <summary>
		/// Inserts the resource into the databank, which belong to the owner Id..
		/// </summary>
		/// <param name="ressourceFire">Ressource fire.</param>
		/// <param name="ressourceEarth">Ressource earth.</param>
		/// <param name="ressourceWater">Ressource water.</param>
		/// <param name="ressourceAir">Ressource air.</param>
		/// <param name="ressourceMagic">Ressource magic.</param>
		/// <param name="ressourceGold">Ressource gold.</param>
		/// <param name="id">Identifier.</param>
       public void InsertIntoResource(int ressourceFire, int ressourceEarth, int ressourceWater, int ressourceAir, int ressourceMagic, int ressourceGold, int id)
       {
           SQLiteConnection con = new SQLiteConnection(ServerConstants.DB_PATH); 

           DBRessource dbr = new DBRessource(con);
           dbr.TableRessource(ressourceFire, ressourceEarth, ressourceWater, ressourceAir, ressourceMagic, ressourceGold, id);  
       }
		/// <summary>
		/// Updates the unit, which belong to the owner Id.
		/// </summary>
		/// <param name="unitEntity">Unit entity.</param>
		/// <param name="id">Identifier.</param>
       public void UpdateUnit(Entity unitEntity, int id)
       {
           SQLiteConnection con = new SQLiteConnection(ServerConstants.DB_PATH); 

           TableUnit tu = new TableUnit();
           tu.Id = id;
           tu.PositionX = unitEntity.Position.X;
           tu.PositionY = unitEntity.Position.Y;

           con.InsertOrReplace(tu);
       }
		/// <summary>
		/// Updates the building, which belong to the owner Id.
		/// </summary>
		/// <param name="buildingEntity">Building entity.</param>
		/// <param name="id">Identifier.</param>
       public void UpdateBuilding(Entity buildingEntity, int id)
       {
           SQLiteConnection con = new SQLiteConnection(ServerConstants.DB_PATH); 

           TableBuilding tb = new TableBuilding();
           tb.Id = id;
           tb.PositionX = buildingEntity.Position.X;
           tb.PositionY = buildingEntity.Position.Y;

           con.InsertOrReplace(tb);
       }
		/// <summary>
		/// Inserts all units, which belong to the owner Id.
		/// </summary>
		/// <param name="unitEntitiys">Unit entitiys.</param>
		/// <param name="id">Identifier.</param>
       public void InsertAllUnits(IList<Entity>  unitEntitiys, int id)
       {          

           for (int index = 0; index < unitEntitiys.Count; ++index)
           {
               InsertIntoUnit(id, unitEntitiys[index]);
           }
       }
		/// <summary>
		/// Inserts all buildings, which belong to the owner Id.
		/// </summary>
		/// <param name="buildingEntitys">Building entitys.</param>
		/// <param name="id">Identifier.</param>
       public void InsertAllBuildings(IList<Entity> buildingEntitys, int id)
       {

           for (int index = 0; index < buildingEntitys.Count; ++index)
           {
               InsertIntoBuilding(id, buildingEntitys[index]);
           }
       }
		/// <summary>
		/// Updates the ressource, which belong to the owner Id.
		/// </summary>
		/// <param name="ressourceFire">Ressource fire.</param>
		/// <param name="ressourceEarth">Ressource earth.</param>
		/// <param name="ressourceWater">Ressource water.</param>
		/// <param name="ressourceAir">Ressource air.</param>
		/// <param name="ressourceMagic">Ressource magic.</param>
		/// <param name="ressourceGold">Ressource gold.</param>
		/// <param name="id">Identifier.</param>
       public void UpdateRessource(int ressourceFire, int ressourceEarth, int ressourceWater, int ressourceAir, int ressourceMagic, int ressourceGold, int id)
       {
           SQLiteConnection con = new SQLiteConnection(ServerConstants.DB_PATH); 

           TableRessource tr = new TableRessource();
           tr.Id = id;
           tr.Fire = ressourceFire;
           tr.Earth = ressourceEarth;
           tr.Water = ressourceWater;
           tr.Air = ressourceAir;
           tr.Magic = ressourceMagic;
           tr.Gold = ressourceGold;

           con.InsertOrReplace(tr);

       }
		/// <summary>
		/// Deletes the unit.
		/// </summary>
		/// <param name="id">Identifier.</param>
       public void DeleteUnit(int id)
       {
           SQLiteConnection con = new SQLiteConnection(ServerConstants.DB_PATH); 

           con.Delete<TableUnit>(id);
       }
		/// <summary>
		/// Deletes the building.
		/// </summary>
		/// <param name="id">Identifier.</param>
       public void DeleteBuilding(int id)
       {
           SQLiteConnection con = new SQLiteConnection(ServerConstants.DB_PATH); 

           con.Delete<TableBuilding>(id);
       }
		/// <summary>
		/// Deletes the account from all tables.
		/// </summary>
		/// <param name="id">Identifier.</param>
       public void DeleteAccountFromAllTables(int id)
       {
           SQLiteConnection con = new SQLiteConnection(ServerConstants.DB_PATH); 

           con.Delete<TableAccount>(id);
           con.Delete<TableBuilding>(id);
           con.Delete<TableRessource>(id);
           con.Delete<TableUnit>(id);
       }
    }
}

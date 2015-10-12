namespace Server.DB
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using Core.Models;
    using Server.DB.Models;
    using Server.Models;
    using SQLite;

    /// <summary>
    /// DB handle.
    /// </summary>
    public class DBHandle
    {         
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
       /// <param name="account">Account for a new user.</param>
       /// <param name="password">Password for a new user.</param>
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
       /// <param name="account">Account for DB data.</param>
       /// <param name="password">Password for DB data.</param>
       public TableData GetAccountDataViaDBLogin(Account account, string password)
       {
           SQLiteConnection con = new SQLiteConnection(ServerConstants.DB_PATH);  

           DBAccount dbacc = new DBAccount(con);
           TableData tb = new TableData();

           if (dbacc.Login(account.UserName, password))
           {
               var data = con.Query<TableAccount>("SELECT Id FROM Account WHERE UserName = ? LIMIT 1", account.UserName);
               tb.Id = data[0].Id;
               tb.Units = con.Query<TableUnit>("SELECT * FROM Unit WHERE Id = ?", tb.Id);
               tb.Buildings = con.Query<TableBuilding>("SELECT * FROM Building WHERE Id = ?", tb.Id);
               tb.Resources = con.Query<TableResource>("SELECT * FROM Ressources WHERE Id = ?", tb.Id);               
           }
           
           return tb;
       }

       /// <summary>
       /// Gets the account data via I.
       /// </summary>
       /// <returns>The account data via ID.</returns>
       /// <param name="id">Identifier for DB access.</param>
       public TableData GetAccountDataViaID(int id)
       {
           SQLiteConnection con = new SQLiteConnection(ServerConstants.DB_PATH);
                      
           TableData tb = new TableData();
           
           tb.Units = con.Query<TableUnit>("SELECT * FROM Unit WHERE Id = ?", id);
           tb.Buildings = con.Query<TableBuilding>("SELECT * FROM Building WHERE Id = ?", id);
           tb.Resources = con.Query<TableResource>("SELECT * FROM Ressources WHERE Id = ?", id);          

           return tb;
       }

       /// <summary>
       /// Inserts the into unit.
       /// </summary>
       /// <param name="id">Identifier for the account.</param>
       /// <param name="unitEntity">Unit entity.</param>
       public void InsertIntoUnit(int id, Entity unitEntity)
       {
           SQLiteConnection con = new SQLiteConnection(ServerConstants.DB_PATH); 

           DBUnits dbu = new DBUnits(con);
           dbu.NewUnit(unitEntity, id);  
       }

       /// <summary>
       /// Inserts the into building.
       /// </summary>
       /// <param name="id">Identifier for the account.</param>
       /// <param name="buildingEntity">Building entity.</param>
       public void InsertIntoBuilding(int id, Entity buildingEntity)
       {
           SQLiteConnection con = new SQLiteConnection(ServerConstants.DB_PATH); 

           DBBuildings dbb = new DBBuildings(con);
           dbb.NewBuildings(buildingEntity, id);
       }

       /// <summary>
       /// Inserts the into resource.
       /// </summary>
       /// <param name="resourceFire">Resource fire.</param>
       /// <param name="resourceEarth">Resource earth.</param>
       /// <param name="resourceWater">Resource water.</param>
       /// <param name="resourceAir">Resource air.</param>
       /// <param name="resourceMagic">Resource magic.</param>
       /// <param name="resourceGold">Resource gold.</param>
       /// <param name="id">Identifier for the account.</param>
       public void InsertIntoResource(int resourceFire, int resourceEarth, int resourceWater, int resourceAir, int resourceMagic, int resourceGold, int id)
       {
           SQLiteConnection con = new SQLiteConnection(ServerConstants.DB_PATH); 

           DBResource dbr = new DBResource(con);
           dbr.TableResource(resourceFire, resourceEarth, resourceWater, resourceAir, resourceMagic, resourceGold, id);  
       }

       /// <summary>
       /// Updates the unit.
       /// </summary>
       /// <param name="unitEntity">Unit entity.</param>
       /// <param name="id">Identifier for the account.</param>
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
       /// Updates the building.
       /// </summary>
       /// <param name="buildingEntity">Building entity.</param>
        /// <param name="id">Identifier for the account.</param>
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
       /// Inserts all units.
       /// </summary>
       /// <param name="unitEntities">Unit entities.</param>
       /// <param name="id">Identifier for the account.</param>
       public void InsertAllUnits(IList<Entity> unitEntities, int id)
       {  
           for (int index = 0; index < unitEntities.Count; ++index)
           {
               InsertIntoUnit(id, unitEntities[index]);
           }
       }

       /// <summary>
       /// Inserts all buildings.
       /// </summary>
       /// <param name="buildingEntities">Building entities.</param>
       /// <param name="id">Identifier for the account.</param>
       public void InsertAllBuildings(IList<Entity> buildingEntities, int id)
       {
           for (int index = 0; index < buildingEntities.Count; ++index)
           {
               InsertIntoBuilding(id, buildingEntities[index]);
           }
       }

       /// <summary>
       /// Updates the resource.
       /// </summary>
       /// <param name="ressourceFire">Resource fire.</param>
       /// <param name="ressourceEarth">Resource earth.</param>
       /// <param name="ressourceWater">Resource water.</param>
       /// <param name="ressourceAir">Resource air.</param>
       /// <param name="ressourceMagic">Resource magic.</param>
       /// <param name="ressourceGold">Resource gold.</param>
       /// <param name="id">Identifier for the account.</param>
       public void UpdateResource(int resourceFire, int resourceEarth, int resourceWater, int resourceAir, int resourceMagic, int resourceGold, int id)
       {
           SQLiteConnection con = new SQLiteConnection(ServerConstants.DB_PATH); 

           TableResource tr = new TableResource();
           tr.Id = id;
           tr.Fire = resourceFire;
           tr.Earth = resourceEarth;
           tr.Water = resourceWater;
           tr.Air = resourceAir;
           tr.Magic = resourceMagic;
           tr.Gold = resourceGold;

           con.InsertOrReplace(tr);
       }

       /// <summary>
       /// Deletes the unit.
       /// </summary>
        /// <param name="id">Identifier for the account..</param>
       public void DeleteUnit(int id)
       {
           SQLiteConnection con = new SQLiteConnection(ServerConstants.DB_PATH); 

           con.Delete<TableUnit>(id);
       }

       /// <summary>
       /// Deletes the building.
       /// </summary>
        /// <param name="id">Identifier for the account..</param>
       public void DeleteBuilding(int id)
       {
           SQLiteConnection con = new SQLiteConnection(ServerConstants.DB_PATH); 

           con.Delete<TableBuilding>(id);
       }

       /// <summary>
       /// Deletes the account from all tables.
       /// </summary>
        /// <param name="id">Identifier for the account..</param>
       public void DeleteAccountFromAllTables(int id)
       {
           SQLiteConnection con = new SQLiteConnection(ServerConstants.DB_PATH); 

           con.Delete<TableAccount>(id);
           con.Delete<TableBuilding>(id);
           con.Delete<TableResource>(id);
           con.Delete<TableUnit>(id);
       }

       /// <summary>
       /// The instance from the DB.
       /// </summary>
       private static DBHandle instance;

       /// <summary>
       /// Prevents a default instance of the <see cref="Server.DB.DBHandle"/> class from being created.
       /// </summary>
       private DBHandle() 
       { 
       }
    }
}

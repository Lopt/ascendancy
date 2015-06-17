using @base.model;
using SQLite;
using System;
using System.Collections.Generic;
using server.model;
using System.Collections;
using server.DB.Model;

namespace server.DB
{
    public class DBHandle
    {
       private static DBHandle instance;

       private DBHandle() { }

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

       public void CreateNewDBAccount(Account account, string password)
       {
           SQLiteConnection con = new SQLiteConnection(ServerConstants.DB_PATH);  

           DBAccount dbacc = new DBAccount(con);
          // DBUnits dbunit = new DBUnits(con);
          // DBBuildings dbbuild = new DBBuildings(con);

           dbacc.CreateAccount(account, password);
       }

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

       public TableData GetAccountDataViaID(int Id)
       {
           SQLiteConnection con = new SQLiteConnection(ServerConstants.DB_PATH);
                      
           TableData tb = new TableData();
           
           tb.m_units = con.Query<TableUnit>("SELECT * FROM Unit WHERE Id = ?", Id);
           tb.m_buildings = con.Query<TableBuilding>("SELECT * FROM Building WHERE Id = ?", Id);
           tb.m_ressources = con.Query<TableRessource>("SELECT * FROM Ressources WHERE Id = ?", Id);          

           return tb;
       }

       public void InsertIntoUnit(int id, Entity unitEntity)
       {
           SQLiteConnection con = new SQLiteConnection(ServerConstants.DB_PATH); 

           DBUnits dbu = new DBUnits(con);
           dbu.NewUnit(unitEntity, id);  
       }

       public void InsertIntoBuilding(int id, Entity buildingEntity)
       {
           SQLiteConnection con = new SQLiteConnection(ServerConstants.DB_PATH); 

           DBBuildings dbb = new DBBuildings(con);
           dbb.NewBuildings(buildingEntity, id);
       }

       public void InsertIntoResource(int ressourceFire, int ressourceEarth, int ressourceWater, int ressourceAir, int ressourceMagic, int ressourceGold, int id)
       {
           SQLiteConnection con = new SQLiteConnection(ServerConstants.DB_PATH); 

           DBRessource dbr = new DBRessource(con);
           dbr.TableRessource(ressourceFire, ressourceEarth, ressourceWater, ressourceAir, ressourceMagic, ressourceGold, id);  
       }

       public void UpdateUnit(Entity unitEntity, int id)
       {
           SQLiteConnection con = new SQLiteConnection(ServerConstants.DB_PATH); 

           TableUnit tu = new TableUnit();
           tu.Id = id;
           tu.PositionX = unitEntity.Position.X;
           tu.PositionY = unitEntity.Position.Y;

           con.InsertOrReplace(tu);
       }

       public void UpdateBuilding(Entity buildingEntity, int id)
       {
           SQLiteConnection con = new SQLiteConnection(ServerConstants.DB_PATH); 

           TableBuilding tb = new TableBuilding();
           tb.Id = id;
           tb.PositionX = buildingEntity.Position.X;
           tb.PositionY = buildingEntity.Position.Y;

           con.InsertOrReplace(tb);
       }

       public void InsertAllUntis(IList<Entity>  unitEntitiys, int id)
       {          

           for (int index = 0; index < unitEntitiys.Count; ++index)
           {
               InsertIntoUnit(id, unitEntitiys[index]);
           }
       }

       public void InsertAllBuildings(IList<Entity> buildingEntitys, int id)
       {

           for (int index = 0; index < buildingEntitys.Count; ++index)
           {
               InsertIntoBuilding(id, buildingEntitys[index]);
           }
       }

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

       public void DeleteUnit(int id)
       {
           SQLiteConnection con = new SQLiteConnection(ServerConstants.DB_PATH); 

           con.Delete<TableUnit>(id);
       }

       public void DeleteBuilding(int id)
       {
           SQLiteConnection con = new SQLiteConnection(ServerConstants.DB_PATH); 

           con.Delete<TableBuilding>(id);
       }

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

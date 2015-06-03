using @base.model;
<<<<<<< HEAD
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace server.DB
{
    class DBHandle
    {
       private static DBHandle instance;
=======
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
       private static SQLiteConnection con;
>>>>>>> d741c7fafad5bbf531fdb127801a8575f7c0add4

       private DBHandle() { }

       public static DBHandle Instance
       {
<<<<<<< HEAD
          get 
          {
             if (instance == null)
             {
                 instance = new DBHandle();
             }
             return instance;
          }
       }

       public void InsertIntoAccount(Account account, string password)
       {
           DBLogin dbLog = new DBLogin();
           dbLog.TableAccount(account, password);
       }

       public void InsertIntoyUnit()
       {
           DBUnits dbunit = new DBUnits();
           dbunit.TableUnits();
       }

       public void InsertIntoBuilding()
       {

       }

       public void InsertIntoResource()
       {
       }

       public void SearchEntry();
       public void DeleteAllEntrys();


=======
           get
           {
               if (instance == null)
               {
                   instance = new DBHandle();
                   con = new SQLiteConnection(ServerConstants.DB_PATH);
               }
               return instance;
           }
       }

       public void CreateNewDBAccount(Account account, string password)
       {
           DBAccount dbacc = new DBAccount(con);
           DBUnits dbunit = new DBUnits(con);
           DBBuildings dbbuild = new DBBuildings(con);

           dbacc.CreateAccount(account, password);           
       }

       public TableData GetAccountData(Account account, string password)
       {
           DBAccount dbacc = new DBAccount(con);
           TableData tb = new TableData();

           if (dbacc.Login(account.UserName, password))
           {
               var id = con.Query<TableAccount>("SELECT Id FROM Items WHERE = ?",account.UserName);
               tb.m_units.Add(con.Query<TableUnit>("SELECT * FROM Items WHERE  = ?", id));
               tb.m_buildings.Add(con.Query<TableBuilding>("SELECT * FROM Items WHERE  = ?", id));
               tb.m_ressources.Add(con.Query<TableRessource>("SELECT * FROM Items WHERE  = ?", id));               
           }
           
           return tb;
       }

       public void InsertIntoUnit(int id, Entity unitEntity)
       {
           DBUnits dbu = new DBUnits(con);
           dbu.NewUnit(unitEntity, id);
           con.Insert(dbu);           
       }

       public void InsertIntoBuilding(int id, Entity buildingEntity)
       {
           DBBuildings dbb = new DBBuildings(con);
           dbb.NewBuildings(buildingEntity, id);
           con.Insert(dbb);
       }

       public void InsertIntoResource(int ressourceFire, int ressourceEarth, int ressourceWater, int ressourceAir, int ressourceMagic, int ressourceGold, int id)
       {
           DBRessource dbr = new DBRessource(con);
           dbr.TableRessource(ressourceFire, ressourceEarth, ressourceWater, ressourceAir, ressourceMagic, ressourceGold, id);
           con.Insert(dbr);
       }

       public void UpdateUnit(Entity unitEntity, int id)
       {
           TableUnit tu = new TableUnit();
           tu.ID = id;
           tu.PositionX = unitEntity.Position.X;
           tu.PositionY = unitEntity.Position.Y;

           con.InsertOrReplace(tu);
          // con.Update(tu);
           
       }

       public void UpdateBuilding(Entity buildingEntity, int id)
       {
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
           TableRessource tr = new TableRessource();
           tr.ID = id;
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
           con.Delete<TableUnit>(id);
       }

       public void DeleteBuilding(int id)
       {
           con.Delete<TableBuilding>(id);
       }

       public void DeleteAccountFromAllTables(int id)
       {
           con.Delete<TableAccount>(id);
           con.Delete<TableBuilding>(id);
           con.Delete<TableRessource>(id);
           con.Delete<TableUnit>(id);
       }
>>>>>>> d741c7fafad5bbf531fdb127801a8575f7c0add4
    }
}

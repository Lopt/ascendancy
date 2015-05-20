using @base.model;
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


    }
}

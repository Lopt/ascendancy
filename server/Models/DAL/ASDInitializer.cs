using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SQLite;

namespace server.Models.DAL
{
    public class ASDInitializer : System.Data.Entity. DropCreateDatabaseIfModelChanges<UserData>
    {

        protected override void Seed(UserData context)
        {
            var Users = new List<User>
            {
              new User{Value = "alvbalbg"},
              new User{Value = "sbgbalbg"},
              new User{Value = "aabg"},
              new User{Value = "ggggggg"},
              new User{Value = "a534345"},
              new User{Value = "3435434a3g"},
              new User{Value = "alvbalbg"},
            };

            Users.ForEach(s => context.User.Add(s));
            context.SaveChanges();
        }
               
    }
}
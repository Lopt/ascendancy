using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

// DAL = Data Access Layer
namespace server.Models.DAL
{
    public class UserData : DbContext
    {
        // ASD Database Name
        public UserData() : base("ASD.mdf")
        {
        }

        public DbSet<User> User {get; set;}

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}
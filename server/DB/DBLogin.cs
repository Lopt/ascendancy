using System;
using System.Collections;
using @base.model;
using SQLite;
using server.model;
using System.Security.Cryptography;
using System.Text;

namespace server.DB
{
    public class DBLogin
    {
        
        public void TableAccount(Account account, string password)
        {
            var db = new SQLiteConnection(model.ServerConstants.DB_PATH);

            db.CreateCommand("CREATE TABLE IF NOT EXISTS [Account] ([Id] INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL, [Username] TEXT NOT NULL, [Password] TEXT NOT NULL , [Salt] TEXT NOT NULL CONSTRAINT [PK_ContentItems] PRIMARY KEY ([Id]))");
         
            var salt = GenerateSaltValue();
            var DBPassword = GenerateSaltedHash(password, salt);

            db.CreateCommand("INSERT INTO [Account] (Username,Password,Salt) VALUES('account.UserName','DBPassword','salt')");
            
        }
        
        private string GenerateSaltValue()
        {
            var utf16 = new UnicodeEncoding();
            Random random = new Random(unchecked((int)DateTime.Now.Ticks));
            
            byte[] saltValue = new byte[model.ServerConstants.SALT_SIZE];

            random.NextBytes(saltValue);

            string SaltValueString = utf16.GetString(saltValue);

            return SaltValueString;
        }

        private string GenerateSaltedHash(string password, string salt)
        {
        
            string sHashWithSalt = password + salt;         
            byte[] saltedHashBytes = Encoding.UTF8.GetBytes(sHashWithSalt);
        
            var algorithm = new SHA256Managed();

            byte[] hash = algorithm.ComputeHash(saltedHashBytes);

            for (int index = 0; index < model.ServerConstants.HASH_CICLE -1; ++index)
            {
                hash = algorithm.ComputeHash(saltedHashBytes);
            }

            return Convert.ToBase64String(hash);
        }
    }


}
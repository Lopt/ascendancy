using System;
using System.Collections;
using @base.model;
using SQLite;
using @server.model;
using System.Security.Cryptography;
using System.Text;
using server.DB.Model;

namespace server.DB
{
    class DBAccount
    {
        
        public DBAccount(SQLiteConnection con)
        {
            m_db = con;
            m_db.CreateTable<TableAccount>();
        }
        
        public void CreateAccount(Account account, string password)
        {
            var salt = GenerateSaltValue();
            var DBPassword = GenerateSaltedHash(password, salt);

            var newData = new TableAccount();
            newData.UserName = account.UserName;
            newData.Password = DBPassword;
            newData.Salt = salt;

            m_db.InsertOrReplace(newData);
        }
        
        public bool Login(string username, string password)
        {
            var result = m_db.Query<TableAccount>("SELECT Id, UserName, Salt, Password FROM Account WHERE UserName = ? LIMIT 1", username);
            if (result.Count == 0)
            {
                return false;
            }

            var DBsalt = result[0].Salt;
            
            var CalcSalt = GenerateSaltedHash(password, DBsalt);

            if (CalcSalt == result[0].Password)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        
        private string GenerateSaltValue()
        {
            var utf16 = new UnicodeEncoding();
            Random random = new Random(unchecked((int)DateTime.Now.Ticks));
            
            byte[] saltValue = new byte[ServerConstants.SALT_SIZE];

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

            for (int index = 0; index < ServerConstants.HASH_CICLE -1; ++index)
            {
                hash = algorithm.ComputeHash(saltedHashBytes);
            }

            return Convert.ToBase64String(hash);
        }

        private SQLiteConnection m_db;
    }


}
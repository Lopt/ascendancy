using Core.Models;
using SQLite;
using Server.Models;
using System.Security.Cryptography;
using Server.DB.Models;
using System.Text;
using System;

namespace Server.DB
{
    /// <summary>
    /// DB account.
    /// </summary>
    class DBAccount
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="server.DB.DBAccount"/> class.
        /// </summary>
        /// <param name="con">Connection to the databank.</param>
        public DBAccount(SQLiteConnection con)
        {
            m_db = con;
            m_db.CreateTable<TableAccount>();
        }        
		/// <summary>
		/// Creates the account with username, password, and the calculated salt value.
		/// </summary>
		/// <param name="account">Account.</param>
		/// <param name="password">Password.</param>
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
        /// <summary>
        /// Login the specified username and password.
        /// </summary>
        /// <param name="username">Username.</param>
        /// <param name="password">Password.</param>
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
        /// <summary>
        /// Generates the salt value.
        /// </summary>
        /// <returns>The salt value.</returns>
        private string GenerateSaltValue()
        {
            var utf16 = new UnicodeEncoding();
            Random random = new Random(unchecked((int)DateTime.Now.Ticks));
            
            byte[] saltValue = new byte[ServerConstants.SALT_SIZE];

            random.NextBytes(saltValue);

            string SaltValueString = utf16.GetString(saltValue);

            return SaltValueString;
        }
		/// <summary>
		/// Generates the salted hash.
		/// </summary>
		/// <returns>The salted hash.</returns>
		/// <param name="password">Password.</param>
		/// <param name="salt">Salt.</param>
        private string GenerateSaltedHash(string password, string salt)
        {
        
            string sHashWithSalt = password + salt;         
            byte[] saltedHashBytes = Encoding.UTF8.GetBytes(sHashWithSalt);
        
            var algorithm = new SHA256Managed();

            byte[] hash = algorithm.ComputeHash(saltedHashBytes);

            for (int index = 0; index < ServerConstants.HASH_CYCLES -1; ++index)
            {
                hash = algorithm.ComputeHash(saltedHashBytes);
            }

            return Convert.ToBase64String(hash);
        }

        private SQLiteConnection m_db;
    }


}
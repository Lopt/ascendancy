namespace Server.DB
{
    using System;
    using System.Security.Cryptography;
    using System.Text;
    using Core.Models;
    using Server.DB.Models;
    using Server.Models;
    using SQLite;

    /// <summary>
    /// DB account.
    /// </summary>
    public class DBAccount
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Server.DB.DBAccount"/> class.
        /// </summary>
        /// <param name="con">Connection to the DB.</param>
        public DBAccount(SQLiteConnection con)
        {
            db = con;
            db.CreateTable<TableAccount>();
        }      

        /// <summary>
        /// Creates the account.
        /// </summary>
        /// <param name="account">Account access.</param>
        /// <param name="password">Password for the DB.</param>
        public void CreateAccount(Account account, string password)
        {
            var salt = GenerateSaltValue();
            var saltedPassword = GenerateSaltedHash(password, salt);

            var newData = new TableAccount();
            newData.UserName = account.UserName;
            newData.Password = saltedPassword;
            newData.Salt = salt;

            db.InsertOrReplace(newData);
        }

        /// <summary>
        /// Login the specified username and password.
        /// </summary>
        /// <param name="username">Username for DB.</param>
        /// <param name="password">Password for DB.</param>
        /// /// <returns>True or false.</returns>
        public bool Login(string username, string password)
        {
            var result = db.Query<TableAccount>("SELECT Id, UserName, Salt, Password FROM Account WHERE UserName = ? LIMIT 1", username);
            if (result.Count == 0)
            {
                return false;
            }

            var salt = result[0].Salt;
            
            var calcSalt = GenerateSaltedHash(password, salt);

            if (calcSalt == result[0].Password)
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

            string saltValueString = utf16.GetString(saltValue);

            return saltValueString;
        }

        /// <summary>
        /// Generates the salted hash.
        /// </summary>
        /// <returns>The salted hash.</returns>
        /// <param name="password">Password for generating hash value.</param>
        /// <param name="salt">Salt for generating hash value.</param>
        private string GenerateSaltedHash(string password, string salt)
        {        
            string hashWithSalt = password + salt;         
            byte[] saltedHashBytes = Encoding.UTF8.GetBytes(hashWithSalt);
        
            var algorithm = new SHA256Managed();

            byte[] hash = algorithm.ComputeHash(saltedHashBytes);

            for (int index = 0; index < ServerConstants.HASH_CYCLES; ++index)
            {
                hash = algorithm.ComputeHash(saltedHashBytes);
            }

            return Convert.ToBase64String(hash);
        }

        /// <summary>
        /// The DB.
        /// </summary>
        private SQLiteConnection db;
    }
}
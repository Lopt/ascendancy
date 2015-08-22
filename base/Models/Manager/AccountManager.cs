namespace Core.Models
{
    using System;
    using System.Collections.Concurrent;
    using Core.Models;
    using Core.Models.Definitions;
    using Newtonsoft.Json;

    /// <summary>
    /// Contains all Accounts with id.
    /// </summary>
    public class AccountManager
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Core.Models.AccountManager"/> class.
        /// </summary>
        public AccountManager()
        {
            Accounts = new ConcurrentDictionary<int, Account>();
        }

        /// <summary>
        /// Adds an account.
        /// </summary>
        /// <param name="account">Account which should be added.</param>
        public void AddAccount(Account account)
        {
            Accounts.TryAdd(account.ID, account);
        }

        /// <summary>
        /// Gets the account or empty account.
        /// </summary>
        /// <returns>The account or empty account.</returns>
        /// <param name="id">Account Identifier.</param>
        public Account GetAccountOrEmpty(int id)
        {
            var account = GetAccount(id);
            if (account == null)
            {
                account = new Account(id);
            }
            return account;
        }

        /// <summary>
        /// Returns the account
        /// </summary>
        /// <returns>The account or null (if there is none)</returns>
        /// <param name="id">Account Identifier .</param>
        public Account GetAccount(int id)
        {
            if (Accounts.ContainsKey(id))
            {
                return Accounts[id];
            }
            return null;
        }

        /// <summary>
        /// The accounts.
        /// </summary>
        public ConcurrentDictionary<int, Account> Accounts;
    }
}

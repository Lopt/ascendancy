using System;
using System.Collections.Concurrent;
using Newtonsoft.Json;
using Core.Models;
using Core.Models.Definitions;

namespace Core.Models
{
    /// <summary>
    /// Contains all Account with id.
    /// </summary>
    public class AccountManager
    {
        public AccountManager()
        {
            Accounts = new ConcurrentDictionary<int, Account>();
        }

        public void AddAccount(Account account)
        {
            Accounts.TryAdd(account.ID, account);
        }

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
        /// <param name="id">Identifier.</param>
        public Account GetAccount(int id)
        {
            if (Accounts.ContainsKey(id))
            {
                return Accounts[id];
            }
            return null;
        }

        public ConcurrentDictionary<int, Account> Accounts;
    }

}


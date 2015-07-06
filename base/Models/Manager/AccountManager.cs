using System;
using System.Collections.Concurrent;
using Newtonsoft.Json;
using @base.model;
using @base.model.definitions;

namespace @base.model
{
    public class AccountManager
    {
        public AccountManager()
        {
            Accounts = new ConcurrentDictionary<int, Account> ();
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
        /// <returns>The account or none (is there is none</returns>
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


using System;
using System.Collections.Concurrent;
using Newtonsoft.Json;
using @base.model;
using @base.model.definitions;

namespace @base.control
{
    public class AccountManagerController
    {
        public AccountManagerController()
        {
        }

        public void AddAccount(Account account)
        {
            World.Instance.Accounts.TryAdd(account.ID, account);
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
            if (World.Instance.Accounts.ContainsKey(id))
            {
                return World.Instance.Accounts[id];
            }
            return null;
        }

    }



}


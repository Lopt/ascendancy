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

       /// <summary>
        /// Returns the account
       /// </summary>
       /// <returns>The account or none (is there is none</returns>
       /// <param name="id">Identifier.</param>
        public Account GetAccount(int id)
        {
            if (!World.Instance.Accounts.ContainsKey(id))
            {
                return World.Instance.Accounts[id];
            }
            return null;
        }

        public bool Registrate(Account account)
        {
            if (!World.Instance.Accounts.ContainsKey(account.ID))
            {
                World.Instance.Accounts[account.ID] = account;
                return true;
            }
            return false;
        }
    }



}


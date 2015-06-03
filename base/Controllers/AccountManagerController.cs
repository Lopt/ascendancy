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

        public Account GetAccount(int id)
        {
            return World.Instance.Accounts[id];
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


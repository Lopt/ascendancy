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

        public Account GetAccount(Guid guid)
        {
            return World.Instance.Accounts[guid];
        }

        public bool Registrate(Account account)
        {
            if (!World.Instance.Accounts.ContainsKey(account.GUID))
            {
                World.Instance.Accounts[account.GUID] = account;
                return true;
            }
            return false;
        }
    }



}


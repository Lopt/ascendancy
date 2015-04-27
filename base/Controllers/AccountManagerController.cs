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
    }



}


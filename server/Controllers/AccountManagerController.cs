using System;
using System.Collections.Concurrent;
using Newtonsoft.Json;
using Core.Models;
using server.model;
using server.DB;

namespace server.control
{
    public class AccountManagerController : Core.Models.AccountManager
    {
        public AccountManagerController()
            : base()
        {
            m_sessions = new ConcurrentDictionary<Guid, Account>();
        }

        public Account Login(string username, string password)
        {
            foreach (var accountPair in World.Instance.AccountManager.Accounts)
            {
                if (accountPair.Value.UserName == username)
                {
                    var accountC = (AccountController)accountPair.Value.Control;
                    if (accountC.Login(username, password))
                    {
                        m_sessions[accountC.SessionID] = accountPair.Value;
                        return accountPair.Value;
                    }
                }
            }
            return null;
        }

        public Account Registrate(string username, string password)
        {
            foreach (var accountPair in Core.Models.World.Instance.AccountManager.Accounts)
            {
                if (accountPair.Value.UserName.ToLower() == username.ToLower())
                {
                    return Login(username, password);
                }				
            }
            var account = new Account(IdGenerator.GetId(), username);
            new AccountController(account, password);

            Core.Models.World.Instance.AccountManager.AddAccount(account);
            return Login(username, password);
        }


        public Account GetAccountBySession(Guid sessionID)
        {
            Account account = null;
            if (m_sessions.TryGetValue(sessionID, out account))
            {
                return account;
            }
            return null;
        }

        ConcurrentDictionary<Guid, Account> m_sessions;
    }
}


using System;
using System.Collections.Concurrent;
using Newtonsoft.Json;
using Core.Models;
using Server.Models;
using Server.DB;

namespace Server.Controllers
{
	/// <summary>
	/// The AccountManagerController handles a list of all users and those, which are logged in. It can also log in users and verify a session id.
	/// </summary>
    public class AccountManagerController : Core.Models.AccountManager
    {
        public AccountManagerController()
            : base()
        {
            m_sessions = new ConcurrentDictionary<Guid, Account>();
        }

		/// <summary>
		/// Login with a username und password, returns Account if everything worked, otherwise <b>null</b>
		/// </summary>
		/// <param name="username">Username.</param>
		/// <param name="password">Password.</param>
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

		/// <summary>
		/// Registrates the username/password combination as a new user. If the username already exist, he will be logged in. returns <b>null</b> when username is already taken with an different password.
		/// </summary>
		/// <param name="username">Username.</param>
		/// <param name="password">Password.</param>
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

		/// <summary>
		/// Gets the account by a session.
		/// </summary>
		/// <returns>An Account which is assigned to this session id or null, if there was none.</returns>
		/// <param name="sessionID">Session ID</param>
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


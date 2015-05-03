using System;
using System.Collections.Concurrent;
using Newtonsoft.Json;
using @base.model;
using server.model;

namespace server.control
{
	public class AccountManagerController : @base.control.AccountManagerController
	{
		public AccountManagerController()
			: base()
		{
			m_sessions = new ConcurrentDictionary<Guid, Account> ();
		}

		public Account Login(string username, string password)
		{
			foreach (var accountPair in World.Instance.Accounts)
			{
				if (accountPair.Value.UserName == username)
				{
					var accountC = (AccountController)accountPair.Value.Control;
					if (accountC.Login (username, password))
					{
						m_sessions [accountC.SessionID] = accountPair.Value;
						return accountPair.Value;
					}
				}
			}
			return null;
		}

		public Account GetAccountBySession(Guid sessionID)
		{
			Account account = null;
			if (m_sessions.TryGetValue (sessionID, out account))
			{
				return account;
			}
			return null;
		}

		ConcurrentDictionary<Guid, Account> m_sessions;
    }
}


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

		public bool Login(string username, string password)
		{
			foreach (var account in World.Instance.Accounts)
			{
				if (account.Value.UserName == username)
				{
					var accountC = (AccountController)account.Value.Control;
					if (accountC.Login (username, password))
					{
						m_sessions [accountC.SessionID] = account.Value;
						return true;
					}
				}
			}
			return false;
		}
	

		ConcurrentDictionary<Guid, Account> m_sessions;
    }
}


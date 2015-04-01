using System;

namespace @base.model
{
	public class Account
	{
		public Account (Guid guid)
		{
			m_guid = guid;
            m_username = "???";
		}
            
		public Guid GUID
		{
			get { return this.m_guid; }
		}

		public string UserName
		{
			set { this.m_username = value; }
			get { return this.m_username; }
		}


		private Guid m_guid;
		private string m_username;

	}
}


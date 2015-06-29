using System;
using SQLite;
using System.Collections.Generic;

namespace @base.model
{
	public class Account : ModelEntity
	{
       
        public Account (int id)
            : base()
        {
            m_id = id;
            m_username = "???";
            m_headquarters = new LinkedList<PositionI>();
        }

        public Account (int id, string userName)
        {
            m_id = id;
            m_username = userName;
            m_headquarters = new LinkedList<PositionI>();
        }

		public int ID
		{
			get { return this.m_id; }
		}

		public string UserName
		{
			set { this.m_username = value; }
			get { return this.m_username; }
        }

        public LinkedList<PositionI> Headquarters
        {
            get { return m_headquarters; }
        }

		private int m_id;
		private string m_username;
        private LinkedList<PositionI> m_headquarters;

	}
}


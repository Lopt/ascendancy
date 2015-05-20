using System;
using System.Collections.ObjectModel;

namespace @base.model
{
	public class Account : ModelEntity
	{
        public Account (Guid guid)
            : base()
        {
            m_guid = guid;
            m_username = "???";
            m_headquarters = new ObservableCollection<PositionI>();
        }

        public Account (Guid guid, string userName)
        {
            m_guid = guid;
            m_username = userName;
            m_headquarters = new ObservableCollection<PositionI>();
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

        public ObservableCollection<PositionI> Headquarters
        {
            get { return m_headquarters; }
        }

		private Guid m_guid;
		private string m_username;
        private ObservableCollection<PositionI> m_headquarters;

	}
}


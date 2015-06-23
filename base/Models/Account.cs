using System;
using SQLite;
using System.Collections.ObjectModel;

namespace @base.model
{
	public class Account : ModelEntity
	{
       
        public Account (int id)
            : base()
        {
            m_id = id;
            m_username = "???";
            m_headquarters = new ObservableCollection<PositionI>();
            m_units = new ObservableCollection<PositionI>();
        }

        public Account (int id, string userName)
        {
            m_id = id;
            m_username = userName;
            m_headquarters = new ObservableCollection<PositionI>();
            m_units = new ObservableCollection<PositionI>();
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

        public ObservableCollection<PositionI> Headquarters
        {
            get { return m_headquarters; }
        }

        public ObservableCollection<PositionI> Units
        {
            get { return m_headquarters; }
        }

		private int m_id;
		private string m_username;
        private ObservableCollection<PositionI> m_headquarters;
        private ObservableCollection<PositionI> m_units;

	}
}


using System;

namespace @base.model
{
	public class Entity
	{
		public Entity ()
		{

		}
            
        public Guid GUID
        {
            get { return this.m_guid; }
        }

        private Guid m_guid;
        private view.Entity m_view;
        private model.Definition m_definition;


	}
}


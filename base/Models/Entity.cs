using System;

namespace @base.model
{
	public class Entity
	{
        public Entity (Guid guid, model.Definition defintion)
		{
            m_guid = guid; 
            m_definition = defintion;
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


using System;

using @base.model;
using @base.model.definitions;

namespace @base.model
{
    public class Entity : ModelEntity
	{
        public Entity (Guid guid, Definition defintion, CombinedPosition position)
            : base()
		{
            m_guid = guid; 
            m_definition = defintion;
            m_position = position;
		}
            
        public Guid GUID
        {
            get { return m_guid; }
        }

        public Definition Definition
        {
            get { return m_definition; }
        }

        public CombinedPosition Position
        {
            get { return m_position; }
            set { m_position = value; }
        }

        private Guid m_guid;
        private CombinedPosition m_position;
        private Definition m_definition;
	}
}


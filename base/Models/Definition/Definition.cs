using System;

namespace @base.model
{
    public class Definition
    {
        public enum DefinitionType 
        {
            terrain,
            unit,
            building
        }

        public Definition(Guid guid, DefinitionType type)
        {
            m_guid = guid;
            m_type = type;
        }

        public Guid GUID
        {
            get { return this.m_guid; }
        }

        public DefinitionType Type
        {
            get { return this.m_type; }
        }

     
        private Guid m_guid;
        private DefinitionType m_type;
    }
}


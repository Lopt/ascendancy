using System;

namespace @base.model
{
    public class Definition
    {
        public enum DefinitionType 
        {
            Terrain,
            Unit,
            Building
        }

        public Definition(int id, DefinitionType type)
        {
            m_type = type;
        }

        public int ID
        {
            get { return this.m_id; }
        }

        public DefinitionType Type
        {
            get { return this.m_type; }
        }

     
        private int m_id;
        private DefinitionType m_type;
    }
}


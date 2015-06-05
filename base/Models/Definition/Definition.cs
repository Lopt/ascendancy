using System;

namespace @base.model
{
    public class Definition
    {
        public enum DefinitionType 
        {
            Invalid = -1,
            Terrain,
            Unit,
            Building
        }

        public Definition(int id)
        {
            m_id = id;
        }

        public int ID
        {
            get { return this.m_id; }
        }

        public DefinitionType Type
        {
            get
            {
                if (m_id < 60)
                {
                    return DefinitionType.Terrain;
                }
                if (m_id < 276)
                {
                    return DefinitionType.Unit;
                }
                if (m_id < 1000)
                {
                    return DefinitionType.Building;
                }
                return DefinitionType.Invalid;
            }
        }
                 
        private int m_id;
    }
}


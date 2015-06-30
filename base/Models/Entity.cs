using System;

using @base.model;
using @base.model.definitions;
using Newtonsoft.Json;

namespace @base.model
{
    public class Entity : ModelEntity
    {
        public Entity(int id, Definition defintion, PositionI position)
            : base()
        {
            m_id = id; 
            m_definition = defintion;
            m_position = position;
        }

        public int ID
        {
            get { return m_id; }
        }

        public int DefinitionID
        {
            get { return m_definition.ID; }   
        }


        [JsonIgnore]
        public Definition Definition
        {
            get { return m_definition; }
        }

        public PositionI Position
        {
            get { return m_position; }
            set { m_position = value; }
        }

        private int m_id;
        private PositionI m_position;
        private Definition m_definition;
    }
}


using System;
using System.Collections.Concurrent;
using Core.Models.Definitions;

namespace Core.Models
{
    public class DefinitionManager
    {
        public DefinitionManager()
        {
            m_definitions = new ConcurrentDictionary<EntityType, Definition>();
        }

        public Definition GetDefinition(EntityType entityType)
        {
            return m_definitions[entityType];
        }

        public void AddDefinition(Definition definition)
        {
            m_definitions[definition.SubType] = definition;
        }

        private ConcurrentDictionary<EntityType, Definition> m_definitions;

    }
}


using System;
using System.Collections.Concurrent;
using Core.Models.Definitions;

namespace Core.Models
{
    public class DefinitionManager
    {
        public DefinitionManager()
        {
            m_definitions = new ConcurrentDictionary<int, Definition>();
        }

        public Definition GetDefinition(int id)
        {
            return m_definitions[id];
        }

        public void AddDefinition(Definition definition)
        {
            m_definitions[definition.ID] = definition;
        }

        private ConcurrentDictionary<int, Definition> m_definitions;

    }
}


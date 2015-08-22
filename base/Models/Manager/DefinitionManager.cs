namespace Core.Models
{
    using System;
    using System.Collections.Concurrent;
    using Core.Models.Definitions;

    /// <summary>
    /// Contains and manages all Definitions.
    /// </summary>
    public class DefinitionManager
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Core.Models.DefinitionManager"/> class.
        /// </summary>
        public DefinitionManager()
        {
            m_definitions = new ConcurrentDictionary<EntityType, Definition>();
        }

        /// <summary>
        /// Gets an definition.
        /// </summary>
        /// <returns>The definition.</returns>
        /// <param name="entityType">Entity type.</param>
        public Definition GetDefinition(EntityType entityType)
        {
            return m_definitions[entityType];
        }

        /// <summary>
        /// Adds an definition.
        /// </summary>
        /// <param name="definition">Definition which should be added.</param>
        public void AddDefinition(Definition definition)
        {
            m_definitions[definition.SubType] = definition;
        }

        /// <summary>
        /// The definitions.
        /// </summary>
        private ConcurrentDictionary<EntityType, Definition> m_definitions;
    }
}
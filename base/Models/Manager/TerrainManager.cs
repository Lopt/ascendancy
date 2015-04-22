using System;
using System.Collections.Concurrent;
using @base.model.definitions;

namespace @base.model
{
    public class TerrainManager
	{
        public TerrainManager()
        {
            m_terrainDefinitions = new ConcurrentDictionary<Guid, TerrainDefinition> ();
            m_terrainDefintionsByType = new ConcurrentDictionary<TerrainDefinition.TerrainDefinitionType, TerrainDefinition> ();
        }

        public TerrainDefinition GetTerrainDefinition(Guid guid)
        {
            return m_terrainDefinitions[guid];
        }

        public TerrainDefinition GetTerrainDefinition(TerrainDefinition.TerrainDefinitionType type)
        {
            return m_terrainDefintionsByType[type];
        }

        public void AddTerrainDefinition(TerrainDefinition terrainDefinition)
        {
            var guid = terrainDefinition.GUID;
            var terrainType = terrainDefinition.TerrainType;

            m_terrainDefintionsByType[terrainType] = terrainDefinition;
            m_terrainDefinitions[guid] = terrainDefinition;
        }

        private ConcurrentDictionary<Guid, TerrainDefinition> m_terrainDefinitions;
        private ConcurrentDictionary<TerrainDefinition.TerrainDefinitionType, TerrainDefinition> m_terrainDefintionsByType;

	}
}


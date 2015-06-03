using System;
using System.Collections.Concurrent;
using @base.model.definitions;

namespace @base.model
{
    public class TerrainManager
	{
        public TerrainManager()
        {
            m_terrainDefintionsByType = new ConcurrentDictionary<TerrainDefinition.TerrainDefinitionType, TerrainDefinition> ();
        }
            
        public TerrainDefinition GetTerrainDefinition(TerrainDefinition.TerrainDefinitionType type)
        {
            return m_terrainDefintionsByType[type];
        }

        public void AddTerrainDefinition(TerrainDefinition terrainDefinition)
        {
            var terrainType = terrainDefinition.TerrainType;
            m_terrainDefintionsByType[terrainType] = terrainDefinition;
        }

        private ConcurrentDictionary<TerrainDefinition.TerrainDefinitionType, TerrainDefinition> m_terrainDefintionsByType;

	}
}


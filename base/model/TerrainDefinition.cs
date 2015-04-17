using System;

namespace @base.model.definitions
{
    public class TerrainDefinition : Definition
    {
        public enum TerrainDefintionType
        {
            Water = 0,
            Street = 1,
            Building = 2,
            Wood = 3,
        }
      
        public TerrainDefinition(Guid guid, DefinitionType type,
            TerrainDefintionType terrainType, int[] ressources)
            : base(guid, type)
        {
            m_terrainType = terrainType;
            m_ressources = ressources;
        }

        public int[] Ressources
        {
            get { return m_ressources; }
        }

        public TerrainDefintionType TerrainType
        {
            get { return m_terrainType; }
        }

        private TerrainDefintionType m_terrainType;
        private int[] m_ressources;

    }
}


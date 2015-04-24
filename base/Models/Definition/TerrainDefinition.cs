using System;

namespace @base.model.definitions
{
    public class TerrainDefinition : Definition
    {
        public enum TerrainDefinitionType
        {
            Water = 0,
            Buildings = 1,
            Woods = 2,
            Grassland = 3,
            Fields = 4,
            Streets = 5,
            NotDefined = 6,
            Forbidden = 7,
            Town = 8,
            Glacier = 9,
            Beach = 10,
            Park = 11,
            Invalid = 12
        }
      
        public TerrainDefinition(Guid guid, DefinitionType type,
            TerrainDefinitionType terrainType, int[] ressources)
            : base(guid, type)
        {
            m_terrainType = terrainType;
            m_ressources = ressources;
        }

        public int[] Ressources
        {
            get { return m_ressources; }
        }

        public TerrainDefinitionType TerrainType
        {
            get { return m_terrainType; }
        }

        private TerrainDefinitionType m_terrainType;
        private int[] m_ressources;
    }
}


using System;

namespace @base.model.definitions
{
    public class TerrainDefinition : Definition
    {
        public enum TerrainDefinitionType
        {
            // Terrain Range 0-99
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
      
        public TerrainDefinition(TerrainDefinitionType terrainType,
            int[] ressources, bool buildable = true)
            : base((int) terrainType)
        {
            m_ressources = ressources;
            m_buildable = buildable;
        }
        
        public int[] Ressources
        {
            get { return m_ressources; }
        }

        public TerrainDefinitionType TerrainType
        {
            get { return (TerrainDefinitionType) ID; }
        }

        private int[] m_ressources;
        private bool m_buildable;
    }
}


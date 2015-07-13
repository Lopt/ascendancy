using System;

namespace Core.Models.Definitions
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
            ID = id;
        }

        public int ID
        {
            get;
            private set;
        }

        public DefinitionType Type
        {
            get
            {
                if (ID < 60)
                {
                    return DefinitionType.Terrain;
                }
                if (ID < 276)
                {
                    return DefinitionType.Unit;
                }
                if (ID < 1000)
                {
                    return DefinitionType.Building;
                }
                return DefinitionType.Invalid;
            }
        }
    }
}


using System;
using Newtonsoft.Json;

namespace Core.Connections
{
    public class LoadRegionsRequest : Request
    {
        public LoadRegionsRequest(Guid sessionID, Core.Models.Position position, Core.Models.RegionPosition[] regionPositions)
            : base(sessionID, position)
        {
            m_regionPositions = regionPositions;
        }

        public Core.Models.RegionPosition[] RegionPositions
        {
            get
            {
                return m_regionPositions;
            }
            set
            {
                m_regionPositions = value;
            }
        }

        Core.Models.RegionPosition[] m_regionPositions;
    }
}


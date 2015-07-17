using System;
using Newtonsoft.Json;

namespace Core.Connections
{   
    /// <summary>
    /// Request class which should be used to load regions from the server.
    /// Should be serialised before sending, will be deserialised after recieving.
    /// </summary>
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


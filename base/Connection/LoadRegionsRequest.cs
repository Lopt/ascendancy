using System;
using Newtonsoft.Json;

namespace @base.connection
{
    public class LoadRegionsRequest : Request
    {
        public LoadRegionsRequest(Guid sessionID, model.Position position, model.RegionPosition[] regionPositions)
            : base(sessionID, position)
        {
            m_regionPositions = regionPositions;
        }

        public model.RegionPosition[] RegionPositions
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

        model.RegionPosition[] m_regionPositions;
    }
}


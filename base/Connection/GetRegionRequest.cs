using System;
using Newtonsoft.Json;

namespace @base.connection
{
    public class GetRegionRequest : Request
    {
        public GetRegionRequest(Guid sessionID, model.Position position, model.RegionPosition[] regionPositions)
            : base(sessionID, position)
        {
            m_regionPositions = regionPositions;
        }
            
        public model.RegionPosition[] RegionPositions
        {
            get { return m_regionPositions; }
            set { m_regionPositions = value; }
        }

        model.RegionPosition[] m_regionPositions;
    }
}


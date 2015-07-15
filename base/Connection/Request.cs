using System;

namespace Core.Connections
{
    public class Request
    {
        public Request(Guid sessionID, Core.Models.Position position)
        {
            Position = position;
            SessionID = sessionID;
        }

        public Core.Models.Position Position;
        public Guid SessionID;
    }
}


using System;

namespace @base.connection
{
    public class Request
    {
        public Request(Guid sessionID, model.Position position)
        {
            Position = position;
            SessionID = sessionID;
        }

        public model.Position Position;
        public Guid SessionID;
    }
}


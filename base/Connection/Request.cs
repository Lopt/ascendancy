using System;

namespace @base.connection
{
    public class Request
    {
        public Request(Guid sessionID, model.Position position)
        {
            m_position = position;
            m_sessionID = sessionID;
        }

        public model.Position Position
        {
            get { return m_position; }
            set { m_position = value; }
        }

        public Guid SessionID
        {
            get { return m_sessionID; }
            set { m_sessionID = value; }
        }

        model.Position m_position;
        Guid m_sessionID;
    }
}


using System;
using System.Collections.Concurrent;

namespace @base.connection
{
    public class LoginResponse
    {
        public enum ReponseStatus
        {
            OK,
            ERROR,
        }

        public LoginResponse()
        {
            m_status = ReponseStatus.ERROR;
            m_sessionId = Guid.Empty;
            m_accountId = 0;
        }

        public ReponseStatus Status
        {
            get { return m_status; }
            set { m_status = value; }
        }

        public Guid SessionID
        {
            get { return m_sessionId; }
            set { m_sessionId = value; }
        }

        public int AccountId
        {
            get { return m_accountId; }
            set { m_accountId = value; }
        }

        Guid m_sessionId;
        ReponseStatus m_status;
        int m_accountId;

    }
}


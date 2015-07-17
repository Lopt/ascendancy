using System;
using System.Collections.Concurrent;

namespace Core.Connections
{
    /// <summary>
    /// Response class which should be used to login.
    /// Will be serialised before sending, should be deserialised after recieving.
    /// </summary>

    public class LoginResponse
    {
        public enum ReponseStatus
        {
            OK,
            ERROR,
        }

        public LoginResponse()
        {
            Status = ReponseStatus.ERROR;
            SessionID = Guid.Empty;
            AccountId = 0;
        }

        public ReponseStatus Status;
        public Guid SessionID;
        public int AccountId;


    }
}


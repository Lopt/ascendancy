using System;
using System.Collections.Concurrent;

namespace Core.Connections
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
            Status = ReponseStatus.ERROR;
            SessionID = Guid.Empty;
            AccountId = 0;
        }

        public ReponseStatus Status;
        public Guid SessionID;
        public int AccountId;


    }
}


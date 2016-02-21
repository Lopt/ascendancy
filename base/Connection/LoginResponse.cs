namespace Core.Connections
{
    using System;
    using System.Collections.Concurrent;

    /// <summary>
    /// Response class which should be used to login.
    /// Will be serialized before sending, should be deserialized after receiving.
    /// </summary>
    public class LoginResponse
    {
        /// <summary>
        /// Response status of the login response.
        /// </summary>
        public enum ReponseStatus
        {
            OK,
            ERROR,
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Core.Connections.LoginResponse"/> class.
        /// </summary>
        public LoginResponse()
        {
            Status = ReponseStatus.ERROR;
            SessionID = Guid.Empty;
            AccountId = 0;
            ServerTime = DateTime.Now;
        }

        /// <summary>
        /// The Response Status. Only use other data if ResponseStatus == OK
        /// </summary>
        public ReponseStatus Status;

        /// <summary>
        /// The session which the got after the login.
        /// </summary>
        public Guid SessionID;

        /// <summary>
        /// The account identifier which the user has, who logged in.
        /// </summary>
        public int AccountId;

        /// <summary>
        /// The Server time which is be used to synchronize client and server
        /// </summary>
        public DateTime ServerTime;
    }
}

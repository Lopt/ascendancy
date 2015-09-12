namespace Server.Controllers
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.ObjectModel;

    /// <summary>
    /// Manages an account. Which includes Login, holding password and session and the last time when each region was loaded.
    /// </summary>
    public class AccountController : Core.Controllers.ControlEntity
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Server.Controllers.AccountController"/> class.
        /// </summary>
        /// <param name="account">User Account.</param>
        /// <param name="password">User Password.</param>
        public AccountController(Core.Models.Account account, string password)
            : base(account)
        {
            m_regionStatus = new ConcurrentDictionary<Core.Models.RegionPosition, DateTime>(); 
            m_password = password;
            SessionID = Guid.Empty;
        }

        /// <summary>
        /// Every time the client is loading a region, this function should be called with the date time of the last action of the region. 
        /// So it is known which data version the client has.
        /// </summary>
        /// <param name="regionPosition">Region position.</param>
        /// <param name="dateTime">Date time.</param>
        public void RefreshRegion(Core.Models.RegionPosition regionPosition, DateTime dateTime)
        {
            m_regionStatus[regionPosition] = dateTime;
        }

        /// <summary>
        /// Returns the DateTime of the specific region, when the last status was transferred.
        /// </summary>
        /// <returns>A DateTime when the last action of a specific region was transferred. <b>null</b> if it wasn't loaded before.</returns>
        /// <param name="regionPosition">Region Position.</param>
        public DateTime? GetRegionStatus(Core.Models.RegionPosition regionPosition)
        {
            DateTime dateTime = new DateTime();
            if (m_regionStatus.TryGetValue(regionPosition, out dateTime))
            {
                return dateTime;
            }
            return null;
        }

        /// <summary>
        /// Tries to login the user, return true if everything worked.
        /// </summary>
        /// <param name="username">Account Username</param>
        /// <param name="password">Account Password</param>
        public bool Login(string username, string password)
        {
            var account = (Core.Models.Account)Model;
            if (account.UserName == username && password == m_password)
            {
                SessionID = Guid.NewGuid();
                m_regionStatus.Clear();
                return true;
            }
            return false;
        }

        /// <summary>
        /// Gets the session ID.
        /// </summary>
        /// <value>The session ID.</value>
        public Guid SessionID
        {
            get;
            private set;
        }

        /// <summary>
        /// The time when the client updated the region the last time.
        /// </summary>
        ConcurrentDictionary<Core.Models.RegionPosition, DateTime> m_regionStatus;

        /// <summary>
        /// The user password.
        /// </summary>
        string m_password;
    }
}


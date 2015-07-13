using System;
using System.Collections.ObjectModel;
using System.Collections.Concurrent;

namespace @server.control
{
    public class AccountController : Core.Controllers.ControlEntity
    {
        public AccountController(Core.Models.Account account, string password)
            : base(account)
        {
            m_regionStatus = new ConcurrentDictionary<Core.Models.RegionPosition, DateTime>(); 
            m_password = password;
            SessionID = Guid.Empty;
        }

        public void RegionRefreshed(Core.Models.RegionPosition regionPosition, DateTime dateTime)
        {
            m_regionStatus[regionPosition] = dateTime;
        }

        /// <summary>
        /// Returns the DateTime of the specific region, when the last status was transfered.
        /// </summary>
        /// <returns>A DateTime when the last action of a specific region was transfered. <b>null</b> if it wasn't loaded before.</returns>
        /// <param name="region">Region.</param>
        public DateTime? GetRegionStatus(Core.Models.RegionPosition regionPosition)
        {
            DateTime dateTime = new DateTime();
            if (m_regionStatus.TryGetValue(regionPosition, out dateTime))
            {
                return dateTime;
            }
            return null;
        }

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

        public Guid SessionID
        {
            get;
            private set;
        }

        ConcurrentDictionary<Core.Models.RegionPosition, DateTime> m_regionStatus;
        string m_password;
    }
}


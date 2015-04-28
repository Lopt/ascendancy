using System;
using System.Collections.ObjectModel;
using System.Collections.Concurrent;

namespace @server.control
{
	public class Account : @base.control.ControlEntity
    {
		public Account (@base.model.Account account)
			: base(account)
		{
            m_regionStatus = new ConcurrentDictionary<@base.model.Region, DateTime> (); 
		}

        public void RegionRefreshed(@base.model.Region region, DateTime dateTime)
		{
            m_regionStatus [region] = dateTime;
		}

		/// <summary>
		/// Returns the DateTime of the specific region, when the last status was transfered.
		/// </summary>
		/// <returns>A DateTime when the last action of a specific region was transfered. <b>null</b> if it wasn't loaded before.</returns>
		/// <param name="region">Region.</param>
		public DateTime? GetRegionStatus(@base.model.Region region)
		{
			DateTime dateTime = new DateTime();
			if (m_regionStatus.TryGetValue (region, out dateTime))
			{
				return dateTime;
			}
			return null;
		}


		ConcurrentDictionary<@base.model.Region, DateTime> m_regionStatus;
	}
}


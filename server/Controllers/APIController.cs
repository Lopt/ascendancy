using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Collections.Concurrent;
using SQLite;

namespace server.control
{
    public class APIController
    {
		// TODO: find better singleton implementation
		// http://csharpindepth.com/articles/general/singleton.aspx
		// NOT lazy-singletons: throws useless exceptions when initialisation failed
		private static APIController instance=null;

		public static APIController Instance
		{
			get
			{
				if (instance==null)
				{
					instance = new APIController();
				}
				return instance;
			}
		}

		private APIController()
		{
			m_Actions = new ConcurrentQueue<@base.model.Action> ();
		}


        public class RegionData
        {
			public ObservableCollection<ObservableCollection<@base.model.Entity>> EntityDict;
			public ObservableCollection<ObservableCollection<@base.model.Action>> ActionDict;
        }

		public @base.model.Account Login(string username, string password)
		{			
			var controller = @base.control.Controller.Instance;
			var accountManagerC = (control.AccountManagerController)controller.AccountManagerController;
			return accountManagerC.Login (username, password);
		}

		public void DoAction(@base.model.Account account, 
							 @base.model.Action[] actions)
		{
			foreach (var action in actions)
			{
				action.Account = account;
				action.ActionTime = DateTime.Now;

				m_Actions.Enqueue(action);

				Worker ();
			}
		}

        public RegionData LoadRegions(@base.model.Account account,
							         @base.model.RegionPosition[] regionPositions)
		{
			// List<@base.model.Region>, List<@base.control.action.Action> 
			var controller = @base.control.Controller.Instance;
			var regionManagerC = controller.RegionManagerController;
			var accountC = (@server.control.AccountController) account.Control;

			var entityDict = new ObservableCollection<ObservableCollection<@base.model.Entity>> ();
			var actionDict = new ObservableCollection<ObservableCollection<@base.model.Action>> ();

			foreach (var regionPosition in regionPositions)
			{
				var region = regionManagerC.GetRegion (regionPosition);
				if (region.Exist)
				{
					var status = accountC.GetRegionStatus (region);
                    var newStatus = new DateTime ();
                    // account has already loaded the region - now just load changes (actions)
					if (status != null)
                    { 
                        var actions = region.GetCompletedActions (status.Value);
						actionDict.Add(actions.Actions);
                        newStatus = actions.DateTime;
                    }
                    // account hasn't loaded the region
					else
                    {
                        var entities = region.GetEntities();
						entityDict.Add(entities.Entities);
                        newStatus = entities.DateTime;
                    }

                    accountC.RegionRefreshed (region, newStatus);
				}
			}
            var regionData = new RegionData ();
            regionData.ActionDict = actionDict;
            regionData.EntityDict = entityDict;
            return regionData;
			//return regionList;
		}

		public bool WorkAction(@base.model.Action action)
		{
			if (action != null)
			{
				var gotLocked = new HashSet<@base.model.Region> ();
				try
				{
					var affectedRegions = action.GetAffectedRegions();
					foreach (var region in affectedRegions)
					{
						if (region.TryLockRegion())
						{
							gotLocked.Add(region);
						}
						else
						{
							break;
						}
					}


					if (gotLocked.Count == affectedRegions.Count)
					{
						if (action.Possible())
						{
							var changedRegions = action.Do();
							if (changedRegions.Count != 0)
							{
								foreach (var region in changedRegions)
								{
									region.AddCompletedAction(action);
								}
								return true;
							}
							else
							{
								action.Catch ();
								return false;
							}
						}
					}
				}
				catch
				{
					return false;
				}
				finally
				{
					foreach (var region in gotLocked)
					{
						region.Release ();
					}
				}
			}
			return false;
		}


		public void Worker()
		{
			@base.model.Action action;

			//while (true)
			//{
				if (m_Actions.TryDequeue (out action))
				{
					if (!WorkAction (action))
					{
						m_Actions.Enqueue (action);
					}
				}
			//}
		}

		ConcurrentQueue<@base.model.Action> m_Actions;
    }
}

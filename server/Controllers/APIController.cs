using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Collections.Concurrent;
using SQLite;
using System.Threading;

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
			m_hasNewAction = false;
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
			return accountManagerC.Registrate (username, password);
			//return accountManagerC.Login (username, password);
		}

		public void DoAction(@base.model.Account account, 
							 @base.model.Action[] actions)
		{
			foreach (var action in actions)
			{
				action.Account = account;
				action.ActionTime = DateTime.Now;

				m_Actions.Enqueue(action);
			}
			m_hasNewAction = true;
		}

        public RegionData LoadRegions(@base.model.Account account,
							         @base.model.RegionPosition[] regionPositions)
		{
			// List<@base.model.Region>, List<@base.control.action.Action> 
			var controller = @base.control.Controller.Instance;
			var regionManagerC = controller.RegionStatesController.Curr;

			var accountC = (@server.control.AccountController) account.Control;

			var entityDict = new ObservableCollection<ObservableCollection<@base.model.Entity>> ();
			var actionDict = new ObservableCollection<ObservableCollection<@base.model.Action>> ();

			foreach (var regionPosition in regionPositions)
			{
				var region = regionManagerC.GetRegion (regionPosition);
				if (region.Exist)
				{
					var status = accountC.GetRegionStatus (regionPosition);
                    var newStatus = new DateTime ();
                    // account has already loaded the region - now just load changes (actions)
					if (status == null)
                    { 
						var entities = region.GetEntities();
						// TODO: remove entity creation
						var position = new @base.model.PositionI(region.RegionPosition.RegionX * @base.model.Constants.REGION_SIZE_X, region.RegionPosition.RegionY * @base.model.Constants.REGION_SIZE_Y);
						entities.Entities.Add(new @base.model.Entity(@base.model.IdGenerator.GetId(),
											 @base.model.World.Instance.DefinitionManager.GetDefinition(60),
							position));
                       

						entityDict.Add(entities.Entities);
						newStatus = entities.DateTime;
						status = new System.DateTime();
                    }
                    // account hasn't loaded the region
					var actions = region.GetCompletedActions (status.Value);
					actionDict.Add(actions.Actions);
					newStatus = actions.DateTime;

					accountC.RegionRefreshed (regionPosition, newStatus);
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
				var regionStatesController = @base.control.Controller.Instance.RegionStatesController;

				var actionC = (action.Control as @base.control.action.Action);
				var gotLocked = new HashSet<@base.model.Region> ();

				try
				{
					var affectedRegions = actionC.GetAffectedRegions(regionStatesController.Next);
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
						if (actionC.Possible(regionStatesController.Next))
						{
							var changedRegions = actionC.Do(regionStatesController.Next);
							if (changedRegions.Count != 0)
							{
								foreach (var region in changedRegions)
								{
									var regionCurr = regionStatesController.Curr.GetRegion(region.RegionPosition);
									regionCurr.AddCompletedAction(action);
								}
								return true;
							}
							else
							{
								actionC.Catch (regionStatesController.Next);
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


		public void Worker(object state)
		{
			@base.model.Action action;

			while (MvcApplication.Phase != MvcApplication.Phases.Exit)
			{
				while (!m_hasNewAction)
				{
					Thread.Sleep (model.ServerConstants.ACTION_THREAD_SLEEP);
				}

				if (m_Actions.TryDequeue (out action))
				{
					action.ActionTime = DateTime.Now;
					if (!WorkAction (action))
					{
						m_Actions.Enqueue (action);
					}
				}

				m_hasNewAction = !m_Actions.IsEmpty;
			}
		}

		bool m_hasNewAction;
		ConcurrentQueue<@base.model.Action> m_Actions;

    }
}

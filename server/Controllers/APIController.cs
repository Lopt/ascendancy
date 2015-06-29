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

        public class AveragePositionQueue
        {
            public AveragePositionQueue()
            {
                Count = 0;
                Average = new @base.model.Position(0, 0);
                Lock = new Mutex();

                Queue = new Queue<@base.model.Action> ();
            }

            public int Count = 0;
            public @base.model.Position Average;
            public Mutex Lock;
            public Queue<@base.model.Action> Queue;
        }

		private APIController()
		{
            m_threadingInfos = new AveragePositionQueue[model.ServerConstants.ACTION_THREADS];
            for (int nr = 0; nr < model.ServerConstants.ACTION_THREADS; ++nr)
            {
                m_threadingInfos [nr] = new AveragePositionQueue ();
            }
        }
			

        public class RegionData
        {
            public LinkedList<LinkedList<@base.model.Entity>> EntityDict;
            public LinkedList<LinkedList<@base.model.Action>> ActionDict;
        }

		public @base.model.Account Login(string username, string password)
		{	
            if (username != null && password != null)
            {
                var controller = @base.control.Controller.Instance;
                var accountManagerC = (control.AccountManagerController)controller.AccountManagerController;
                return accountManagerC.Registrate (username, password);
            }
            return null;
    			//return accountManagerC.Login (username, password);
		}

		public void DoAction(@base.model.Account account, 
							 @base.model.Action[] actions)
		{
			foreach (var action in actions)
			{
                var bestQueue = 0;
                var actionC = (@base.control.action.Action)action.Control;
                var actionPosition = new @base.model.Position (actionC.GetRegionPosition ());

				action.Account = account;
				action.ActionTime = DateTime.Now;

                for (int queueNr = 0; queueNr < model.ServerConstants.ACTION_THREADS; ++queueNr)
                {
                    if (m_threadingInfos[queueNr].Count == 0)
                    {   
                        bestQueue = queueNr;
                        break;
                    }

                    if (m_threadingInfos[queueNr].Average.Distance(actionPosition) < m_threadingInfos[bestQueue].Average.Distance(actionPosition))
                    {
                        bestQueue = queueNr;  
                    }
                }

                var bestThread = m_threadingInfos[bestQueue];
                try
                {

                    bestThread.Lock.WaitOne ();

                    var newAverageX = bestThread.Average.X * bestThread.Count + actionPosition.X; 
                    var newAverageY = bestThread.Average.Y * bestThread.Count + actionPosition.Y;                            
                    bestThread.Count += 1;
                    bestThread.Average = new @base.model.Position (newAverageX / bestThread.Count, newAverageY / bestThread.Count);

                    bestThread.Queue.Enqueue(action);  
                }
                finally
                {
                    bestThread.Lock.ReleaseMutex ();
                }
            }
		}

        public RegionData LoadRegions(@base.model.Account account,
							         @base.model.RegionPosition[] regionPositions)
		{
			// List<@base.model.Region>, List<@base.control.action.Action> 
			var controller = @base.control.Controller.Instance;
			var regionManagerC = controller.RegionStatesController.Curr; // TODO Curr

			var accountC = (@server.control.AccountController) account.Control;

            var entityDict = new LinkedList<LinkedList<@base.model.Entity>> ();
            var actionDict = new LinkedList<LinkedList<@base.model.Action>> ();

            var lockedRegions = new LinkedList<@base.model.Region> ();
            try
            {
                foreach (var regionPosition in regionPositions)
                {                    
                    var region = regionManagerC.GetRegion (regionPosition);
                    if (region.Exist)
                    {

                        System.Console.WriteLine("E1");
                        region.LockReader ();
                        System.Console.WriteLine("E1+");
                        lockedRegions.AddLast(region);
                    }
                }

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
    						//var position = new @base.model.PositionI(region.RegionPosition.RegionX * @base.model.Constants.REGION_SIZE_X, region.RegionPosition.RegionY * @base.model.Constants.REGION_SIZE_Y);
                            //entities.Entities.AddLast(new @base.model.Entity(@base.model.IdGenerator.GetId(),
                            //					 @base.model.World.Instance.DefinitionManager.GetDefinition(60),
                            //	position));
                           //entityDict.AddFirst(entities.Entities);
                            //newStatus = entities.DateTime;
    						status = new System.DateTime();
                        }
                        // account hasn't loaded the region
    					var actions = region.GetCompletedActions (status.Value);
    					actionDict.AddFirst(actions.Actions);
    					newStatus = actions.DateTime;

    					accountC.RegionRefreshed (regionPosition, newStatus);
    				}
    			}
            }
            finally
            {
                foreach (var region in lockedRegions)
                {
                    region.ReleaseReader ();
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
            var succeed = false;
			if (action != null)
			{
				var regionStatesController = @base.control.Controller.Instance.RegionStatesController;

				var actionC = (action.Control as @base.control.action.Action);
                var gotLocked = new LinkedList<@base.model.Region> () {};

				try
				{
					var affectedRegions = actionC.GetAffectedRegions(regionStatesController.Next);
					foreach (var region in affectedRegions)
					{
                        if (region.Exist)
                        {
                            //System.Console.WriteLine("A1");

                            if (region.LockWriter())
    						{
    							gotLocked.AddLast(region);
                                //    System.Console.WriteLine("A1+");
    						}
    						else
    						{
                                //System.Console.WriteLine("A1-");
                                break;
    						}

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


                                succeed = true;
							}
							else
							{
								//actionC.Catch (regionStatesController.Next);
                                succeed = true;
							}
						}
					}
                }
				catch
				{
				}
				finally
                {
                    foreach (var region in gotLocked)
                    {
                        region.ReleaseWriter ();
                    }
				}
			
            }


            return succeed;
		}

		public void Worker(object state)
		{
            var threadInfo = m_threadingInfos[(int)state];
			@base.model.Action action;
           
			while (MvcApplication.Phase != MvcApplication.Phases.Exit)
//            while (m_Running)
			{
                while (threadInfo.Count == 0 || MvcApplication.Phase == MvcApplication.Phases.Pause)
				{
					Thread.Sleep (model.ServerConstants.ACTION_THREAD_SLEEP);
				}

                try
                {
                    threadInfo.Lock.WaitOne ();
                    action = threadInfo.Queue.Dequeue();

                    action.ActionTime = DateTime.Now;
                    var actionC = (@base.control.action.Action)action.Control;
                    var actionPosition = new @base.model.Position (actionC.GetRegionPosition ());
                    var newAverageX = threadInfo.Average.X * threadInfo.Count - actionPosition.X; 
                    var newAverageY = threadInfo.Average.Y * threadInfo.Count - actionPosition.Y;                            
                    threadInfo.Count -= 1;
                    threadInfo.Average = new @base.model.Position (newAverageX / threadInfo.Count, newAverageY / threadInfo.Count);
                    }
                finally
                {
                    threadInfo.Lock.ReleaseMutex ();
                }

                if (!WorkAction (action))
                {   
                    APIController.Instance.DoAction (action.Account, new @base.model.Action[]{action, });
                }
			}
		}

        public AveragePositionQueue[] m_threadingInfos;
    }
}

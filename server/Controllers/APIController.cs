using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Collections.Concurrent;
using SQLite;
using System.Threading;

namespace server.control
{
    public sealed class APIController
    {
        private static readonly Lazy<APIController> m_singleton =
            new Lazy<APIController>(() => new APIController());

        public static APIController Instance
        {
            get
            {
                return m_singleton.Value;
            }
        }

        private APIController()
        {
            m_threadingInfos = new AveragePositionQueue[model.ServerConstants.ACTION_THREADS];
            for (int nr = 0; nr < model.ServerConstants.ACTION_THREADS; ++nr)
            {
                m_threadingInfos[nr] = new AveragePositionQueue();
            }
        }


        public class AveragePositionQueue
        {
            public AveragePositionQueue()
            {
                Count = 0;
                Average = new Core.Models.Position(0, 0);
                Lock = new Mutex();

                Queue = new Queue<Core.Models.Action>();
            }

            public int Count = 0;
            public Core.Models.Position Average;
            public Mutex Lock;
            public Queue<Core.Models.Action> Queue;
        }

        private enum ActionReturn
        {
            Done,
            NotPossible,
            RessourceBlocked,
            InternalError,
            Exception,
            Unknown,
            RegionDontExist

        }

        public class RegionData
        {
            public LinkedList<LinkedList<Core.Models.Entity>> EntityDict;
            public LinkedList<LinkedList<Core.Models.Action>> ActionDict;
        }

        public Core.Models.Account Login(string username, string password)
        {	
            if (username != null && password != null)
            {
                var controller = Core.Controllers.Controller.Instance;
                var accountManagerC = (control.AccountManagerController)Core.Models.World.Instance.AccountManager;
                return accountManagerC.Registrate(username, password);
            }
            return null;
            //return accountManagerC.Login (username, password);
        }

        public void DoAction(Core.Models.Account account, 
                             Core.Models.Action[] actions)
        {
            foreach (var action in actions)
            {
                var bestQueue = 0;
				var actionC = (Core.Controllers.Actions.Action)action.Control;
                var actionPosition = new Core.Models.Position(actionC.GetRegionPosition());

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

                    bestThread.Lock.WaitOne();

                    var newAverageX = bestThread.Average.X * bestThread.Count + actionPosition.X; 
                    var newAverageY = bestThread.Average.Y * bestThread.Count + actionPosition.Y;                            
                    bestThread.Count += 1;
                    bestThread.Average = new Core.Models.Position(newAverageX / bestThread.Count, newAverageY / bestThread.Count);

                    bestThread.Queue.Enqueue(action);  
                }
                finally
                {
                    bestThread.Lock.ReleaseMutex();
                }
            }
        }

        public RegionData LoadRegions(Core.Models.Account account,
                                      Core.Models.RegionPosition[] regionPositions)
        {
            // List<@base.model.Region>, List<@base.control.action.Action> 
            var controller = Core.Controllers.Controller.Instance;
            var regionManagerC = controller.RegionManagerController;

            var accountC = (@server.control.AccountController)account.Control;

            var entityDict = new LinkedList<LinkedList<Core.Models.Entity>>();
            var actionDict = new LinkedList<LinkedList<Core.Models.Action>>();

            var lockedRegions = new LinkedList<Core.Models.Region>();
            try
            {
                foreach (var regionPosition in regionPositions)
                {                    
                    var region = regionManagerC.GetRegion(regionPosition);
                    if (region.Exist)
                    {
                        region.LockReader();
                        lockedRegions.AddLast(region);
                    }
                }

                foreach (var regionPosition in regionPositions)
                {
                    var region = regionManagerC.GetRegion(regionPosition);
                    if (!region.Exist)
                    {
                        continue;
                    }

                    var status = accountC.GetRegionStatus(regionPosition);
                    var newStatus = new DateTime();
                    // account has already loaded the region - now just load changes (actions)
                    if (status == null)
                    {  
                        var entities = region.GetEntities();
                        entityDict.AddFirst(entities.Entities);
                        newStatus = entities.DateTime;
                    }
                    else
                    {
                        // account hasn't loaded the region
                        var actions = region.GetCompletedActions(status.Value);
                        actionDict.AddFirst(actions.Actions);
                        newStatus = actions.DateTime;
                    }
                    accountC.RegionRefreshed(regionPosition, newStatus);
                }
            }
            finally
            {
                foreach (var region in lockedRegions)
                {
                    region.ReleaseReader();
                }
            }

            var regionData = new RegionData();
            regionData.ActionDict = actionDict;
            regionData.EntityDict = entityDict;
            return regionData;
            //return regionList;
        }

        private ActionReturn WorkAction(Core.Models.Action action)
        {
            if (action != null)
            {
                var regionManager = Core.Controllers.Controller.Instance.RegionManagerController;

                var actionC = (action.Control as Core.Controllers.Actions.Action);
                var gotLocked = new LinkedList<Core.Models.Region>() { };

                try
                {
                    var affectedRegions = actionC.GetAffectedRegions();
                    foreach (var region in affectedRegions)
                    {
                        if (!region.Exist)
                        {
                            return ActionReturn.RegionDontExist;
                        }

                        if (region.LockWriter())
                        {
                            gotLocked.AddLast(region);
                        }
                        else
                        {
                            break;
                        }
                    }


                    if (gotLocked.Count != affectedRegions.Count)
                    {
                        return ActionReturn.RessourceBlocked;
                    }
                    if (!actionC.Possible())
                    {
                        return ActionReturn.NotPossible;
                    }

                    action.ID = Core.Models.IdGenerator.GetId();
                    var changedRegions = actionC.Do();
                    if (changedRegions.Count == 0)
                    {
                        //actionC.Catch (regionStatesController.Next);
                        return ActionReturn.InternalError;
                    }


                    foreach (var region in changedRegions)
                    {
                        var regionCurr = regionManager.GetRegion(region.RegionPosition);
                        regionCurr.AddCompletedAction(action);
                    }
                    return ActionReturn.Done;
                }
                catch
                {
                    return ActionReturn.Exception;
                }
                finally
                {
                    foreach (var region in gotLocked)
                    {
                        region.ReleaseWriter();
                    }
                }
            }

            return ActionReturn.Unknown;
        }

        public void Worker(object state)
        {
            var threadInfo = m_threadingInfos[(int)state];
            Core.Models.Action action;
           
            while (MvcApplication.Phase != MvcApplication.Phases.Exit)
            {
                while (threadInfo.Count == 0 || MvcApplication.Phase == MvcApplication.Phases.Pause)
                {
                    Thread.Sleep(model.ServerConstants.ACTION_THREAD_SLEEP);
                }

                try
                {
                    threadInfo.Lock.WaitOne();
                    action = threadInfo.Queue.Dequeue();

                    action.ActionTime = DateTime.Now;
                    var actionC = (Core.Controllers.Actions.Action)action.Control;
                    var actionPosition = new Core.Models.Position(actionC.GetRegionPosition());
                    var newAverageX = threadInfo.Average.X * threadInfo.Count - actionPosition.X; 
                    var newAverageY = threadInfo.Average.Y * threadInfo.Count - actionPosition.Y;                            
                    threadInfo.Count -= 1;
                    threadInfo.Average = new Core.Models.Position(newAverageX / threadInfo.Count, newAverageY / threadInfo.Count);
                }
                finally
                {
                    threadInfo.Lock.ReleaseMutex();
                }

                if (WorkAction(action) == ActionReturn.RessourceBlocked)
                {   
                    APIController.Instance.DoAction(action.Account, new Core.Models.Action[]{ action, });
                }
            }
        }

        public AveragePositionQueue[] m_threadingInfos;
    }
}

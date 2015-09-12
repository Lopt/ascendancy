namespace Server.Controllers
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Threading;
    using SQLite;

    /// <summary>
    /// API Controller provides functionality to access the game. 
    /// Every access from outside should use API Controller.
    /// As example HTTPController uses only API Controller.
    /// If we later switch to another protocol (or directly sockets) we just need
    /// to build another HTTP Controller-like which uses the API Controller functionality.
    /// </summary>
    public sealed class APIController
    {
        /// <summary>
        /// The singleton instance.
        /// </summary>
        private static readonly Lazy<APIController> Singleton =
            new Lazy<APIController>(() => new APIController());

        /// <summary>
        /// Gets the singleton instance.
        /// </summary>
        /// <value>The instance.</value>
        public static APIController Instance
        {
            get
            {
                return Singleton.Value;
            }
        }

        /// <summary>
        /// Prevents a default instance of the <see cref="Server.Controllers.APIController"/> class from being created.
        /// </summary>
        private APIController()
        {
            m_threads = new Models.AveragePositionQueue[Models.ServerConstants.ACTION_THREADS];
            for (int nr = 0; nr < Models.ServerConstants.ACTION_THREADS; ++nr)
            {
                m_threads[nr] = new Models.AveragePositionQueue();
            }
        }

        /// <summary>
        /// Possible return values, what could have been happen
        /// </summary>
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

        /// <summary>
        /// Region data.
        /// </summary>
        public class RegionData
        {
            /// <summary>
            /// The entities in every region.
            /// </summary>
            public LinkedList<LinkedList<Core.Models.Entity>> EntityDict;

            /// <summary>
            /// The actions in every region.
            /// </summary>
            public LinkedList<LinkedList<Core.Models.Action>> ActionDict;
        }

        /// <summary>
        /// Login with a username und password, returns Account if everything worked, otherwise <b>null</b>
        /// </summary>
        /// <param name="username">Account Username.</param>
        /// <param name="password">Account Password.</param>
        /// <returns>Account if login worked. Otherwise <b>null</b></returns>
        public Core.Models.Account Login(string username, string password)
        {    
            if (username != null && password != null)
            {
                var controller = Core.Controllers.Controller.Instance;
                var accountManagerC = (Server.Controllers.AccountManagerController)Core.Models.World.Instance.AccountManager;
                return accountManagerC.Registrate(username, password);
            }
            return null;
        }

        /// <summary>
        /// Send Actions the server so they will be executed.
        /// (but first, the actions have to wait in a queue)
        /// </summary>
        /// <param name="account">Account who wants the actions executed.</param>
        /// <param name="actions">Array of Actions which should be executed.</param>
        public void DoAction(Core.Models.Account account, Core.Models.Action[] actions)
        {
            foreach (var action in actions)
            {
                var bestThread = m_threads[0];

                action.Account = account;
                action.ActionTime = DateTime.Now;

                var actionC = (Core.Controllers.Actions.Action)action.Control;
                var actionPosition = new Core.Models.Position(actionC.GetRegionPosition());

                for (int queueNr = 0; queueNr < Models.ServerConstants.ACTION_THREADS; ++queueNr)
                {
                    var thread = m_threads[queueNr];

                    if (thread.IsEmpty())
                    {   
                        bestThread = thread;
                        break;
                    }
                    else if (thread.Distance(actionPosition) < bestThread.Distance(actionPosition))
                    {
                        bestThread = thread;  
                    }
                }
                bestThread.Enqueue(action);
            }
        }

        /// <summary>
        /// Loads the regions.
        /// </summary>
        /// <returns>The regions.</returns>
        /// <param name="account">Account who wants the load the regions.</param>
        /// <param name="regionPositions">Positions of the regions.</param>
        public RegionData LoadRegions(
            Core.Models.Account account,
            Core.Models.RegionPosition[] regionPositions)
        {
            // List<@base.model.Region>, List<@base.control.action.Action> 
            var controller = Core.Controllers.Controller.Instance;
            var regionManagerC = controller.RegionManagerController;

            var accountC = (Server.Controllers.AccountController)account.Control;

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
                    accountC.RefreshRegion(regionPosition, newStatus);
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
        }

        /// <summary>
        /// Executes the action.
        /// </summary>
        /// <returns>If it could executed or why not.</returns>
        /// <param name="action">Action which should be executed.</param>
        private ActionReturn WorkAction(Core.Models.Action action)
        {
            if (action != null)
            {
                var regionManager = Core.Controllers.Controller.Instance.RegionManagerController;

                var actionC = action.Control as Core.Controllers.Actions.Action;
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
                        // actionC.Catch (regionStatesController.Next);
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

        /// <summary>
        /// Worker (normally an own thread) which runs until the application finishes.
        /// </summary>
        /// <param name="state">Threading Number (for queue association)</param>
        public void Worker(object state)
        {
            var thread = m_threads[(int)state];
            Core.Models.Action action;
           
            while (MvcApplication.Phase != MvcApplication.Phases.Exit)
            {
                while (thread.IsEmpty() || MvcApplication.Phase == MvcApplication.Phases.Pause)
                {
                    Thread.Sleep(Models.ServerConstants.ACTION_THREAD_SLEEP);
                }

                action = thread.Dequeue();
                if (WorkAction(action) == ActionReturn.RessourceBlocked)
                {   
                    APIController.Instance.DoAction(action.Account, new Core.Models.Action[] { action });
                }
            }
        }

        /// <summary>
        /// The active threads.
        /// </summary>
        private Models.AveragePositionQueue[] m_threads;
    }
}

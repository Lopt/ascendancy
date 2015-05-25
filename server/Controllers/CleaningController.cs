using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Threading;
using System.Collections.Concurrent;

namespace server.control
{
    public class CleaningController
    {
		public CleaningController ()
		{
		}

		public void Run(object state)
		{
			while (MvcApplication.Phase != MvcApplication.Phases.Exit)
			{
				Thread.Sleep (server.model.ServerConstants.CLEANING_INTERVALL);

				var currentStates = @base.model.World.Instance.RegionStates;
				var currentStatesC = @base.control.Controller.Instance.RegionStatesController;

				var newState = new @base.model.RegionManager (currentStates.Next);
				var newStateC = new server.control.RegionManagerController ((server.control.RegionManagerController) currentStatesC.Curr, newState);

//				newState.Regions = new ConcurrentDictionary<@base.model.RegionPosition, @base.model.Region> (currentStates.Next.Regions);

				var nextStates = new @base.model.RegionStates (currentStates.Curr, currentStates.Next, newState);
				var nextStatesC = new @base.control.RegionStatesController (currentStatesC.Curr, currentStatesC.Next, newStateC);

				@base.model.World.Instance.RegionStates = nextStates;
				@base.control.Controller.Instance.RegionStatesController = nextStatesC;

				// removes old RegionManager, so the garbage collector can clean unused space up 
				nextStatesC.Last.Parent = null;
			}
        }
    }
}

using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;

namespace server.control
{
    public class APIController
    {
        public class RegionData
        {
            public Dictionary<@base.model.RegionPosition, @base.model.Region.DatedEntities> EntityDict;
            public Dictionary<@base.model.RegionPosition, @base.model.Region.DatedActions>  ActionDict;
        }


        public APIController()
        {
        }

		public void DoAction(@base.model.Account account, 
							 @base.control.action.Action[] actions)
		{
			foreach (var action in actions)
			{
				if (action.Account == account)
				{
					var region = action.GetMainRegion ();
					region.AddAction (action);
				}
			}
		}

        public RegionData LoadRegions(@base.model.Account account,
							         @base.model.RegionPosition[] regionPositions)
		{
			// List<@base.model.Region>, List<@base.control.action.Action> 
			var controller = @base.control.Controller.Instance;
			var regionManagerC = controller.RegionManagerController;
			var accountC = (@server.control.Account) account.Control;

            var entityDict = new Dictionary<@base.model.RegionPosition, @base.model.Region.DatedEntities> ();
            var actionDict = new Dictionary<@base.model.RegionPosition, @base.model.Region.DatedActions> ();

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
                        actionDict [region.RegionPosition] = actions;
                        newStatus = actions.DateTime;
                    }
                    // account hasn't loaded the region
					else
                    {
                        var entities = region.GetEntities ();
                        entityDict [region.RegionPosition] = entities;
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


		public void Worker(@base.model.Region[] regions)
		{
			foreach (var region in regions)
			{
				var action = region.GetAction ();
				if (action.Possible ())
				{
					if (!action.Do ())
					{
						action.Catch ();
					}
				}
				region.ActionCompleted ();
			}
		}
    }
}

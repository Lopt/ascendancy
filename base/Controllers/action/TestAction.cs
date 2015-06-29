using System;
using System.Collections.Concurrent;

using @base.control;
using @base.model.definitions;

namespace @base.control.action
{
    public class TestAction : Action
    {
        public TestAction(model.ModelEntity model)
            : base(model)
        {
        }


        public const string REGIONS = "Regions";

        /// <summary>
        /// Initializes a new instance of the <see cref="base.control.action.Action"/> class.
        /// </summary>
        /// <param name="actionType">Action type.</param>
        /// <param name="regions">Affected Regions of this action.</param>
        /// <param name="parameters">Parameters.</param>
        override public ConcurrentBag<model.Region> GetAffectedRegions(RegionManagerController regionManagerC)
        {
            /*
            var action = (model.Action)Model;
            var regions = new ConcurrentBag<model.Region> ();
            foreach (Newtonsoft.Json.Linq.JObject regionPosition in (Newtonsoft.Json.Linq.JContainer) action.Parameters[REGIONS])
            {
                regions.Add(regionManagerC.GetRegion(new @base.model.RegionPosition(regionPosition)));
            }
            return regions;*/
            var action = (model.Action)Model;
            return (ConcurrentBag<model.Region>) action.Parameters[REGIONS];

        }

        override public @base.model.RegionPosition GetRegionPosition()
        {
            var action = (model.Action)Model;
            return ((ConcurrentBag<model.Region>) action.Parameters[REGIONS]).ToArray()[0].RegionPosition;
        }


        /// <summary>
        /// Returns if the action is even possible.
        /// </summary>
        public override bool Possible(RegionManagerController regionManagerC)
        {   
            return true;
        }

        /// <summary>
        /// Apply action-related changes to the world.
        /// Returns false if something went terrible wrong
        /// </summary>
        public override ConcurrentBag<model.Region> Do(RegionManagerController regionManagerC)
        {   
            var action = (model.Action)Model;
            return (ConcurrentBag<model.Region>) action.Parameters[REGIONS];
            /*
            var action = (model.Action)Model;
            var regions = new ConcurrentBag<model.Region> ();
            foreach (Newtonsoft.Json.Linq.JObject regionPosition in (Newtonsoft.Json.Linq.JContainer) action.Parameters[REGIONS])
            {
                regions.Add(regionManagerC.GetRegion(new @base.model.RegionPosition(regionPosition)));
            }
            return regions;
            */
        }

        /// <summary>
        /// In case of errors, revert the world data to a valid state.
        /// </summary>
        public override bool Catch(RegionManagerController regionManagerC)
        {
            throw new NotImplementedException();
        }
    }
}


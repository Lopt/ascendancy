using System;
using System.Collections.Concurrent;

using Core.Controllers.Actions;
using Core.Models.Definitions;

namespace Core.Controllers.Actions
{
    public class TestAction : Action
    {
        public TestAction(Core.Models.ModelEntity model)
            : base(model)
        {
        }


        public const string REGIONS = "Regions";

        /// <summary>
        /// Initializes a new instance of the <see cref="Core.Controllers.Actions.Action"/> class.
        /// </summary>
        /// <param name="actionType">Action type.</param>
        /// <param name="regions">Affected Regions of this action.</param>
        /// <param name="parameters">Parameters.</param>
        override public ConcurrentBag<Core.Models.Region> GetAffectedRegions()
        {
            var action = (Core.Models.Action)Model;
            return (ConcurrentBag<Core.Models.Region>)action.Parameters[REGIONS];

        }

        override public Core.Models.RegionPosition GetRegionPosition()
        {
            var action = (Core.Models.Action)Model;
            return ((ConcurrentBag<Core.Models.Region>)action.Parameters[REGIONS]).ToArray()[0].RegionPosition;
        }


        /// <summary>
        /// Returns if the action is even possible.
        /// </summary>
        public override bool Possible()
        {   
            return true;
        }

        /// <summary>
        /// Apply action-related changes to the world.
        /// Returns false if something went terrible wrong
        /// </summary>
        public override ConcurrentBag<Core.Models.Region> Do()
        {   
            var action = (Core.Models.Action)Model;
            return (ConcurrentBag<Core.Models.Region>)action.Parameters[REGIONS];
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
        public override bool Catch()
        {
            throw new NotImplementedException();
        }
    }
}


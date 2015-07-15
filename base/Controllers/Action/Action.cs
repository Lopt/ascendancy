using System;
using System.Collections.Concurrent;
using Newtonsoft.Json;

namespace Core.Controllers.Actions
{
    public class Action : Core.Controllers.ControlEntity
    {
        public Action(Core.Models.ModelEntity model)
            : base(model)
        {
        }

        virtual public ConcurrentBag<Core.Models.Region> GetAffectedRegions()
        {
            throw new NotImplementedException();
        }

        virtual public Core.Models.RegionPosition GetRegionPosition()
        {
            throw new NotImplementedException();
        }



        /// <summary>
        /// Returns if the action is even possible.
        /// </summary>
        virtual public bool Possible()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Apply action-related changes to the world.
        /// Returns set of changed Regions if everything worked, otherwise null
        /// </summary>
        virtual public ConcurrentBag<Core.Models.Region> Do()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// In case of errors, revert the world data to a valid state.
        /// </summary>
        virtual public bool Catch()
        {
            throw new NotImplementedException();
        }
    }
}


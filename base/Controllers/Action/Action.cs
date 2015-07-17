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

        /// <summary>
        /// Returns a bag of all regions which could be affected by this action.
        /// </summary>
        /// <returns>The affected regions.</returns>
        virtual public ConcurrentBag<Core.Models.Region> GetAffectedRegions()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns the Position where the action should be executed (e.g. first region)
        /// </summary>
        /// <returns>Action execution RegionPosition.</returns>
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


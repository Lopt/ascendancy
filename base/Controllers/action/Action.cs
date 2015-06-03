using System;
using System.Collections.Concurrent;
using Newtonsoft.Json;

namespace @base.control.action
{
    public class Action : control.ControlEntity
    {
        public Action(model.ModelEntity model)
            : base(model)
        {
        }

        virtual public ConcurrentBag<model.Region> GetAffectedRegions(RegionManagerController regionManagerC)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns if the action is even possible.
        /// </summary>
        virtual public bool Possible(RegionManagerController regionManagerC)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Apply action-related changes to the world.
        /// Returns set of changed Regions if everything worked, otherwise null
        /// </summary>
        virtual public ConcurrentBag<model.Region> Do(RegionManagerController regionManagerC)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// In case of errors, revert the world data to a valid state.
        /// </summary>
        virtual public bool Catch(RegionManagerController regionManagerC)
        {
            throw new NotImplementedException();
        }
    }
}


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

        virtual public model.Region GetMainRegion()
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
        /// Returns True if everything worked, otherwise False
        /// </summary>
        virtual public bool Do()
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


namespace Core.Controllers.Actions
{
    using System;
    using System.Collections.Concurrent;
    using Newtonsoft.Json;

    /// <summary>
    /// Action class. Should be used as base for each other action.
    /// Actions are part of the game logic, which can be used from the server or the client to manipulate the world.
    /// Every action should do deterministic calculations, so the client can calculate the current game state by an old game state and the list of actions which were executed since the last loaded game state.
    /// </summary>
    public class Action : Core.Controllers.ControlEntity
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Core.Controllers.Actions.Action"/> class.
        /// </summary>
        /// <param name="model">action model.</param>
        public Action(Core.Models.ModelEntity model)
            : base(model)
        {
        }

        /// <summary>
        /// Returns a bag of all regions which could be affected by this action.
        /// </summary>
        /// <returns>The affected regions.</returns>
        public virtual ConcurrentBag<Core.Models.Region> GetAffectedRegions()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns the Position where the action should be executed (e.g. first region)
        /// </summary>
        /// <returns>Action execution RegionPosition.</returns>
        public virtual Core.Models.RegionPosition GetRegionPosition()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns if the action is even possible.
        /// </summary>
        /// <returns>true if this is action possible</returns>
        public virtual bool Possible()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Apply action-related changes to the world.
        /// Returns set of changed Regions if everything worked, otherwise null
        /// </summary>
        /// <returns>all affected (changed) regions</returns>
        public virtual ConcurrentBag<Core.Models.Region> Do()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// If an error occurred in do, this function will be called to set the data to the last known valid state.
        /// </summary>
        /// <returns>true if rewind was successfully</returns>
        public virtual bool Catch()
        {
            throw new NotImplementedException();
        }
    }
}

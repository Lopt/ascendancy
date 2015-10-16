namespace Core.Controllers.AStar
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Core.Controllers.Actions;
    using Core.Models;

    /// <summary>
    /// Defines the parameters which will be used to find a path across a section of the map
    /// </summary>
    public class SearchParameters
    {
        /// <summary>
        /// Set the start location for the Astar algorithm.
        /// </summary>
        public PositionI StartLocation;

        /// <summary>
        /// Set the destination location for the Astar algorithm. 
        /// </summary>
        public PositionI EndLocation;

        /// <summary>
        /// Set the account id of the walking unit
        /// </summary>
        public int AccountID { get; set; }

        /// <summary>
        /// Set start and enp point for the Astar algorithm.
        /// </summary>
        /// <param name="startLocation"></param>
        /// <param name="endLocation"></param>
        public SearchParameters(PositionI startLocation, PositionI endLocation, int accountID)
        {
            this.StartLocation = startLocation;
            this.EndLocation = endLocation;
            this.AccountID = accountID;
        }
    }
}

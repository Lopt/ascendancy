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
        /// Set the start location for the AStar algorithm.
        /// </summary>
        public PositionI StartLocation;

        /// <summary>
        /// Set the destination location for the AStar algorithm. 
        /// </summary>
        public PositionI EndLocation;

        /// <summary>
        /// Gets or sets the account ID.
        /// </summary>
         /// <value>The account ID.</value>
        public int AccountID { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Core.Controllers.AStar.SearchParameters"/> class.
        /// Set start and end point for the AStar algorithm.
        /// </summary>
        /// <param name="startLocation">PositionI from the start for the algorithm.</param>
        /// <param name="endLocation">PositionI from the end for the algorithm.</param>
        /// <param name="accountID">Account ID.</param>
        public SearchParameters(PositionI startLocation, PositionI endLocation, int accountID)
        {
            this.StartLocation = startLocation;
            this.EndLocation = endLocation;
            this.AccountID = accountID;
        }
    }
}

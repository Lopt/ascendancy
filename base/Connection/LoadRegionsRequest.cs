namespace Core.Connections
{   
    using System;
    using Newtonsoft.Json;

    /// <summary>
    /// Request class which should be used to send actions to the server.
    /// Should be serialized before sending, will be deserialized after receiving.
    /// </summary>
    public class LoadRegionsRequest : Request
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Core.Connections.LoadRegionsRequest"/> class.
        /// </summary>
        /// <param name="sessionID">Session ID of the user.</param>
        /// <param name="position">Position where the user is standing (converted GPS information).</param>
        /// <param name="regionPositions">Region positions.</param>
        public LoadRegionsRequest(Guid sessionID, Core.Models.Position position, Core.Models.RegionPosition[] regionPositions)
            : base(sessionID, position)
        {
            RegionPositions = regionPositions;
        }

        /// <summary>
        /// Gets or sets the region positions.
        /// </summary>
        /// <value>The region positions.</value>
        public Core.Models.RegionPosition[] RegionPositions;
    }
}
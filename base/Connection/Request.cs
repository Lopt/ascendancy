namespace Core.Connections
{
    using System;

    /// <summary>
    /// Request class which should be used to send actions to the server.
    /// Should be serialized before sending, will be deserialized after receiving.
    /// </summary>
    public class Request
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Core.Connections.Request"/> class.
        /// </summary>
        /// <param name="sessionID">Session ID of the user.</param>
        /// <param name="position">Position where the user is standing (converted GPS information).</param>
        public Request(Guid sessionID, Core.Models.Position position)
        {
            Position = position;
            SessionID = sessionID;
        }

        /// <summary>
        /// The position of the user (converted current GPS information).
        /// </summary>
        public Core.Models.Position Position;

        /// <summary>
        /// The session of the user.
        /// </summary>
        public Guid SessionID;
    }
}
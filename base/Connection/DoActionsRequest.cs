namespace Core.Connections
{
    using System;

    /// <summary>
    /// Request class which should be used to send actions to the server.
    /// Should be serialized before sending, will be deserialized after receiving.
    /// </summary>
    public class DoActionsRequest : Request
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Core.Connections.DoActionsRequest"/> class.
        /// </summary>
        /// <param name="sessionID">Session ID of the user.</param>
        /// <param name="position">Position where the user is standing (converted GPS information).</param>
        /// <param name="actions">Actions which should be executed.</param>
        public DoActionsRequest(Guid sessionID, Core.Models.Position position, Core.Models.Action[] actions)
            : base(sessionID, position)
        {
            Actions = actions;
        }

        /// <summary>
        /// Gets or sets the actions.
        /// </summary>
        /// <value>The actions.</value>
        public Core.Models.Action[] Actions;
    }
}
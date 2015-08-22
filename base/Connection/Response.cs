namespace Core.Connections
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Standard response class which is used at every response (except login).
    /// Will be serialized before sending, should be deserialized after receiving.
    /// </summary>
    public class Response
    {
        /// <summary>
        /// Response status, (if and) which error occurred while the server tried to execute the request.
        /// </summary>
        public enum ReponseStatus
        {
            OK,
            ERROR,
            INTERNAL_ERROR,
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Core.Connections.Response"/> class.
        /// </summary>
        public Response()
        {
            Status = ReponseStatus.INTERNAL_ERROR;
            Entities = new LinkedList<LinkedList<Core.Models.Entity>>();
            Actions = new LinkedList<LinkedList<Core.Models.Action>>();
        }

        /// <summary>
        /// The status, only use other data if Status == OK.
        /// </summary>
        public ReponseStatus Status;

        /// <summary>
        /// The actions which the server executed at the requested regions.
        /// </summary>
        public LinkedList<LinkedList<Core.Models.Action>> Actions;

        /// <summary>
        /// The entities which are at the requested regions.
        /// </summary>
        public LinkedList<LinkedList<Core.Models.Entity>> Entities;
    }
}

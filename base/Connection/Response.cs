using System;
using System.Collections.Generic;

namespace Core.Connections
{
    /// <summary>
    /// Response class which is used at every response (except login response)
    /// Will be serialised before sending, should be deserialised after recieving.
    /// </summary>

    public class Response
    {
        public enum ReponseStatus
        {
            OK,
            ERROR,
            INTERNAL_ERROR,
        }

        public Response()
        {
            Status = ReponseStatus.INTERNAL_ERROR;
            Entities = new LinkedList<LinkedList<Core.Models.Entity>>();
            Actions = new LinkedList<LinkedList<Core.Models.Action>>();
        }

        public ReponseStatus Status;
        public LinkedList<LinkedList<Core.Models.Action>> Actions;
        public LinkedList<LinkedList<Core.Models.Entity>> Entities;

    }
}


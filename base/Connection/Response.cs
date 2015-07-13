using System;
using System.Collections.Generic;

namespace @base.connection
{
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


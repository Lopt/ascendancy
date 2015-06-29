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
            Entities = new LinkedList<LinkedList<model.Entity>>();
            Actions = new LinkedList<LinkedList<model.Action>>();
        }

        public ReponseStatus Status;
        public LinkedList<LinkedList<model.Action>> Actions;
        public LinkedList<LinkedList<model.Entity>> Entities;

    }
}


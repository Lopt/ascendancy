using System;
using System.Collections.ObjectModel;

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
            Entities = new ObservableCollection<ObservableCollection<model.Entity>>();
            Actions = new ObservableCollection<ObservableCollection<model.Action>>();
        }

        public ReponseStatus Status;
        public ObservableCollection<ObservableCollection<model.Action>> Actions;
        public ObservableCollection<ObservableCollection<model.Entity>> Entities;

    }
}


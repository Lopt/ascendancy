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
            m_status = ReponseStatus.INTERNAL_ERROR;
            m_entities = new ObservableCollection<ObservableCollection<model.Entity>>();
            m_actions = new ObservableCollection<ObservableCollection<model.Action>>();
        }

        public ReponseStatus Status
        {
            get { return m_status; }
            set { m_status = value; }
        }

        public ObservableCollection<ObservableCollection<model.Action>> Actions
        {
            get { return m_actions; }
            set { m_actions = value; }
        }

        public ObservableCollection<ObservableCollection<model.Entity>> Entities
        {
            get { return m_entities; }
            set { m_entities = value; }
        }

        ObservableCollection<ObservableCollection<model.Action>> m_actions;
        ObservableCollection<ObservableCollection<model.Entity>> m_entities;
        ReponseStatus m_status;

    }
}


using System;
using System.Collections.Concurrent;

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

        public Response(ReponseStatus status,
            ConcurrentDictionary<model.RegionPosition, model.Region.DatedActions> actions,
            ConcurrentDictionary<model.RegionPosition, model.Region.DatedEntities> entities)
        {
            m_status = status;
            m_entities = entities;
            m_actions = actions;
        }

        public ReponseStatus Status
        {
            get { return m_status; }
            set { m_status = value; }
        }

        public ConcurrentDictionary<model.RegionPosition, model.Region.DatedActions> Actions
        {
            get { return m_actions; }
            set { m_actions = value; }
        }

        public ConcurrentDictionary<model.RegionPosition, model.Region.DatedEntities> Entities
        {
            get { return m_entities; }
            set { m_entities = value; }
        }

        ConcurrentDictionary<model.RegionPosition, model.Region.DatedActions> m_actions;
        ConcurrentDictionary<model.RegionPosition, model.Region.DatedEntities> m_entities;
        ReponseStatus m_status;

    }
}


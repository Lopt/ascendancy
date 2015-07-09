using System;

namespace @base.connection
{
    public class DoActionsRequest : Request
    {
        public DoActionsRequest(Guid sessionID, model.Position position, model.Action[] actions)
            : base(sessionID, position)
        {
            m_actions = actions;
        }

        public model.Action[] Actions
        {
            get
            {
                return m_actions;
            }
            set
            {
                m_actions = value;
            }
        }

        model.Action[] m_actions;
    }
}


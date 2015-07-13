using System;

namespace @base.connection
{
    public class DoActionsRequest : Request
    {
        public DoActionsRequest(Guid sessionID, Core.Models.Position position, Core.Models.Action[] actions)
            : base(sessionID, position)
        {
            m_actions = actions;
        }

        public Core.Models.Action[] Actions
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

        Core.Models.Action[] m_actions;
    }
}


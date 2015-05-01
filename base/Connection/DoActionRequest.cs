using System;

namespace @base.connection
{
    public class DoActionRequest : Request
    {
        public DoActionRequest(Guid sessionID, model.Position position, control.action.Action[] actions)
            : base(sessionID, position)
        {
            m_actions = actions;
        }
            
        public control.action.Action[] Actions
        {
            get { return m_actions; }
            set { m_actions = value; }
        }

        control.action.Action[] m_actions;
    }
}


using System;

namespace @base.control.action
{
    public class Action
    {
        public enum ActionType 
        {
            Create,
            Move,
            Attack,
        }

        public Action(ActionType actionType, @base.model.Region[] regions, string parameters)
        {
            m_parameters = parameters;
            m_regions = regions;
            m_actionType = actionType;
            m_actionTime = DateTime.Now;
        }

        public bool Possible()
        {
            throw new NotImplementedException();
        }

        public bool Do()
        {
            throw new NotImplementedException();
        }

        public bool Catch()
        {
            throw new NotImplementedException();
        }

        public String Parameters
        {
            get { return m_parameters; }
        }

        public @base.model.Region[] Regions
        {
            get { return m_regions; }
        }

        public ActionType Type
        {
            get { return m_actionType; }
        }

        public DateTime ActionTime
        {
            get { return m_actionTime; }
        }

        private String m_parameters;
        private @base.model.Region[] m_regions;
        private ActionType m_actionType;
        private DateTime m_actionTime; 


    }
}


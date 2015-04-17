using System;

namespace @base.control
{
    public class Action
    {
        public enum ActionType 
        {
            Create,
            Move,
            Attack,
        }

        public Action(ActionType actionType, @base.model.Region regions, string parameters)
        {
            m_parameters = parameters;
            m_regions = regions;
            m_actionType = actionType;
        }



        private String m_parameters;
        private @base.model.Region m_regions;
        private ActionType m_actionType;


    }
}


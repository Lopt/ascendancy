using System;

namespace @base.model
{
    public class ModelEntity
    {
        public ModelEntity()
        {
        }

        public view.ViewEntity View
        {
            get { return m_viewEntity; }
            set { m_viewEntity = value; }
        }

        public control.ControlEntity Control
        {
            get { return m_controlEntity; }
            set { m_controlEntity = value; }
        }

        view.ViewEntity m_viewEntity;
        control.ControlEntity m_controlEntity;
    }
}


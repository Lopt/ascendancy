using System;

namespace @base.control
{
    public class ControlEntity
    {
        public ControlEntity (model.ModelEntity model)
        {   
            m_model = model;
            if (m_model.Control != null)
            {
                throw new Exception("ModelEntity.Control already has an ControlEntity.");
            }
            m_model.Control = this;
        }

        ~ControlEntity()
        {
            if (m_model.Control == this)
            {
                m_model.Control = null;
            }
        }

        model.ModelEntity Model
        {
            get { return m_model; }
        }

        private model.ModelEntity m_model;
    }
}


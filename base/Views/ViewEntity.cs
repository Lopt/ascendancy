using System;

namespace @base.view
{
	public class ViewEntity
	{
        public ViewEntity (model.ModelEntity model)
		{   
            m_model = model;
            if (m_model.View == null)
            {
                throw new Exception("ModelEntity.Control already has an ControlEntity.");
            }
            m_model.View = this;
		}

        ~ViewEntity()
        {
            if (m_model.View == this)
            {
                m_model.View = null;
            }
        }

        public model.ModelEntity Model
        {
            get { return m_model; }
        }

        private model.ModelEntity m_model;
	}
}


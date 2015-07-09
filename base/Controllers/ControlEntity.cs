using System;

namespace @base.control
{
    public class ControlEntity
    {
        public ControlEntity(model.ModelEntity model)
        {   
            Model = model;
            if (Model.Control != null)
            {
                throw new Exception("ModelEntity.Control already has an ControlEntity.");
            }
            Model.Control = this;
        }

        ~ControlEntity()
        {
            if (Model.Control == this)
            {
                Model.Control = null;
            }
        }

        public model.ModelEntity Model
        {
            get;
            private set;
        }
    }
}


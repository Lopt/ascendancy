using System;

namespace Core.Controllers
{
    public class ControlEntity
    {
        public ControlEntity(Core.Models.ModelEntity model)
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

        public Core.Models.ModelEntity Model
        {
            get;
            private set;
        }
    }
}


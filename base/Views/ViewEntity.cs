using System;

namespace @base.view
{
    public class ViewEntity
    {
        public ViewEntity(model.ModelEntity model)
        {   
            Model = model;
            if (Model.View != null)
            {
                throw new Exception("ModelEntity.Control already has an ViewEntity.");
            }
            Model.View = this;
        }

        ~ViewEntity()
        {
            if (Model.View == this)
            {
                Model.View = null;
            }
        }

        public model.ModelEntity Model
        {
            get;
            private set;
        }

    }
}


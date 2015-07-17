using System;

namespace Core.Views
{
    /// <summary>
    /// MVC View.
    /// </summary>
    public class ViewEntity
    {
        public ViewEntity(Core.Models.ModelEntity model)
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

        public Core.Models.ModelEntity Model
        {
            get;
            private set;
        }

    }
}


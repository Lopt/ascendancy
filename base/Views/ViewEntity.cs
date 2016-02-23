namespace Core.Views
{
    using System;

    /// <summary>
    /// MVC View.
    /// </summary>
    public class ViewEntity
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Core.Views.ViewEntity"/> class.
        /// </summary>
        /// <param name="model">The model.</param>
        public ViewEntity(Core.Models.ModelEntity model)
        {   
            Model = model;
            if (Model.View != null)
            {
                throw new Exception("ModelEntity.Control already has an ViewEntity.");
            }
            Model.View = this;
        }

        /// <summary>
        /// Gets the model.
        /// </summary>
        /// <value>The model.</value>
        public Core.Models.ModelEntity Model
        {
            get;
            private set;
        }
    }
}

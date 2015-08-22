namespace Core.Controllers
{
    using System;

    /// <summary>
    /// MVC Controller.
    /// </summary>
    public class ControlEntity
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Core.Controllers.ControlEntity"/> class.
        /// </summary>
        /// <param name="model">entity model.</param>
        public ControlEntity(Core.Models.ModelEntity model)
        {   
            Model = model;
            if (Model.Control != null)
            {
                throw new Exception("ModelEntity.Control already has an ControlEntity.");
            }
            Model.Control = this;
        }

        /// <summary>
        /// Finalizes an instance of the <see cref="Core.Controllers.ControlEntity"/> class.
        /// </summary>
        ~ControlEntity()
        {
            if (Model.Control == this)
            {
                Model.Control = null;
            }
        }

        /// <summary>
        /// Gets the model of the entity
        /// </summary>
        /// <value>The model.</value>
        public Core.Models.ModelEntity Model
        {
            get;
            private set;
        }
    }
}
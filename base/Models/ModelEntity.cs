namespace Core.Models
{
    using System;
    using Newtonsoft.Json;

    /// <summary>
    /// MVC Model.
    /// </summary>
    public class ModelEntity
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Core.Models.ModelEntity"/> class.
        /// </summary>
        public ModelEntity()
        {
        }

        /// <summary>
        /// The view part of MVC.
        /// </summary>
        [JsonIgnore]
        public Views.ViewEntity View;

        /// <summary>
        /// The control part of MVC.
        /// </summary>
        [JsonIgnore]
        public Controllers.ControlEntity Control;
    }
}
namespace Core.Controllers
{
    using System;

    /// <summary>
    /// Singleton which contains all core controllers.
    /// </summary>
    public sealed class Controller
    {
        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <value>The instance.</value>
        public static Controller Instance
        {
            get
            {
                return Singleton.Value;
            }
        }

        /// <summary>
        /// Prevents a default instance of the <see cref="Core.Controllers.Controller"/> class from being created.
        /// </summary>
        private Controller()
        {
        }

        /// <summary>
        /// The region manager controller.
        /// </summary>
        public RegionManagerController RegionManagerController;

        /// <summary>
        /// The definition manager controller.
        /// </summary>
        public DefinitionManagerController DefinitionManagerController;

        /// <summary>
        /// The singleton instance.
        /// </summary>
        private static readonly Lazy<Controller> Singleton =
            new Lazy<Controller>(() => new Controller());        
    }
}
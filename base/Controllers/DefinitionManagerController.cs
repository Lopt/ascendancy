namespace Core.Controllers
{
    using Core.Models;

    /// <summary>
    /// Loads and adds definitions to the definition manager
    /// </summary>
    public class DefinitionManagerController
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Core.Controllers.DefinitionManagerController"/> class.
        /// </summary>
        public DefinitionManagerController()
        {
            m_definitionManager = World.Instance.DefinitionManager;
        }

        /// <summary>
        /// Gets the definition manager.
        /// </summary>
        /// <value>The definition manager.</value>
        public DefinitionManager DefinitionManager
        {
            get
            {
                return m_definitionManager;
            }
        }

        /// <summary>
        /// The definition manager.
        /// </summary>
        private DefinitionManager m_definitionManager;
    }
}
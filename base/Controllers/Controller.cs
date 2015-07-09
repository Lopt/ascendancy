using System;

namespace @base.control
{
    public sealed class Controller
    {
        private static readonly Lazy<Controller> m_singleton =
            new Lazy<Controller>(() => new Controller());

        public static Controller Instance
        {
            get
            {
                return m_singleton.Value;
            }
        }

        private Controller()
        {
        }

        public RegionManagerController RegionManagerController;
        public DefinitionManagerController DefinitionManagerController;
    }
}


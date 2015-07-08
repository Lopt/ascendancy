using System;

namespace @base.control
{
    public sealed class Controller
	{
        private static readonly Lazy<Controller> lazy =
            new Lazy<Controller>(() => new Controller());

        public static Controller Instance { get { return lazy.Value; } }

		private Controller()
		{
		}

        public RegionManagerController RegionManagerController;
        public DefinitionManagerController DefinitionManagerController;
	}
}


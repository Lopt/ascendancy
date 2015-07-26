using System;
using CocosSharp;
using System.Threading.Tasks;

namespace Client.Common.Views
{
    /// <summary>
    /// The Start scene.
    /// </summary>
    public class StartScene : CCScene
    {
        /// <summary>
        /// The loading phases.
        /// </summary>
        public enum Phases
        {
            Start,
            LoggedIn,
            TerrainTypeLoaded,
            EntityTypeLoaded,
            RegionLoaded,
            EntitiesLoaded,
            Done,
            Failure,
        }

        /// <summary>
        /// Gets the loading phase.
        /// </summary>
        /// <value>The phase.</value>
        public Phases Phase
        {
            get;
            private set;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Client.Common.Views.StartScene"/> class.
        /// </summary>
        /// <param name="mainWindow">Main window.</param>
        public StartScene(CCWindow mainWindow)
            : base(mainWindow)
        {
            Phase = Phases.Start;

            m_LogoLayer = new LogoLayer(this);
            this.AddChild(m_LogoLayer);

            InitWorld();
        }

        /// <summary>
        /// Initializing the network controller, geolocation and the world with their region and definition controllers.
        /// </summary>
        void InitWorld()
        {
            var initNet = Controllers.NetworkController.Instance;
            var initGeo = Models.Geolocation.Instance;

            var world = Core.Models.World.Instance;
            var controller = Core.Controllers.Controller.Instance;
            controller.RegionManagerController = new Client.Common.Manager.RegionManagerController();      
            controller.DefinitionManagerController = new Client.Common.Manager.DefinitionManagerController();
        }

        /// <summary>
        /// Inits the loadings async. Login to the server and set the account and load the definitions and regions at the current geolocation.
        /// </summary>
        /// <returns>The account async.</returns>
        public async Task<Core.Models.Account> InitLoadingAsync()
        {
            var account = await LoginAsync(); 
            if (account != null)
            {
                Phase = Phases.LoggedIn;

                var controller = Core.Controllers.Controller.Instance;
                // adds his own account to AccountManager, so it is known
                Core.Models.World.Instance.AccountManager.AddAccount(account);

                var entityManagerController = controller.DefinitionManagerController as Client.Common.Manager.DefinitionManagerController;
                await entityManagerController.LoadTerrainDefinitionsAsync();
                Phase = Phases.TerrainTypeLoaded;

                await entityManagerController.LoadUnitDefinitionsAsync();
                Phase = Phases.EntitiesLoaded;

                var regionManagerController = controller.RegionManagerController as Client.Common.Manager.RegionManagerController;
                await regionManagerController.LoadRegionsAsync();
                Phase = Phases.RegionLoaded;
                // do something in the future
                Phase = Phases.Done;

            }
            else
            {
                Phase = Phases.Failure;
                throw new NotImplementedException("Login failure");
            }
            return account;
        }

        /// <summary>
        /// Login async to the server, with the device id and name and a password.
        /// </summary>
        /// <returns>The account.</returns>
        async Task<Core.Models.Account> LoginAsync()
        {
            var currentGamePosition = Client.Common.Models.Geolocation.Instance.CurrentGamePosition;
            var device = Client.Common.Models.Device.GetInstance;
            var user = device.DeviceId;
            user += device.DeviceName;

            return await Client.Common.Controllers.NetworkController.Instance.LoginAsync(currentGamePosition, user, "Password");
        }


        #region Properties

        /// <summary>
        /// The m_logo layer.
        /// </summary>
        LogoLayer m_LogoLayer;

        #endregion
    }
}


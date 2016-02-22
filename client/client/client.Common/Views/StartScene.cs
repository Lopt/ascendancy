namespace Client.Common.Views
{
    using System;
    using System.Threading.Tasks;
    using Client.Common.Manager;
    using Client.Common.Models;
    using CocosSharp;

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
            PositionAquired,
            LoggedIn,
            TerrainTypeLoaded,
            EntityTypeLoaded,
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

            m_logoLayer = new LogoLayer(this);
            this.AddChild(m_logoLayer);

            InitWorld();
        }

        /// <summary>
        /// Loads everything async.
        /// - User Login to the Server (and sets the Account)
        /// - Loads the definitions (unit and terrain)
        /// - Loads regions at the current Geo Location.
        /// </summary>
        /// <returns>The account async.</returns>
        public async Task<Core.Models.Account> InitLoadingAsync()
        {
            // wait until there is an known position
            await Geolocation.Instance.GetPositionAsync();
            Phase = Phases.PositionAquired;

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
                Phase = Phases.EntityTypeLoaded;
       
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
        private async Task<Core.Models.Account> LoginAsync()
        {
            var currentGamePosition = Client.Common.Models.Geolocation.Instance.CurrentGamePosition;
            var device = Client.Common.Models.Device.Instance;
            var user = device.DeviceId;
            user += device.DeviceName;

            return await Client.Common.Controllers.NetworkController.Instance.LoginAsync(currentGamePosition, user, "Password");
        }

        /// <summary>
        /// Initializing the network controller, Geo Location and the world with their region and definition controllers.
        /// </summary>
        private void InitWorld()
        {
            var initNet = Controllers.NetworkController.Instance;
            var initGeo = Models.Geolocation.Instance;

            var world = Core.Models.World.Instance;
            var controller = Core.Controllers.Controller.Instance;
            controller.RegionManagerController = new Client.Common.Manager.RegionManagerController();      
            controller.DefinitionManagerController = new Client.Common.Manager.DefinitionManagerController();
        }

        #region Properties

        /// <summary>
        /// The m_logo layer.
        /// </summary>
        private LogoLayer m_logoLayer;

        #endregion
    }
}
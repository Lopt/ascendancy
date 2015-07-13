using System;
using CocosSharp;
using System.Threading.Tasks;

namespace client.Common.Views
{
    public class StartScene : CCScene
    {
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

        public Phases Phase
        {
            get;
            private set;
        }

        public StartScene(CCWindow mainWindow)
            : base(mainWindow)
        {
            Phase = Phases.Start;

            m_LogoLayer = new LogoLayer(this);
            this.AddChild(m_LogoLayer);

            InitWorld();
        }

		void InitWorld()
		{
			var initNet = Controllers.NetworkController.Instance;
			var initGeo = Models.Geolocation.Instance;

			var world = Core.Models.World.Instance;
			var controller = Core.Controllers.Controller.Instance;
			controller.RegionManagerController = new client.Common.Manager.RegionManagerController();      
			controller.DefinitionManagerController = new client.Common.Manager.DefinitionManagerController();
		}


		public async Task<Core.Models.Account> InitLoadingAsync()
        {
            var account = await LogInAsync(); 
            if (account != null)
            {
                Phase = Phases.LoggedIn;

                var controller = Core.Controllers.Controller.Instance;
                // adds his own account to AccountManager, so it is known
                Core.Models.World.Instance.AccountManager.AddAccount(account);

                var entityManagerController = controller.DefinitionManagerController as client.Common.Manager.DefinitionManagerController;
                await entityManagerController.LoadTerrainDefinitionsAsync();
                Phase = Phases.TerrainTypeLoaded;

                await entityManagerController.LoadEntityDefinitionsAsync();
                Phase = Phases.EntitiesLoaded;

                var regionManagerController = controller.RegionManagerController as client.Common.Manager.RegionManagerController;
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



        async Task<Core.Models.Account> LogInAsync()
        {
            var currentGamePosition = client.Common.Models.Geolocation.Instance.CurrentGamePosition;
            var device = client.Common.Models.Device.GetInstance;
            var user = device.DeviceId;
            user += device.DeviceName;

            return await client.Common.Controllers.NetworkController.Instance.LoginAsync(currentGamePosition, user, "Password");
        }


        #region Properties

        LogoLayer m_LogoLayer;

        #endregion
    }
}


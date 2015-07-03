using System;
using CocosSharp;
using Xamarin.Forms;
using CocosDenshion;
using client.Common.Helper;
using client.Common.Controllers;
using @base.model;
using client.Common.Views;
using System.Threading.Tasks;
using client.Common.Models;
using client.Common.Manager;
using Xamarin.Forms.Xaml;





namespace client.Common
{
    public class GameAppDelegate : CCApplicationDelegate
    {
        public enum Loading
        {
            Started,
            Login,
            Loggedin,
            TerrainTypeLoading,
            TerrainTypeLoaded,
            EntityTypeLoading,
            EntityTypeLoaded,
            RegionLoading,
            RegionLoaded,
            EntitiesLoading,
            EntitiesLoaded,
            Done,
        }

        public static Loading LoadingState = Loading.Started;

        public static Account Account {
            get;
            private set;
        }


        public override void ApplicationDidFinishLaunching (CCApplication application, CCWindow mainWindow)
        {
            application.PreferMultiSampling = false;

            SetContentPaths (application);

            CCSize windowSize = mainWindow.WindowSizeInPixels;

            float desiredWidth = 1024.0f;
            float desiredHeight = 768.0f;

            // erstellen der Welt und anlegen bzw. verknüpfen mit den Controllern
            InitWorld ();

            // This will set the world bounds to be (0,0, w, h)
            // CCSceneResolutionPolicy.ShowAll will ensure that the aspect ratio is preserved
            CCScene.SetDefaultDesignResolution (windowSize.Width, windowSize.Height, CCSceneResolutionPolicy.ShowAll);
            
            // Determine whether to use the high or low def versions of our images
            // Make sure the default texel to content size ratio is set correctly
            // Of course you're free to have a finer set of image resolutions e.g (ld, hd, super-hd)
            if (desiredWidth < windowSize.Width) {
                application.ContentSearchPaths.Add (ClientConstants.IMAGES_HD);
                CCSprite.DefaultTexelToContentSizeRatio = 2.0f;
            } else {
                application.ContentSearchPaths.Add (ClientConstants.IMAGES_LD);
                CCSprite.DefaultTexelToContentSizeRatio = 1.0f;
            }
           
            InitLoading ();

            StartScene startScene = new StartScene (mainWindow);
            mainWindow.RunWithScene (startScene);
         
           
        }

        public override void ApplicationDidEnterBackground (CCApplication application)
        {
            Geolocation.GetInstance.StopListening ();
            application.Paused = true;
        }

        public override void ApplicationWillEnterForeground (CCApplication application)
        {
            Geolocation.GetInstance.StartListening (1000, 4);
            application.Paused = false;
        }

        private async Task InitLoading ()
        {
            LoadingState = Loading.Login;
            await LogInAsync ();

            if (NetworkController.GetInstance.IsLogedin) {

                var controller = @base.control.Controller.Instance;

                LoadingState = Loading.Loggedin;
                LoadingState = Loading.TerrainTypeLoading;
                var entityManagerController = controller.DefinitionManagerController as client.Common.Manager.DefinitionManagerController;
                await entityManagerController.LoadTerrainDefinitionsAsync ();
                LoadingState = Loading.TerrainTypeLoaded;

                LoadingState = Loading.EntitiesLoading;
                await entityManagerController.LoadEntityDefinitionsAsync ();
                LoadingState = Loading.EntitiesLoaded;

                LoadingState = Loading.RegionLoading;
                var regionManagerController = controller.RegionManagerController as client.Common.Manager.RegionManagerController;
                await regionManagerController.LoadRegionsAsync ();
                LoadingState = Loading.RegionLoaded;
                // do something in the future
                LoadingState = Loading.Done;

            } else {
                throw new NotImplementedException ("Login failure");
            }

        }

        private void InitWorld ()
        {
            var world = World.Instance;
            var controller = @base.control.Controller.Instance;
            controller.RegionManagerController = new client.Common.Manager.RegionManagerController ();      
            controller.DefinitionManagerController = new DefinitionManagerController ();
        }

        private async Task LogInAsync ()
        {
            var currentGamePosition = Geolocation.GetInstance.CurrentGamePosition;
            var device = client.Common.Models.Device.GetInstance;
            var user = device.DeviceId;
            user += device.DeviceName;
            LoadingState = Loading.Login;
            var id = await NetworkController.GetInstance.LoginAsync (currentGamePosition, user, "Password");
            if (NetworkController.GetInstance.IsLogedin) {
                Account = new Account (id, user);
                LoadingState = Loading.Loggedin;
            }
                
        }

        private void SetContentPaths (CCApplication application)
        {
            application.ContentRootDirectory = ClientConstants.CONTENT;
            application.ContentSearchPaths.Add (ClientConstants.ANIMATIONS);
            application.ContentSearchPaths.Add (ClientConstants.FONTS);
            application.ContentSearchPaths.Add (ClientConstants.SOUNDS);
            application.ContentSearchPaths.Add (ClientConstants.TILES);
            application.ContentSearchPaths.Add (ClientConstants.IMAGES);
        }
    }
}

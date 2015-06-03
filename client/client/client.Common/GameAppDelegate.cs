using System;
using CocosSharp;
using Xamarin.Forms;
using CocosDenshion;
using client.Common.Helper;
using client.Common.Controllers;
using @base.model;
using @base.control;
using client.Common.Views;
using System.Threading.Tasks;
using client.Common.Models;




namespace client.Common
{
    public class GameAppDelegate : CCApplicationDelegate
    {
        public enum Loading
        {
            Started,
            TerrainTypeLoading,
            TerrainTypeLoaded,
            RegionLoading,
            RegionLoaded,
            EntitiesLoading,
            EntitiesLoaded,
            Done,
        }

        public static Loading LoadingState = Loading.Started;

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

            StartScene startScene = new StartScene (mainWindow);
            mainWindow.RunWithScene (startScene);

            InitLoading ();
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
            LoadingState = Loading.TerrainTypeLoading;
            var terrainController = Controller.Instance.TerrainManagerController as TerrainController;
            await terrainController.LoadTerrainDefinitionsAsync ();
            LoadingState = Loading.TerrainTypeLoaded;

            LoadingState = Loading.RegionLoading;
            var regionController = Controller.Instance.RegionManagerController as RegionController;
            await regionController.LoadRegionsAsync ();
            LoadingState = Loading.RegionLoaded;
            // do something in the future
            LoadingState = Loading.Done;
        }

        private void InitWorld ()
        {
            var world = World.Instance;
            var controller = Controller.Instance;
            controller.TerrainManagerController = new TerrainController ();
            controller.RegionManagerController = new RegionController ();
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
=======
	public class GameAppDelegate : CCApplicationDelegate
	{
		public enum Loading
		{
			Started,
			TerrainTypeLoading,
			TerrainTypeLoaded,
			RegionLoading,
			RegionLoaded,
			EntitiesLoading,
			EntitiesLoaded,
			Done,
		}

		public static Loading m_Loading = Loading.Started;

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

			StartScene startScene = new StartScene (mainWindow);
			mainWindow.RunWithScene (startScene);

			InitLoading ();
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
			m_Loading = Loading.TerrainTypeLoading;
			var terrainController = Controller.Instance.TerrainManagerController as TerrainController;
			await terrainController.LoadTerrainDefinitionsAsync ();
			m_Loading = Loading.TerrainTypeLoaded;

			m_Loading = Loading.RegionLoading;
			var regionController = Controller.Instance.RegionManagerController as RegionController;
			await regionController.LoadRegionsAsync ();
			m_Loading = Loading.RegionLoaded;
			m_Loading = Loading.Done;
		}

		private void InitWorld ()
		{
			var world = World.Instance;
			var controller = Controller.Instance;
			controller.TerrainManagerController = new TerrainController ();
			controller.RegionManagerController = new RegionController ();
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
>>>>>>> d741c7fafad5bbf531fdb127801a8575f7c0add4
}

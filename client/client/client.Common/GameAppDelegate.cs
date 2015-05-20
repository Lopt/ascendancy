using System;
using CocosSharp;
using Xamarin.Forms;
using CocosDenshion;
using client.Common.helper;
using client.Common.Controllers;
using @base.model;
using @base.control;
using client.Common.controller;
using client.Common.view;




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

		public static Loading m_Loading = Loading.Started;

		public override void ApplicationDidFinishLaunching (CCApplication application, CCWindow mainWindow)
		{
			application.PreferMultiSampling = false;
			application.ContentRootDirectory = ClientConstants.CONTENT;
			application.ContentSearchPaths.Add (ClientConstants.ANIMATIONS);
			application.ContentSearchPaths.Add (ClientConstants.FONTS);
			application.ContentSearchPaths.Add (ClientConstants.SOUNDS);
			application.ContentSearchPaths.Add (ClientConstants.TILES);
			application.ContentSearchPaths.Add (ClientConstants.IMAGES);

			CCSize windowSize = mainWindow.WindowSizeInPixels;

			float desiredWidth = 1024.0f;
			float desiredHeight = 768.0f;

			// erstellen der Welt und anlegen bzw. verknüpfen mit den Controllern
			var world = World.Instance;
			var controller = Controller.Instance;
			controller.TerrainManagerController = new TerrainController ();
			controller.RegionManagerController = new RegionController ();

			var terrainController = Controller.Instance.TerrainManagerController as TerrainController;
			terrainController.LoadTerrainDefinitionsAsync ();

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

//            CCScene Gamescene = new CCScene(mainWindow);
//            GameLayer gameLayer = new GameLayer();
//            Gamescene.AddChild(gameLayer);
         
//            CCScene MyGeolocationScene = new GeolocationScene(mainWindow);
			//CCScene MyDeviceScene = new DeviceScene (mainWindow);
			//CCScene MyTouchTestScene = new TouchTestScene(mainWindow);
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
	}
}

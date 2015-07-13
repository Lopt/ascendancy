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


        static public Account Account
        {
            get;
            private set;
        }


        public enum Phases
        {
            Start,
            StartScene,
            GameScene,
            Exit,
        }

        public Phases Phase
        {
            get;
            private set;
        }

        public override void ApplicationDidFinishLaunching(CCApplication application, CCWindow mainWindow)
        {
            m_window = mainWindow;
            Phase = Phases.Start;

            application.PreferMultiSampling = false;

            SetContentPaths(application);

            CCSize windowSize = mainWindow.WindowSizeInPixels;

            float desiredWidth = 1024.0f;
            //float desiredHeight = 768.0f;


            // This will set the world bounds to be (0,0, w, h)
            // CCSceneResolutionPolicy.ShowAll will ensure that the aspect ratio is preserved
            CCScene.SetDefaultDesignResolution(windowSize.Width, windowSize.Height, CCSceneResolutionPolicy.ShowAll);
            
            // Determine whether to use the high or low def versions of our images
            // Make sure the default texel to content size ratio is set correctly
            // Of course you're free to have a finer set of image resolutions e.g (ld, hd, super-hd)
            if (desiredWidth < windowSize.Width)
            {
                application.ContentSearchPaths.Add(ClientConstants.IMAGES_HD);
                CCSprite.DefaultTexelToContentSizeRatio = 2.0f;
            }
            else
            {
                application.ContentSearchPaths.Add(ClientConstants.IMAGES_LD);
                CCSprite.DefaultTexelToContentSizeRatio = 1.0f;
            }
           
			SceneStart();//.RunSynchronously();
        }

		private async Task SceneStart()
		{
			
			m_currentScene = new StartScene(m_window);
			Phase = Phases.StartScene;
			m_window.RunWithScene(m_currentScene);


			Account = await ((StartScene) m_currentScene).InitLoadingAsync();

			m_currentScene = new GameScene(m_window);
            Phase = Phases.GameScene;
            m_window.DefaultDirector.ReplaceScene(m_currentScene);
        }


        public override void ApplicationDidEnterBackground(CCApplication application)
        {
            Geolocation.Instance.StopListening();
            application.Paused = true;
        }

        public override void ApplicationWillEnterForeground(CCApplication application)
        {
            application.Paused = false;
            Geolocation.Instance.StartListening(1000, 4);
        }

        private void SetContentPaths(CCApplication application)
        {
            application.ContentRootDirectory = ClientConstants.CONTENT;
            application.ContentSearchPaths.Add(ClientConstants.ANIMATIONS);
            application.ContentSearchPaths.Add(ClientConstants.FONTS);
            application.ContentSearchPaths.Add(ClientConstants.SOUNDS);
            application.ContentSearchPaths.Add(ClientConstants.TILES);
            application.ContentSearchPaths.Add(ClientConstants.IMAGES);
        }

        private CCWindow m_window;
		private CCScene m_currentScene;

    }
}

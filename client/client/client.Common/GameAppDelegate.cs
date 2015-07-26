using System;
using CocosSharp;
using Xamarin.Forms;
using CocosDenshion;
using Client.Common.Helper;
using Client.Common.Controllers;
using Core.Models;
using Client.Common.Views;
using System.Threading.Tasks;
using Client.Common.Models;
using Client.Common.Manager;
using Xamarin.Forms.Xaml;


namespace Client.Common
{
    /// <summary>
    /// The Game app delegate is the game entry point.
    /// </summary>
    public class GameAppDelegate : CCApplicationDelegate
    {

        /// <summary>
        /// Gets the account.
        /// </summary>
        /// <value>The account.</value>
        static public Account Account
        {
            get;
            private set;
        }

        /// <summary>
        /// Game phases.
        /// </summary>
        public enum Phases
        {
            Start,
            StartScene,
            GameScene,
            Exit,
        }

        /// <summary>
        /// Gets the game phase.
        /// </summary>
        /// <value>The phase.</value>
        public Phases Phase
        {
            get;
            private set;
        }

        /// <summary>
        /// When the Application did finish launching.
        /// </summary>
        /// <param name="application">Application.</param>
        /// <param name="mainWindow">Main window.</param>
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
           
            SceneStartAsync();//.RunSynchronously();
        }

        /// <summary>
        /// Set the start scene and if all is initialized the game scene.
        /// </summary>
        /// <returns>The task.</returns>
        private async Task SceneStartAsync()
        {
			
            m_currentScene = new StartScene(m_window);
            Phase = Phases.StartScene;
            m_window.RunWithScene(m_currentScene);

            Account = await ((StartScene)m_currentScene).InitLoadingAsync();

            m_currentScene = new GameScene(m_window);
            Phase = Phases.GameScene;
            m_window.DefaultDirector.ReplaceScene(m_currentScene);
        }

        /// <summary>
        /// When the Application did enter background. Stop listening the geolocation and pause the application. 
        /// </summary>
        /// <param name="application">Application.</param>
        public override void ApplicationDidEnterBackground(CCApplication application)
        {
            Geolocation.Instance.StopListening();
            application.Paused = true;
        }

        /// <summary>
        /// When the Application will enter foreground end the pause and start listening the geolocation.
        /// </summary>
        /// <param name="application">Application.</param>
        public override void ApplicationWillEnterForeground(CCApplication application)
        {
            application.Paused = false;
            Geolocation.Instance.StartListening(1000, 4);
        }

        /// <summary>
        /// Sets the content paths.
        /// </summary>
        /// <param name="application">Application.</param>
        private void SetContentPaths(CCApplication application)
        {
            application.ContentRootDirectory = ClientConstants.CONTENT;
            application.ContentSearchPaths.Add(ClientConstants.ANIMATIONS);
            application.ContentSearchPaths.Add(ClientConstants.FONTS);
            application.ContentSearchPaths.Add(ClientConstants.SOUNDS);
            application.ContentSearchPaths.Add(ClientConstants.TILES);
            application.ContentSearchPaths.Add(ClientConstants.IMAGES);
        }

        /// <summary>
        /// The m_window.
        /// </summary>
        private CCWindow m_window;
        /// <summary>
        /// The m_current scene.
        /// </summary>
        private CCScene m_currentScene;

    }
}

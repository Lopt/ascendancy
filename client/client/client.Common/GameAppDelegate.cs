namespace Client.Common
{
    using System;
    using System.Threading.Tasks;
    using Client.Common.Controllers;
    using Client.Common.Helper;
    using Client.Common.Manager;
    using Client.Common.Models;
    using Client.Common.Views;
    using CocosDenshion;
    using CocosSharp;
    using Core.Models;
    using Xamarin.Forms;
    using Xamarin.Forms.Xaml;

    /// <summary>
    /// The Game app delegate is the game entry point.
    /// </summary>
    public class GameAppDelegate : CCApplicationDelegate
    {
        /// <summary>
        /// Gets the account.
        /// </summary>
        /// <value>The account.</value>
        public static Account Account
        {
            get;
            private set;
        }

        /// <summary>
        /// To calculate resources and other stuff, the current time is needed. But not from the client, the server time is needed
        /// But it is bad to ask the server always "how late is it?" so it gives the server time at the login
        /// Calculates then what's the time difference the client and the server time
        /// And adds the difference to the client time, always when asked
        /// There is a small inaccurancy bescause the network access (login) needs time.
        /// But this can be neglected, bescause the server is ahead of the client
        /// (if you want to build something... the server has more resources then your client, bescause he is a few millisecondsa ahead)
        /// </summary>
        /// <value>The server time.</value>
        private static TimeSpan SeverTimeDifference;

        public static DateTime ServerTime
        {
            get
            {
                return DateTime.Now - SeverTimeDifference;
            }
            set
            {
                SeverTimeDifference = DateTime.Now - value;
            }
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
        /// <param name="application">Main Application.</param>
        /// <param name="mainWindow">Main window.</param>
        public override void ApplicationDidFinishLaunching(CCApplication application, CCWindow mainWindow)
        {
            m_window = mainWindow;
            Phase = Phases.Start;

            application.PreferMultiSampling = false;

            SetContentPaths(application);

            CCSize windowSize = mainWindow.WindowSizeInPixels;

            float desiredWidth = 1024.0f;
            // float desiredHeight = 768.0f;

            // This will set the world bounds to be (0,0, w, h)
            // CCSceneResolutionPolicy.ShowAll will ensure that the aspect ratio is preserved
            CCScene.SetDefaultDesignResolution(windowSize.Width, windowSize.Height, CCSceneResolutionPolicy.ShowAll);
            CCSprite.DefaultTexelToContentSizeRatio = 1.0f;
            // Determine whether to use the high or low def versions of our images
            // Make sure the default texel to content size ratio is set correctly
            // Of course you're free to have a finer set of image resolutions e.g (ld, hd, super-hd)
            if (desiredWidth < windowSize.Width)
            {
                application.ContentSearchPaths.Add(Constants.ClientConstants.IMAGES_HD);               
            }
            else
            {
                application.ContentSearchPaths.Add(Constants.ClientConstants.IMAGES_LD);
            }

             SceneStartAsync(); // .RunSynchronously();
        }

        /// <summary>
        /// When the Application enters background. Stops listening the GeoLocation and pause the application. 
        /// </summary>
        /// <param name="application">Main Application.</param>
        public override void ApplicationDidEnterBackground(CCApplication application)
        {
            Geolocation.Instance.StopListening();
            application.Paused = true;
        }

        /// <summary>
        /// When the Application enters foreground, then end the pause and start listening the GeoLocation.
        /// </summary>
        /// <param name="application">Main Application.</param>
        public override void ApplicationWillEnterForeground(CCApplication application)
        {
            application.Paused = false;
            Geolocation.Instance.StartListening(1000, 4);
        }

        /// <summary>
        /// Sets the content paths.
        /// </summary>
        /// <param name="application">Main Application.</param>
        private void SetContentPaths(CCApplication application)
        {
            application.ContentRootDirectory = Constants.ClientConstants.CONTENT;
            application.ContentSearchPaths.Add(Constants.ClientConstants.ANIMATIONS);
            application.ContentSearchPaths.Add(Constants.ClientConstants.FONTS);
            application.ContentSearchPaths.Add(Constants.ClientConstants.SOUNDS);
            application.ContentSearchPaths.Add(Constants.ClientConstants.TILES);
            application.ContentSearchPaths.Add(Constants.ClientConstants.IMAGES);
            application.ContentSearchPaths.Add(Constants.ClientConstants.UNITS);
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

            Account = await((StartScene)m_currentScene).InitLoadingAsync();

            m_currentScene = new GameScene(m_window);
            Phase = Phases.GameScene;
            m_window.DefaultDirector.ReplaceScene(m_currentScene);
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

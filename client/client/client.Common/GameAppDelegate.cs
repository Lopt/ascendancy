using System;
using CocosSharp;
using Xamarin.Forms;
using CocosDenshion;
using client.Common.helper;


namespace client.Common
{
    public class GameAppDelegate : CCApplicationDelegate
    {
        public override void ApplicationDidFinishLaunching(CCApplication application, CCWindow mainWindow)
        {
            application.PreferMultiSampling = false;
            application.ContentRootDirectory = ClientConstants.Content;
            application.ContentSearchPaths.Add(ClientConstants.animations);
            application.ContentSearchPaths.Add(ClientConstants.fonts);
            application.ContentSearchPaths.Add(ClientConstants.sounds);

            CCSize windowSize = mainWindow.WindowSizeInPixels;

            float desiredWidth = 1024.0f;
            float desiredHeight = 768.0f;
            
            // This will set the world bounds to be (0,0, w, h)
            // CCSceneResolutionPolicy.ShowAll will ensure that the aspect ratio is preserved
            CCScene.SetDefaultDesignResolution(windowSize.Width, windowSize.Height, CCSceneResolutionPolicy.ShowAll);
            
            // Determine whether to use the high or low def versions of our images
            // Make sure the default texel to content size ratio is set correctly
            // Of course you're free to have a finer set of image resolutions e.g (ld, hd, super-hd)
            if (desiredWidth < windowSize.Width)
            {
                application.ContentSearchPaths.Add(ClientConstants.images_hd);
                CCSprite.DefaultTexelToContentSizeRatio = 2.0f;
            }
            else
            {
                application.ContentSearchPaths.Add(ClientConstants.images_ld);
                CCSprite.DefaultTexelToContentSizeRatio = 1.0f;
            }


//            CCScene Gamescene = new CCScene(mainWindow);
//            GameLayer gameLayer = new GameLayer();
//            Gamescene.AddChild(gameLayer);
         
//            CCScene MyGeolocationScene = new GeolocationScene(mainWindow);
            CCScene MyDeviceScene = new DeviceScene(mainWindow);
            //CCScene MyTouchTestScene = new TouchTestScene(mainWindow);

            mainWindow.RunWithScene(MyDeviceScene);
        }

        public override void ApplicationDidEnterBackground(CCApplication application)
        {
            Geolocation.GetInstance.StopListening();
            application.Paused = true;
        }

        public override void ApplicationWillEnterForeground(CCApplication application)
        {
            Geolocation.GetInstance.StartListening(10000, 4);
            application.Paused = false;
        }
    }
}

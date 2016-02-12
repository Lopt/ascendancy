namespace Client.iOS
{
    using System;
    using Client.Common;
    using CocosSharp;
    using Foundation;
    using UIKit;
    using Xamarin.Forms;
    using XLabs.Forms;
    using XLabs.Ioc;
    using XLabs.Platform.Device;
    using XLabs.Platform.Services;
    using XLabs.Platform.Services.Geolocation;
    using XLabs.Platform.Services.IO;
    using XLabs.Platform.Services.Media;

    // The UIApplicationDelegate for the application. This class is responsible for launching the
    // User Interface of the application, as well as listening (and optionally responding) to
    // application events from iOS.

    /// <summary>
    /// App delegate.
    /// </summary>
    [Register("GameAppDelegate")]
    public partial class AppDelegate : UIApplicationDelegate
    {
        // This method is invoked when the application has loaded and is ready to run. In this
        // method you should instantiate the window, load the UI into it and then make the window
        // visible.
        // You have 17 seconds to return from this method, or iOS will terminate your application.

        /// <summary>
        /// When Finished the launching.
        /// </summary>
        /// <param name="app">The App.</param>
        public override void FinishedLaunching(UIApplication app)
        {
            if (!Resolver.IsSet)
            {
                this.SetIoc();
            }
            // aktivating Xamarin.Forms
            global::Xamarin.Forms.Forms.Init();

            // activating tcp connection for iOS
            TcpConnection.Connector = new TcpConnection();

            CCApplication application = new CCApplication();
            application.ApplicationDelegate = new GameAppDelegate();

            application.StartGame();
        }

        /// <summary>
        /// The entry point of the program, where the program control starts and ends.
        /// </summary>
        /// <param name="args">The command-line arguments.</param>
        public static void Main(string[] args)
        {
            UIApplication.Main(args, null, "GameAppDelegate");
        }

        /// <summary>
        /// Sets the io container.
        /// </summary>
        private void SetIoc()
        {
            var resolverContainer = new SimpleContainer();

            resolverContainer.Register<IDevice>(t => AppleDevice.CurrentDevice)
                .Register<IAccelerometer>(t => t.Resolve<IDevice>().Accelerometer)
                .Register<IDisplay>(t => t.Resolve<IDevice>().Display)
                .Register<INetwork>(t => t.Resolve<IDevice>().Network)
                .Register<IGeolocator,Geolocator>()
                .Register<IDependencyContainer>(resolverContainer);

            Resolver.SetResolver(resolverContainer.GetResolver());
        }
    }
}
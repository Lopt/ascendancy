using System;
using Foundation;
using UIKit;
using CocosSharp;
using XLabs.Forms;
using Xamarin.Forms;
using XLabs.Platform.Services;
using XLabs.Platform.Services.Geolocation;
using XLabs.Platform.Services.Media;
using XLabs.Platform.Services.IO;
using XLabs.Platform.Device;
using Client.Common;
using XLabs.Ioc;

namespace Client.iOS
{
	// The UIApplicationDelegate for the application. This class is responsible for launching the
	// User Interface of the application, as well as listening (and optionally responding) to
	// application events from iOS.
	[Register("AppDelegate")]
	public partial class AppDelegate : UIApplicationDelegate
	{
		//
		// This method is invoked when the application has loaded and is ready to run. In this
		// method you should instantiate the window, load the UI into it and then make the window
		// visible.
		//
		// You have 17 seconds to return from this method, or iOS will terminate your application.
		//
		public override void FinishedLaunching(UIApplication app)
		{
			if (!Resolver.IsSet)
			{
				this.SetIoc();
			}

			// aktivating Xamarin.Forms
			global::Xamarin.Forms.Forms.Init();
			// Register XLabs Services
			// DependencyService.Register<TextToSpeechService>();
			DependencyService.Register<Geolocator>();
			// DependencyService.Register<SoundService>();
			//DependencyService.Register<AppleDevice>();

			CCApplication application = new CCApplication();
			application.ApplicationDelegate = new GameAppDelegate();

			application.StartGame();
		}

		static void Main(string[] args)
		{
			UIApplication.Main(args, null, "AppDelegate");
		}

		private void SetIoc()
		{
			var resolverContainer = new SimpleContainer();

			resolverContainer.Register<IDevice>(t => AppleDevice.CurrentDevice)
                .Register<IAccelerometer>(t => t.Resolve<IDevice>().Accelerometer)
                .Register<IDependencyContainer>(resolverContainer);

			Resolver.SetResolver(resolverContainer.GetResolver());
		}
	}
}



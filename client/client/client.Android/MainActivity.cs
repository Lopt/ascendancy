using System;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Microsoft.Xna.Framework;
using XLabs.Forms;
using CocosSharp;
using Xamarin.Forms;
using XLabs.Platform.Services;
using XLabs.Platform.Services.Geolocation;
using XLabs.Platform.Services.Media;
using XLabs.Platform.Device;
using Client.Common;
using XLabs.Ioc;
using Android.Net.Rtp;

namespace Client.Droid
{
	/// <summary>
	/// Main activity.
	/// </summary>
	[Activity(
		Label = "Ascendancy",
		AlwaysRetainTaskState = true,
		Icon = "@drawable/Icon",
		Theme = "@android:style/Theme.NoTitleBar",
		ScreenOrientation = ScreenOrientation.Portrait | ScreenOrientation.ReversePortrait,
		LaunchMode = LaunchMode.SingleInstance,
		MainLauncher = true,
		ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.ScreenSize | ConfigChanges.Keyboard | ConfigChanges.KeyboardHidden)
    ]
	public class MainActivity : AndroidGameActivity
	{
		/// <summary>
		/// On the create event.
		/// </summary>
		/// <param name="bundle">Bundle.</param>
		protected override void OnCreate(Bundle bundle)
		{
			base.OnCreate(bundle);

			if (!Resolver.IsSet)
			{
				this.SetIoc();
			}
           
			// aktivating Xamarin.Forms
			global::Xamarin.Forms.Forms.Init(this, bundle);

			// Register XLabs Services
			//DependencyService.Register<TextToSpeechService>();
			DependencyService.Register<Geolocator>();

			var application = new CCApplication();
			application.ApplicationDelegate = new GameAppDelegate();
			SetContentView(application.AndroidContentView);
			application.StartGame();
		}

		/// <summary>
		/// Sets the io container.
		/// </summary>
		private void SetIoc()
		{
			var resolverContainer = new SimpleContainer();

			resolverContainer.Register<IDevice>(t => AndroidDevice.CurrentDevice)
                .Register<IAccelerometer>(t => t.Resolve<IDevice>().Accelerometer)
                .Register<IDependencyContainer>(resolverContainer);
            
			Resolver.SetResolver(resolverContainer.GetResolver());
		}
            
	}
}



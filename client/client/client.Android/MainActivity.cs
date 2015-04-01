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

namespace client
{
	[Activity (
		Label = "client.Android",
		AlwaysRetainTaskState = true,
		Icon = "@drawable/icon",
		Theme = "@android:style/Theme.NoTitleBar",
		ScreenOrientation = ScreenOrientation.Landscape | ScreenOrientation.ReverseLandscape,
		LaunchMode = LaunchMode.SingleInstance,
		MainLauncher = true,
		ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.ScreenSize | ConfigChanges.Keyboard | ConfigChanges.KeyboardHidden)
    ]
	public class MainActivity : AndroidGameActivity
	{
		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);
			// aktivating Xamarin.Forms
			global::Xamarin.Forms.Forms.Init (this, bundle);
			// Register XLabs Services
			DependencyService.Register<TextToSpeechService> ();
			DependencyService.Register<Geolocator> ();
			DependencyService.Register<SoundService> ();
			DependencyService.Register<AndroidDevice> ();

			var application = new CCApplication ();
			application.ApplicationDelegate = new GameAppDelegate ();
			SetContentView (application.AndroidContentView);
			application.StartGame ();
		}
	}
}



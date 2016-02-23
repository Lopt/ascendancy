namespace Client.Droid
{
    using System;
    using Android.App;
    using Android.Content;
    using Android.Content.PM;
    using Android.Net.Rtp;
    using Android.OS;
    using Android.Runtime;
    using Android.Views;
    using Client.Common;
    using CocosSharp;
    using Microsoft.Xna.Framework;
    using Xamarin.Forms;
    using XLabs.Forms;
    using XLabs.Ioc;
    using XLabs.Platform.Device;
    using XLabs.Platform.Services;
    using XLabs.Platform.Services.Geolocation;
    using XLabs.Platform.Services.Media;

    /// <summary>
    /// Main activity.
    /// </summary>
    [Activity(
        Label = "Ascendancy",
        AlwaysRetainTaskState = true,
        Icon = "@drawable/Icon",
        Theme = "@android:style/Theme.NoTitleBar",
        ScreenOrientation = ScreenOrientation.Portrait,
        LaunchMode = LaunchMode.SingleInstance,
        MainLauncher = true,
        ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.ScreenSize | ConfigChanges.Keyboard | ConfigChanges.KeyboardHidden)
    ]
    public class MainActivity : AndroidGameActivity
    {
        /// <summary>
        /// On the create event.
        /// </summary>
        /// <param name="bundle">Bundle values.</param>
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            if (!Resolver.IsSet)
            {
                this.SetIoc();
            }
           
            // activating Xamarin.Forms
            global::Xamarin.Forms.Forms.Init(this, bundle);

            // activating tcp connection for android
            TcpConnection.Connector = new TcpConnection();

            var application = new CCApplication();
            application.ApplicationDelegate = new GameAppDelegate();
            this.SetContentView(application.AndroidContentView);
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
                .Register<IDisplay>(t => t.Resolve<IDevice>().Display)
                .Register<INetwork>(t => t.Resolve<IDevice>().Network)
                .Register<IGeolocator, Geolocator>()
                .Register<IDependencyContainer>(resolverContainer);
            
            Resolver.SetResolver(resolverContainer.GetResolver());
        }
    }
}
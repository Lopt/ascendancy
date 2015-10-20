namespace Client.Common.Models
{
    using System;
    using CocosSharp;
    using SQLite;
    using Xamarin.Forms;
    using XLabs.Ioc;
    using XLabs.Platform.Device;
    using XLabs.Platform.Services;
    using XLabs.Platform.Services.Media;

    /// <summary>
    /// The Device as a singleton class for device information.
    /// </summary>
    public sealed class Device
    {
        #region Singelton

        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <value>The instance.</value>
        public static Device Instance
        {
            get
            { 
                return Singleton.Value;
            }
        }

        /// <summary>
        /// Prevents a default instance of the <see cref="Device"/> class from being created.
        /// </summary>
        private Device()
        {
            m_device = Resolver.Resolve<IDevice>();
        }

        /// <summary>
        /// The singleton instance.
        /// </summary>
        private static readonly Lazy<Device> Singleton =
            new Lazy<Device>(() => new Device());

        #endregion

        #region Device

        /// <summary>
        /// The m device.
        /// </summary>
        private readonly IDevice m_device = null;

        // TODO solve Exception IAccelerometer why returns null

        /// <summary>
        /// Gets the accelerometer.
        /// </summary>
        /// <value>The accelerometer.</value>
        public IAccelerometer Accelerometer
        { 
            get
            {
                return m_device.Accelerometer;
            }

            private set
            {
            } 
        }

        /// <summary>
        /// Gets the battery.
        /// </summary>
        /// <value>The battery.</value>
        public IBattery Battery
        {
            get
            {
                return m_device.Battery;
            }

            private set
            {
            }
        }

        /// <summary>
        /// Gets the bluetooth hub.
        /// </summary>
        /// <value>The bluetooth hub.</value>
        public IBluetoothHub BluetoothHub
        { 
            get
            {
                return m_device.BluetoothHub;
            }

            private set
            {
            }
        }

        /// <summary>
        /// Gets the display.
        /// </summary>
        /// <value>The display.</value>
        public IDisplay Display
        { 
            get
            {
                return m_device.Display;
            }

            private set
            {
            }
        }

        // TODO solve Exception IGyroscope why returns null

        /// <summary>
        /// Gets the gyroscope.
        /// </summary>
        /// <value>The gyroscope.</value>
        public IGyroscope Gyroscope
        { 
            get
            {
                return m_device.Gyroscope;
            }

            private set
            {
            }
        }

        /// <summary>
        /// Gets the media picker.
        /// </summary>
        /// <value>The media picker.</value>
        public IMediaPicker MediaPicker
        { 
            get
            {
                return m_device.MediaPicker;
            }

            private set
            {
            }
        }

        // TODO solve Exception IAudioStream why returns null

        /// <summary>
        /// Gets the microphone.
        /// </summary>
        /// <value>The microphone.</value>
        public IAudioStream Microphone
        { 
            get
            {
                return m_device.Microphone;
            }

            private set
            {
            }
        }

        /// <summary>
        /// Gets the network.
        /// </summary>
        /// <value>The network.</value>
        public INetwork Network
        { 
            get
            {
                return m_device.Network;
            }

            private set
            {
            }
        }

        /// <summary>
        /// Gets the phone service.
        /// </summary>
        /// <value>The phone service.</value>
        public IPhoneService PhoneService
        { 
            get
            {
                return m_device.PhoneService;
            }

            private set
            {
            }
        }

        /// <summary>
        /// Gets the battery level.
        /// </summary>
        /// <value>The battery level.</value>
        public string BatteryLevel
        { 
            get
            { 
                return m_device.Battery.Level.ToString();
            }

            private set
            {
            }
        }

        /// <summary>
        /// Gets the device identifier.
        /// </summary>
        /// <value>The device identifier.</value>
        public string DeviceId
        { 
            get
            {
                return m_device.Id;
            }

            private set
            {
            }
        }

        /// <summary>
        /// Gets the firmware version.
        /// </summary>
        /// <value>The firmware version.</value>
        public string FirmwareVersion
        { 
            get
            {
                return m_device.FirmwareVersion;
            }

            private set
            {
            }
        }

        /// <summary>
        /// Gets the hardware version.
        /// </summary>
        /// <value>The hardware version.</value>
        public string HardwareVersion
        { 
            get
            {
                return m_device.HardwareVersion; 
            }

            private set
            {
            }
        }

        /// <summary>
        /// Gets the language code.
        /// </summary>
        /// <value>The language code.</value>
        public string LanguageCode
        { 
            get
            {
                return m_device.LanguageCode; 
            }

            private set
            {
            }
        }

        /// <summary>
        /// Gets the manufacturer.
        /// </summary>
        /// <value>The manufacturer.</value>
        public string Manufacturer
        { 
            get
            {
                return m_device.Manufacturer;
            }

            private set
            {
            }
        }

        /// <summary>
        /// Gets the name of the device.
        /// </summary>
        /// <value>The name of the device.</value>
        public string DeviceName
        { 
            get
            { 
                return m_device.Name;
            }

            private set
            {
            }
        }

        /// <summary>
        /// Gets the time zone.
        /// </summary>
        /// <value>The time zone.</value>
        public string TimeZone
        { 
            get
            { 
                return m_device.TimeZone; 
            }

            private set
            {
            }
        }

        /// <summary>
        /// Gets the time zone offset.
        /// </summary>
        /// <value>The time zone offset.</value>
        public string TimeZoneOffset
        { 
            get
            { 
                return m_device.TimeZoneOffset.ToString(); 
            }

            private set
            {
            }
        }

        /// <summary>
        /// Gets the device memory.
        /// </summary>
        /// <value>The device memory.</value>
        public string DeviceTotalMemory
        { 
            get
            { 
                return m_device.TotalMemory.ToString();
            }

            private set
            {
            }
        }

        #endregion
    }
}

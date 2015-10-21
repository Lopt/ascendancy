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
        #region Singleton

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
        }

        #endregion
    }
}

namespace Client.Common.Models
{
    using System;
    using SQLite;
    using XLabs.Platform.Device;
    using Xamarin.Forms;
    using XLabs.Platform.Services.Media;
    using XLabs.Platform.Services;
    using CocosSharp;
    using XLabs.Ioc;

    /// <summary>
    /// The Device as a singleton class for device information.
    /// </summary>
    [Table("Device")]
    public sealed class Device : ViewBaseModel
    {
        #region Singelton

        /// <summary>
        /// The m instance.
        /// </summary>
        private static readonly Device Singleton = new Device();

        /// <summary>
        /// Prevents a default instance of the <see cref="Device"/> class from being created.
        /// </summary>
        private Device()
        {
            m_device = Resolver.Resolve<IDevice>();
            Accelerometer = Resolver.Resolve<IAccelerometer>();
            BluetoothHub = m_device.BluetoothHub;
            Display = m_device.Display;
            Gyroscope = m_device.Gyroscope;
            MediaPicker = m_device.MediaPicker;
            Microphone = m_device.Microphone;
            Network = m_device.Network;
            PhoneService = m_device.PhoneService;
            Battery = m_device.Battery;

            Battery.OnLevelChange += (object sender, XLabs.EventArgs<int> e) =>
            {
                BatteryLevel = Battery.Level.ToString();
            };

            BatteryLevel = Battery.Level.ToString();
            DeviceId = m_device.Id;
            FirmwareVersion = m_device.FirmwareVersion;
            HardwareVersion = m_device.HardwareVersion;
            LanguageCode = m_device.LanguageCode;
            Manufacturer = m_device.Manufacturer;
            DeviceName = m_device.Name;
            TimeZone = m_device.TimeZone;
            TimeZoneOffset = m_device.TimeZoneOffset.ToString();
            DeviceMemory = m_device.TotalMemory.ToString();
        }

        /// <summary>
        /// Gets the get instance.
        /// </summary>
        /// <value>The get instance.</value>
        public static Device GetInstance
        {
            get
            { 
                return Singleton;
            }
        }

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
            get; 
            private set; 
        }

        /// <summary>
        /// Gets the battery.
        /// </summary>
        /// <value>The battery.</value>
        public IBattery Battery
        {
            get;
            private set; 
        }

        /// <summary>
        /// Gets the bluetooth hub.
        /// </summary>
        /// <value>The bluetooth hub.</value>
        public IBluetoothHub BluetoothHub
        { 
            get; 
            private set; 
        }

        /// <summary>
        /// Gets the display.
        /// </summary>
        /// <value>The display.</value>
        public IDisplay Display
        { 
            get; 
            private set; 
        }

        // TODO solve Exception IGyroscope why returns null

        /// <summary>
        /// Gets the gyroscope.
        /// </summary>
        /// <value>The gyroscope.</value>
        public IGyroscope Gyroscope
        { 
            get; 
            private set; 
        }

        /// <summary>
        /// Gets the media picker.
        /// </summary>
        /// <value>The media picker.</value>
        public IMediaPicker MediaPicker
        { 
            get; 
            private set; 
        }

        // TODO solve Exception IAudioStream why returns null

        /// <summary>
        /// Gets the microphone.
        /// </summary>
        /// <value>The microphone.</value>
        public IAudioStream Microphone
        { 
            get; 
            private set; 
        }

        /// <summary>
        /// Gets the network.
        /// </summary>
        /// <value>The network.</value>
        public INetwork Network
        { 
            get; 
            private set; 
        }

        // TODO solve Exception IPhoneService why returns null

        /// <summary>
        /// Gets the phone service.
        /// </summary>
        /// <value>The phone service.</value>
        public IPhoneService PhoneService
        { 
            get; 
            private set; 
        }

        #endregion

        #region ViewPoperties

        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        [PrimaryKey, AutoIncrement]
        public int Id
        { 
            get; 
            set; 
        }

        /// <summary>
        /// Gets or sets the battery level.
        /// </summary>
        /// <value>The battery level.</value>
        [Column("BatteryLevel")]
        public string BatteryLevel
        { 
            get
            { 
                return m_batteryLevel;
            }

            set
            {
                SetProperty(m_batteryLevel, value, val =>
                    {
                        m_batteryLevel = val;
                    },
                    PropertyNameBatteryLevel);
            }
        }

        /// <summary>
        /// The property name battery level.
        /// </summary>
        public static string PropertyNameBatteryLevel = "BatteryLevel";

        /// <summary>
        /// The m_battery level.
        /// </summary>
        private string m_batteryLevel;

        /// <summary>
        /// Gets or sets the device identifier.
        /// </summary>
        /// <value>The device identifier.</value>
        [Column("DeviceId")]
        public string DeviceId
        { 
            get
            {
                return m_deviceId;
            }

            set
            {
                SetProperty(m_deviceId, value, (val) =>
                    {
                        m_deviceId = val;

                    }, PropertyNameDeviceId);
            }
        }

        /// <summary>
        /// The property name device identifier.
        /// </summary>
        public static string PropertyNameDeviceId = "DeviceId";

        /// <summary>
        /// The m_device identifier.
        /// </summary>
        private string m_deviceId;

        /// <summary>
        /// Gets or sets the firmware version.
        /// </summary>
        /// <value>The firmware version.</value>
        [Column("FirmwareVersion")]
        public string FirmwareVersion
        { 
            get
            {
                return m_firmwareVersion;
            }
            set
            {
                SetProperty(m_firmwareVersion, value, (val) =>
                    {
                        m_firmwareVersion = val;

                    }, PropertyNameFirmwareVersion);
            }
        }

        /// <summary>
        /// The property name firmware version.
        /// </summary>
        public static string PropertyNameFirmwareVersion = "FirmwareVersion";
        /// <summary>
        /// The m_firmware version.
        /// </summary>
        private string m_firmwareVersion;

        /// <summary>
        /// Gets or sets the hardware version.
        /// </summary>
        /// <value>The hardware version.</value>
        [Column("HardwareVersion")]
        public string HardwareVersion
        { 
            get
            {
                return m_hardwareVersion; 
            }
            set
            {
                SetProperty(m_hardwareVersion, value, (val) =>
                    {
                        m_hardwareVersion = val;

                    }, PropertyNameHardwareVersion);
            }
        }

        /// <summary>
        /// The property name hardware version.
        /// </summary>
        public static string PropertyNameHardwareVersion = "HardwareVersion";
        /// <summary>
        /// The m_hardware version.
        /// </summary>
        private string m_hardwareVersion;

        /// <summary>
        /// Gets or sets the language code.
        /// </summary>
        /// <value>The language code.</value>
        [Column("LanguageCode")]
        public string LanguageCode
        { 
            get
            {
                return m_languageCode; 
            }
            set
            {
                SetProperty(m_languageCode, value, (val) =>
                    {
                        m_languageCode = val;

                    }, PropertyNameLanguageCode);
            }
        }

        /// <summary>
        /// The property name language code.
        /// </summary>
        public static string PropertyNameLanguageCode = "LanguageCode";
        /// <summary>
        /// The m_language code.
        /// </summary>
        private string m_languageCode;

        /// <summary>
        /// Gets or sets the manufacturer.
        /// </summary>
        /// <value>The manufacturer.</value>
        [Column("Manufacturer")]
        public string Manufacturer
        { 
            get
            {
                return m_manufacturer;
            }
            set
            {
                SetProperty(m_manufacturer, value, (val) =>
                    {
                        m_manufacturer = val;

                    }, PropertyNameManufacturer);
            }
        }

        /// <summary>
        /// The property name manufacturer.
        /// </summary>
        public static string PropertyNameManufacturer = "Manufacturer";
        /// <summary>
        /// The m_manufacturer.
        /// </summary>
        private string m_manufacturer;

        /// <summary>
        /// Gets or sets the name of the device.
        /// </summary>
        /// <value>The name of the device.</value>
        [Column("DeviceName")]
        public string DeviceName
        { 
            get
            { 
                return m_deviceName;
            }
            set
            {
                SetProperty(m_deviceName, value, (val) =>
                    {
                        m_deviceName = val;

                    }, PropertyNameDeviceName);
            }
        }

        /// <summary>
        /// The name of the property name device.
        /// </summary>
        public static string PropertyNameDeviceName = "DeviceName";
        /// <summary>
        /// The name of the m_device.
        /// </summary>
        private string m_deviceName;

        /// <summary>
        /// Gets or sets the time zone.
        /// </summary>
        /// <value>The time zone.</value>
        [Column("TimeZone")]
        public string TimeZone
        { 
            get
            { 
                return m_timeZone; 
            }
            set
            {
                SetProperty(m_timeZone, value, (val) =>
                    {
                        m_timeZone = val;

                    }, PropertyNameTimeZone);
            }
        }

        /// <summary>
        /// The property name time zone.
        /// </summary>
        public static string PropertyNameTimeZone = "TimeZone";
        /// <summary>
        /// The m_time zone.
        /// </summary>
        private string m_timeZone;

        /// <summary>
        /// Gets or sets the time zone offset.
        /// </summary>
        /// <value>The time zone offset.</value>
        [Column("TimeZoneOffset")]
        public string TimeZoneOffset
        { 
            get
            { 
                return m_timeZoneOffset; 
            }
            set
            {
                SetProperty(m_timeZoneOffset, value, (val) =>
                    {
                        m_timeZoneOffset = val;

                    }, PropertyNameTimeZoneOffset);
            }
        }

        /// <summary>
        /// The property name time zone offset.
        /// </summary>
        public static string PropertyNameTimeZoneOffset = "TimeZoneOffset";
        /// <summary>
        /// The m_time zone offset.
        /// </summary>
        private string m_timeZoneOffset;

        /// <summary>
        /// Gets or sets the device memory.
        /// </summary>
        /// <value>The device memory.</value>
        [Column("DeviceMemory")]
        public string DeviceMemory
        { 
            get
            { 
                return m_deviceMemory;
            }
            set
            {
                SetProperty(m_deviceMemory, value, (val) =>
                    {
                        m_deviceMemory = val;

                    }, PropertyNameDeviceMemory);
            }
        }

        /// <summary>
        /// The property name device memory.
        /// </summary>
        public static string PropertyNameDeviceMemory = "DeviceMemory";
        /// <summary>
        /// The m_device memory.
        /// </summary>
        private string m_deviceMemory;

        #endregion
      
    }
}


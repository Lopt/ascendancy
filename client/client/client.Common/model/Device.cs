using System;
using SQLite;
using client.Common.model;
using XLabs.Platform.Device;
using Xamarin.Forms;
using XLabs.Platform.Services.Media;
using XLabs.Platform.Services;
using CocosSharp;
using XLabs.Ioc;

//using client.Common.helper;

namespace client.Common
{
    [Table("Device")]
    public sealed class Device : viewBaseModel
    {

        #region Singelton

        private static readonly Device _instance = new Device();

        private Device()
        {
            _device = Resolver.Resolve<IDevice>();
            Accelerometer = Resolver.Resolve<IAccelerometer>();//_device.Accelerometer;
            BluetoothHub = _device.BluetoothHub;
            Display = _device.Display;
            Gyroscope = _device.Gyroscope;
            MediaPicker = _device.MediaPicker;
            Microphone = _device.Microphone;
            Network = _device.Network;
            PhoneService = _device.PhoneService;
            Battery = _device.Battery;

            Battery.OnLevelChange += (object sender, XLabs.EventArgs<int> e) =>
            {
                BatteryLevel = Battery.Level.ToString();
            };

            BatteryLevel = Battery.Level.ToString();
            DeviceId = _device.Id;
            FirmwareVersion = _device.FirmwareVersion;
            HardwareVersion = _device.HardwareVersion;
            LanguageCode = _device.LanguageCode;
            Manufacturer = _device.Manufacturer;
            DeviceName = _device.Name;
            TimeZone = _device.TimeZone;
            TimeZoneOffset = _device.TimeZoneOffset.ToString();
            DeviceMemory = _device.TotalMemory.ToString();

       
        }

        public static Device GetInstance{ get { return _instance; } }

        #endregion

        #region Device

        private readonly IDevice _device = null;

        //TODO solve Exception IAccelerometer why returns null
        public IAccelerometer Accelerometer{ get; private set; }

        public IBattery Battery { get; private set; }

        public IBluetoothHub BluetoothHub{ get; private set; }

        public IDisplay Display{ get; private set; }

        //TODO solve Exception IGyroscope why returns null
        public IGyroscope Gyroscope{ get; private set; }

        public IMediaPicker MediaPicker{ get; private set; }

        //TODO solve Exception IAudioStream why returns null
        public IAudioStream Microphone{ get; private set; }

        public INetwork Network{ get; private set; }

        //TODO solve Exception IPhoneService why returns null
        public IPhoneService PhoneService{ get; private set; }

        #endregion

        #region ViewPoperties

        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [Column("BatteryLevel")]
        public string BatteryLevel
        { 
            get { return _batteryLevel; }
            set
            {
                SetProperty(_batteryLevel, value, (val) =>
                    {
                        _batteryLevel = val;
                    }, PropertyNameBatteryLevel);
            }
        }

        public static string PropertyNameBatteryLevel = "BatteryLevel";
        private string _batteryLevel;

        [Column("DeviceId")]
        public string DeviceId
        { 
            get { return _deviceId; }
            set
            {
                SetProperty(_deviceId, value, (val) =>
                    {
                        _deviceId = val;
                    }, PropertyNameDeviceId);
            }
        }

        public static string PropertyNameDeviceId = "DeviceId";
        private string _deviceId;

        [Column("FirmwareVersion")]
        public string FirmwareVersion
        { 
            get { return _firmwareVersion; }
            set
            {
                SetProperty(_firmwareVersion, value, (val) =>
                    {
                        _firmwareVersion = val;
                    }, PropertyNameFirmwareVersion);
            }
        }

        public static string PropertyNameFirmwareVersion = "FirmwareVersion";
        private string _firmwareVersion;

        [Column("HardwareVersion")]
        public string HardwareVersion
        { 
            get { return _hardwareVersion; }
            set
            {
                SetProperty(_hardwareVersion, value, (val) =>
                    {
                        _hardwareVersion = val;
                    }, PropertyNameHardwareVersion);
            }
        }

        public static string PropertyNameHardwareVersion = "HardwareVersion";
        private string _hardwareVersion;

        [Column("LanguageCode")]
        public string LanguageCode
        { 
            get { return _languageCode; }
            set
            {
                SetProperty(_languageCode, value, (val) =>
                    {
                        _languageCode = val;
                    }, PropertyNameLanguageCode);
            }
        }

        public static string PropertyNameLanguageCode = "LanguageCode";
        private string _languageCode;

        [Column("Manufacturer")]
        public string Manufacturer
        { 
            get { return _manufacturer; }
            set
            {
                SetProperty(_manufacturer, value, (val) =>
                    {
                        _manufacturer = val;
                    }, PropertyNameManufacturer);
            }
        }

        public static string PropertyNameManufacturer = "Manufacturer";
        private string _manufacturer;

        [Column("DeviceName")]
        public string DeviceName
        { 
            get { return _deviceName; }
            set
            {
                SetProperty(_deviceName, value, (val) =>
                    {
                        _deviceName = val;
                    }, PropertyNameDeviceName);
            }
        }

        public static string PropertyNameDeviceName = "DeviceName";
        private string _deviceName;

        [Column("TimeZone")]
        public string TimeZone
        { 
            get { return _timeZone; }
            set
            {
                SetProperty(_timeZone, value, (val) =>
                    {
                        _timeZone = val;
                    }, PropertyNameTimeZone);
            }
        }

        public static string PropertyNameTimeZone = "TimeZone";
        private string _timeZone;

        [Column("TimeZoneOffset")]
        public string TimeZoneOffset
        { 
            get { return _timeZoneOffset; }
            set
            {
                SetProperty(_timeZoneOffset, value, (val) =>
                    {
                        _timeZoneOffset = val;
                    }, PropertyNameTimeZoneOffset);
            }
        }

        public static string PropertyNameTimeZoneOffset = "TimeZoneOffset";
        private string _timeZoneOffset;


        [Column("DeviceMemory")]
        public string DeviceMemory
        { 
            get { return _deviceMemory; }
            set
            {
                SetProperty(_deviceMemory, value, (val) =>
                    {
                        _deviceMemory = val;
                    }, PropertyNameDeviceMemory);
            }
        }

        public static string PropertyNameDeviceMemory = "DeviceMemory";
        private string _deviceMemory;

        #endregion
      
    }
}


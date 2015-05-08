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
	[Table ("Device")]
	public sealed class Device : viewBaseModel
	{

		#region Singelton

		private static readonly Device m_instance = new Device ();

		private Device ()
		{
			m_device = Resolver.Resolve<IDevice> ();
			Accelerometer = Resolver.Resolve<IAccelerometer> ();//_device.Accelerometer;
			BluetoothHub = m_device.BluetoothHub;
			Display = m_device.Display;
			Gyroscope = m_device.Gyroscope;
			MediaPicker = m_device.MediaPicker;
			Microphone = m_device.Microphone;
			Network = m_device.Network;
			PhoneService = m_device.PhoneService;
			Battery = m_device.Battery;

			Battery.OnLevelChange += (object sender, XLabs.EventArgs<int> e) => {
				BatteryLevel = Battery.Level.ToString ();
			};

			BatteryLevel = Battery.Level.ToString ();
			DeviceId = m_device.Id;
			FirmwareVersion = m_device.FirmwareVersion;
			HardwareVersion = m_device.HardwareVersion;
			LanguageCode = m_device.LanguageCode;
			Manufacturer = m_device.Manufacturer;
			DeviceName = m_device.Name;
			TimeZone = m_device.TimeZone;
			TimeZoneOffset = m_device.TimeZoneOffset.ToString ();
			DeviceMemory = m_device.TotalMemory.ToString ();

       
		}

		public static Device GetInstance{ get { return m_instance; } }

		#endregion

		#region Device

		private readonly IDevice m_device = null;

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

		[Column ("BatteryLevel")]
		public string BatteryLevel { 
			get { return m_batteryLevel; }
			set {
				SetProperty (m_batteryLevel, value, (val) => {
					m_batteryLevel = val;
				}, PropertyNameBatteryLevel);
			}
		}

		public static string PropertyNameBatteryLevel = "BatteryLevel";
		private string m_batteryLevel;

		[Column ("DeviceId")]
		public string DeviceId { 
			get { return m_deviceId; }
			set {
				SetProperty (m_deviceId, value, (val) => {
					m_deviceId = val;
				}, PropertyNameDeviceId);
			}
		}

		public static string PropertyNameDeviceId = "DeviceId";
		private string m_deviceId;

		[Column ("FirmwareVersion")]
		public string FirmwareVersion { 
			get { return m_firmwareVersion; }
			set {
				SetProperty (m_firmwareVersion, value, (val) => {
					m_firmwareVersion = val;
				}, PropertyNameFirmwareVersion);
			}
		}

		public static string PropertyNameFirmwareVersion = "FirmwareVersion";
		private string m_firmwareVersion;

		[Column ("HardwareVersion")]
		public string HardwareVersion { 
			get { return m_hardwareVersion; }
			set {
				SetProperty (m_hardwareVersion, value, (val) => {
					m_hardwareVersion = val;
				}, PropertyNameHardwareVersion);
			}
		}

		public static string PropertyNameHardwareVersion = "HardwareVersion";
		private string m_hardwareVersion;

		[Column ("LanguageCode")]
		public string LanguageCode { 
			get { return m_languageCode; }
			set {
				SetProperty (m_languageCode, value, (val) => {
					m_languageCode = val;
				}, PropertyNameLanguageCode);
			}
		}

		public static string PropertyNameLanguageCode = "LanguageCode";
		private string m_languageCode;

		[Column ("Manufacturer")]
		public string Manufacturer { 
			get { return m_manufacturer; }
			set {
				SetProperty (m_manufacturer, value, (val) => {
					m_manufacturer = val;
				}, PropertyNameManufacturer);
			}
		}

		public static string PropertyNameManufacturer = "Manufacturer";
		private string m_manufacturer;

		[Column ("DeviceName")]
		public string DeviceName { 
			get { return m_deviceName; }
			set {
				SetProperty (m_deviceName, value, (val) => {
					m_deviceName = val;
				}, PropertyNameDeviceName);
			}
		}

		public static string PropertyNameDeviceName = "DeviceName";
		private string m_deviceName;

		[Column ("TimeZone")]
		public string TimeZone { 
			get { return m_timeZone; }
			set {
				SetProperty (m_timeZone, value, (val) => {
					m_timeZone = val;
				}, PropertyNameTimeZone);
			}
		}

		public static string PropertyNameTimeZone = "TimeZone";
		private string m_timeZone;

		[Column ("TimeZoneOffset")]
		public string TimeZoneOffset { 
			get { return m_timeZoneOffset; }
			set {
				SetProperty (m_timeZoneOffset, value, (val) => {
					m_timeZoneOffset = val;
				}, PropertyNameTimeZoneOffset);
			}
		}

		public static string PropertyNameTimeZoneOffset = "TimeZoneOffset";
		private string m_timeZoneOffset;


		[Column ("DeviceMemory")]
		public string DeviceMemory { 
			get { return m_deviceMemory; }
			set {
				SetProperty (m_deviceMemory, value, (val) => {
					m_deviceMemory = val;
				}, PropertyNameDeviceMemory);
			}
		}

		public static string PropertyNameDeviceMemory = "DeviceMemory";
		private string m_deviceMemory;

		#endregion
      
	}
}


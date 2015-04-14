using System;
using CocosSharp;
using Xamarin.Forms;
using XLabs.Forms;
using XLabs.Platform.Services.Geolocation;
using System.Threading.Tasks;
using System.Threading;
using System.Runtime.InteropServices;

namespace client.Common
{
    public class DeviceScene: CCScene
    {
        CCLayerColor mDeviceLayer;

        public DeviceScene(CCWindow _MainWindow)
            : base(_MainWindow)
        {
            mDeviceLayer = new DeviceLayer();

            this.AddChild(mDeviceLayer);
        }
    }


    public class DeviceLayer: CCLayerColor
    {
        Device mDevice = null;

        CCLabelTtf LabelDeviceId;
        CCLabelTtf LabelDeviceName;
        CCLabelTtf LabelManufacturer;
        CCLabelTtf LabelBatteryLevel;
        CCLabelTtf LabelFirmwareVersion;
        CCLabelTtf LabelHardwareVersion;
        CCLabelTtf LabelLanguageCode;
        CCLabelTtf LabelTimeZone;
        CCLabelTtf LabelTimeZoneOffset;
        CCLabelTtf LabelDeviceMemory;
        CCLabelTtf LabelNetworkStatus;
       

        string DeviceId;
        string DeviceName;
        string Manufacturer;
        string BatteryLevel;
        string FirmwareVersion;
        string HardwareVersion;
        string LanguageCode;
        string TimeZone;
        string TimeZoneOffset;
        string DeviceMemory;
        string NetworkStatus;

        public DeviceLayer()
            : base()
        {
            mDevice = Device.GetInstance;

            DeviceId = Device.PropertyNameDeviceId;
            DeviceName = Device.PropertyNameDeviceName;
            Manufacturer = Device.PropertyNameManufacturer;
            BatteryLevel = Device.PropertyNameBatteryLevel;
            FirmwareVersion = Device.PropertyNameFirmwareVersion;
            HardwareVersion = Device.PropertyNameHardwareVersion;
            LanguageCode = Device.PropertyNameLanguageCode;
            TimeZone = Device.PropertyNameTimeZone;
            TimeZoneOffset = Device.PropertyNameTimeZoneOffset;
            DeviceMemory = Device.PropertyNameDeviceMemory;
            NetworkStatus = "NetworkStatus";

            LabelDeviceId = new CCLabelTtf(DeviceId, "arial", 22);
            LabelDeviceName = new CCLabelTtf(DeviceName, "arial", 22);
            LabelManufacturer = new CCLabelTtf(Manufacturer, "arial", 22);
            LabelBatteryLevel = new CCLabelTtf(BatteryLevel, "arial", 22);
            LabelFirmwareVersion = new CCLabelTtf(FirmwareVersion, "arial", 22);
            LabelHardwareVersion = new CCLabelTtf(HardwareVersion, "arial", 22);
            LabelLanguageCode = new CCLabelTtf(LanguageCode, "arial", 22);
            LabelTimeZone = new CCLabelTtf(TimeZone, "arial", 22);
            LabelTimeZoneOffset = new CCLabelTtf(TimeZoneOffset, "arial", 22);
            LabelDeviceMemory = new CCLabelTtf(DeviceMemory, "arial", 22);    
            LabelNetworkStatus = new CCLabelTtf(NetworkStatus, "arial", 22); 


            var touchListener = new CCEventListenerTouchAllAtOnce();
            touchListener.OnTouchesEnded = (touches, ccevent) =>
            {
                Window.DefaultDirector.ReplaceScene(new GeolocationScene(Window));
            };


            this.AddEventListener(touchListener, this);

            this.Color = CCColor3B.Gray;
            this.Opacity = 255;

            this.AddChild(LabelDeviceId);
            this.AddChild(LabelDeviceName);
            this.AddChild(LabelManufacturer);
            this.AddChild(LabelBatteryLevel);
            this.AddChild(LabelFirmwareVersion);
            this.AddChild(LabelHardwareVersion);
            this.AddChild(LabelLanguageCode);
            this.AddChild(LabelTimeZone);
            this.AddChild(LabelTimeZoneOffset);
            this.AddChild(LabelDeviceMemory);
            this.AddChild(LabelNetworkStatus);

            this.Schedule(SetDeviceInfo);

        }

        protected override void AddedToScene()
        {
            base.AddedToScene();

            LabelDeviceId.PositionX = this.VisibleBoundsWorldspace.MinX + 20;
            LabelDeviceId.PositionY = this.VisibleBoundsWorldspace.MaxY - 20;
            LabelDeviceId.AnchorPoint = CCPoint.AnchorUpperLeft;

            LabelDeviceName.PositionX = VisibleBoundsWorldspace.MinX + 20;
            LabelDeviceName.PositionY = VisibleBoundsWorldspace.MaxY - 50;
            LabelDeviceName.AnchorPoint = CCPoint.AnchorUpperLeft;

            LabelManufacturer.PositionX = VisibleBoundsWorldspace.MinX + 20;
            LabelManufacturer.PositionY = VisibleBoundsWorldspace.MaxY - 80;
            LabelManufacturer.AnchorPoint = CCPoint.AnchorUpperLeft;

            LabelBatteryLevel.PositionX = VisibleBoundsWorldspace.MinX + 20;
            LabelBatteryLevel.PositionY = VisibleBoundsWorldspace.MaxY - 110;
            LabelBatteryLevel.AnchorPoint = CCPoint.AnchorUpperLeft;

            LabelFirmwareVersion.PositionX = VisibleBoundsWorldspace.MinX + 20;
            LabelFirmwareVersion.PositionY = VisibleBoundsWorldspace.MaxY - 140;
            LabelFirmwareVersion.AnchorPoint = CCPoint.AnchorUpperLeft;

            LabelHardwareVersion.PositionX = VisibleBoundsWorldspace.MinX + 20;
            LabelHardwareVersion.PositionY = VisibleBoundsWorldspace.MaxY - 170;
            LabelHardwareVersion.AnchorPoint = CCPoint.AnchorUpperLeft;

            LabelLanguageCode.PositionX = VisibleBoundsWorldspace.MinX + 20;
            LabelLanguageCode.PositionY = VisibleBoundsWorldspace.MaxY - 200;
            LabelLanguageCode.AnchorPoint = CCPoint.AnchorUpperLeft;

            LabelTimeZone.PositionX = VisibleBoundsWorldspace.MinX + 20;
            LabelTimeZone.PositionY = VisibleBoundsWorldspace.MaxY - 230;
            LabelTimeZone.AnchorPoint = CCPoint.AnchorUpperLeft;

            LabelTimeZoneOffset.PositionX = VisibleBoundsWorldspace.MinX + 20;
            LabelTimeZoneOffset.PositionY = VisibleBoundsWorldspace.MaxY - 260;
            LabelTimeZoneOffset.AnchorPoint = CCPoint.AnchorUpperLeft;

            LabelDeviceMemory.PositionX = VisibleBoundsWorldspace.MinX + 20;
            LabelDeviceMemory.PositionY = VisibleBoundsWorldspace.MaxY - 290;
            LabelDeviceMemory.AnchorPoint = CCPoint.AnchorUpperLeft;

            LabelNetworkStatus.PositionX = VisibleBoundsWorldspace.MinX + 20;
            LabelNetworkStatus.PositionY = VisibleBoundsWorldspace.MaxY - 320;
            LabelNetworkStatus.AnchorPoint = CCPoint.AnchorUpperLeft;
        }

        void SetDeviceInfo(float FrameTimesInSecond)
        {

            LabelDeviceId.Text = DeviceId + " = " + mDevice.DeviceId;
            LabelDeviceName.Text = DeviceName + " = " + mDevice.DeviceName;
            LabelManufacturer.Text = Manufacturer + " = " + mDevice.Manufacturer;
            LabelBatteryLevel.Text = BatteryLevel + " = " + mDevice.BatteryLevel;
            LabelFirmwareVersion.Text = FirmwareVersion + " = " + mDevice.FirmwareVersion;
            LabelHardwareVersion.Text = HardwareVersion + " = " + mDevice.HardwareVersion;
            LabelLanguageCode.Text = LanguageCode + " = " + mDevice.LanguageCode;
            LabelTimeZone.Text = TimeZone + " = " + mDevice.TimeZone;
            LabelTimeZoneOffset.Text = TimeZoneOffset + " = " + mDevice.TimeZoneOffset;
            LabelDeviceMemory.Text = DeviceMemory + " = " + mDevice.DeviceMemory;
            LabelNetworkStatus.Text = NetworkStatus + " = " + mDevice.Network.InternetConnectionStatus().ToString();

        }

    }
}


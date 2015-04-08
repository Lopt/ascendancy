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
    public class TestScene: CCScene
    {
        CCLayer mTestLayer;

        public TestScene(CCWindow _MainWindow)
            : base(_MainWindow)
        {
            mTestLayer = new TestLayer();

            this.AddChild(mTestLayer);
        }
    }


    public class TestLayer: CCLayer
    {
        Geolocation mGeolocation;

        CCLabelTtf LabelLatitude;
        CCLabelTtf LabelLongitude;
        CCLabelTtf LabelPosStatus;
        CCLabelTtf LabelTimestamp;
        CCLabelTtf LabelAltitude;
        CCLabelTtf LabelHeading;
        CCLabelTtf LabelAccuracy;


        CCLabelTtf LabelLastLatitude;
        CCLabelTtf LabelLastLongitude;
        CCLabelTtf LabelLastTimestamp;
        CCLabelTtf LabelLastAltitude;

        string Latitude;
        string Longitude;
        string PositionStatus;
        string Timestamp;
        string Altitude;
        string Heading;
        string Accuracy;

        string LastLatitude;
        string LastLongitude;
        string LastPositionStatus;
        string LastTimestamp;
        string LastAltitude;
        string LastHeading;
        string LastAccuracy;

        public TestLayer()
        {
            mGeolocation = Geolocation.GetInstance;
            Latitude = Geolocation.PropertyNameLatitude;
            Longitude = Geolocation.PropertyNameLongitude;
            PositionStatus = Geolocation.PropertyNameStatus;
            Timestamp = Geolocation.PropertyNameTimeStamp;
            Altitude = Geolocation.PropertyNameAltitude;
            Heading = Geolocation.PropertyNameHeading;
            Accuracy = Geolocation.PropertyNameAccuracy;

            LastLatitude = Geolocation.PropertyNameLatitude;
            LastLongitude = Geolocation.PropertyNameLongitude;
            LastPositionStatus = Geolocation.PropertyNameStatus;
            LastTimestamp = Geolocation.PropertyNameTimeStamp;
            LastAltitude = Geolocation.PropertyNameAltitude;
            LastHeading = Geolocation.PropertyNameHeading;
            LastAccuracy = Geolocation.PropertyNameAccuracy;

            LabelLatitude = new CCLabelTtf(Latitude, "arial", 22);
            LabelLongitude = new CCLabelTtf(Longitude, "arial", 22);
            LabelPosStatus = new CCLabelTtf(PositionStatus, "arial", 22);
            LabelTimestamp = new CCLabelTtf(Timestamp, "arial", 22);
            LabelAltitude = new CCLabelTtf(Altitude, "arial", 22);
            LabelHeading = new CCLabelTtf(Heading, "arial", 22);
            LabelAccuracy = new CCLabelTtf(Accuracy, "arial", 22);

            LabelLastLatitude = new CCLabelTtf(Latitude, "arial", 22);
            LabelLastLongitude = new CCLabelTtf(Longitude, "arial", 22);
            LabelLastTimestamp = new CCLabelTtf(Timestamp, "arial", 22);
            LabelLastAltitude = new CCLabelTtf(Altitude, "arial", 22);

            LabelLastLatitude.Color = CCColor3B.Red;
            LabelLastLongitude.Color = CCColor3B.Red;
            LabelLastTimestamp.Color = CCColor3B.Red;
            LabelLastAltitude.Color = CCColor3B.Red;

            this.AddChild(LabelLatitude);
            this.AddChild(LabelLongitude);
            this.AddChild(LabelPosStatus);
            this.AddChild(LabelTimestamp);
            this.AddChild(LabelAltitude);
            this.AddChild(LabelHeading);
            this.AddChild(LabelAccuracy);
            this.AddChild(LabelLastLatitude);
            this.AddChild(LabelLastLongitude);
            this.AddChild(LabelLastTimestamp);
            this.AddChild(LabelLastAltitude);

            this.Schedule(SetGeolocation);
           
        }

        protected override void AddedToScene()
        {
            base.AddedToScene();
           
            LabelLatitude.PositionX = this.VisibleBoundsWorldspace.MinX + 20;
            LabelLatitude.PositionY = this.VisibleBoundsWorldspace.MaxY - 20;
            LabelLatitude.AnchorPoint = CCPoint.AnchorUpperLeft;

            LabelLongitude.PositionX = VisibleBoundsWorldspace.MinX + 20;
            LabelLongitude.PositionY = VisibleBoundsWorldspace.MaxY - 50;
            LabelLongitude.AnchorPoint = CCPoint.AnchorUpperLeft;

            LabelPosStatus.PositionX = VisibleBoundsWorldspace.MinX + 20;
            LabelPosStatus.PositionY = VisibleBoundsWorldspace.MaxY - 80;
            LabelPosStatus.AnchorPoint = CCPoint.AnchorUpperLeft;

            LabelTimestamp.PositionX = VisibleBoundsWorldspace.MinX + 20;
            LabelTimestamp.PositionY = VisibleBoundsWorldspace.MaxY - 110;
            LabelTimestamp.AnchorPoint = CCPoint.AnchorUpperLeft;

            LabelAltitude.PositionX = VisibleBoundsWorldspace.MinX + 20;
            LabelAltitude.PositionY = VisibleBoundsWorldspace.MaxY - 140;
            LabelAltitude.AnchorPoint = CCPoint.AnchorUpperLeft;

            LabelHeading.PositionX = VisibleBoundsWorldspace.MinX + 20;
            LabelHeading.PositionY = VisibleBoundsWorldspace.MaxY - 170;
            LabelHeading.AnchorPoint = CCPoint.AnchorUpperLeft;

            LabelAccuracy.PositionX = VisibleBoundsWorldspace.MinX + 20;
            LabelAccuracy.PositionY = VisibleBoundsWorldspace.MaxY - 200;
            LabelAccuracy.AnchorPoint = CCPoint.AnchorUpperLeft;

            LabelLastLatitude.PositionX = VisibleBoundsWorldspace.MinX + 20;
            LabelLastLatitude.PositionY = VisibleBoundsWorldspace.MaxY - 230;
            LabelLastLatitude.AnchorPoint = CCPoint.AnchorUpperLeft;

            LabelLastLongitude.PositionX = VisibleBoundsWorldspace.MinX + 20;
            LabelLastLongitude.PositionY = VisibleBoundsWorldspace.MaxY - 260;
            LabelLastLongitude.AnchorPoint = CCPoint.AnchorUpperLeft;

            LabelLastAltitude.PositionX = VisibleBoundsWorldspace.MinX + 20;
            LabelLastAltitude.PositionY = VisibleBoundsWorldspace.MaxY - 290;
            LabelLastAltitude.AnchorPoint = CCPoint.AnchorUpperLeft;

            LabelLastTimestamp.PositionX = VisibleBoundsWorldspace.MinX + 20;
            LabelLastTimestamp.PositionY = VisibleBoundsWorldspace.MaxY - 320;
            LabelLastTimestamp.AnchorPoint = CCPoint.AnchorUpperLeft;
        }

        void SetGeolocation(float FrameTimesInSecond)
        {

            LabelLatitude.Text = Latitude + " = " + mGeolocation.Latitude;
            LabelLongitude.Text = Longitude + " = " + mGeolocation.Longitude;
            LabelPosStatus.Text = PositionStatus + " = " + mGeolocation.Status;
            LabelTimestamp.Text = Timestamp + " = " + mGeolocation.TimeStamp;
            LabelAltitude.Text = Altitude + " = " + mGeolocation.Altitude;
            LabelHeading.Text = Heading + " = " + mGeolocation.Heading;
            LabelAccuracy.Text = Accuracy + " = " + mGeolocation.Accuracy;

            LabelLastLatitude.Text = Latitude + " = " + mGeolocation.LastPosition.Latitude;
            LabelLastLongitude.Text = Longitude + " = " + mGeolocation.LastPosition.Longitude;
            LabelLastTimestamp.Text = Timestamp + " = " + mGeolocation.LastPosition.Timestamp;
            LabelLastAltitude.Text = Altitude + " = " + mGeolocation.LastPosition.Altitude;
          
        }

    }
}


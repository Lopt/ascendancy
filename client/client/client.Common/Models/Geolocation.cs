using System;
using XLabs.Forms.Mvvm;
using Xamarin.Forms;
using SQLite;
using XLabs.Platform.Services.Geolocation;
using Xamarin.Forms.Xaml;
using System.Threading.Tasks;
using System.Threading;
using XLabs.Platform.Device;



namespace Client.Common.Models
{
    /// <summary>
    /// The Geolocation as a singleton class for geolocation information.
    /// </summary>
    [Table("Geolocation")]
    public sealed class Geolocation : ViewBaseModel
    {
        #region Singelton

        /// <summary>
        /// The lazy.
        /// </summary>
        private static readonly Lazy<Geolocation> lazy =
            new Lazy<Geolocation>(() => new Geolocation());

        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <value>The instance.</value>
        public static Geolocation Instance { get { return lazy.Value; } }

        /// <summary>
        /// Initializes a new instance of the <see cref="Client.Common.Models.Geolocation"/> class.
        /// </summary>
        private Geolocation()
        {
            m_geolocator = DependencyService.Get<IGeolocator>();
            m_geolocator.DesiredAccuracy = 1.0;
            m_geolocator.PositionChanged += OnPositionChanged;

            m_tokensource = new CancellationTokenSource();
            m_scheduler = TaskScheduler.FromCurrentSynchronizationContext();

            Position position = new Position();
            position.Latitude = 50.98520741; // standardPosition
            position.Longitude = 11.04233265; // standardPosition
            CurrentPosition = position;
            LastPosition = new Position();

            IsBusy = false;
            IsPositionChanged = true;
        }


        #endregion

        #region Geolocator

        /// <summary>
        /// The m_geolocator.
        /// </summary>
        private readonly IGeolocator m_geolocator = null;
        /// <summary>
        /// The m_tokensource.
        /// </summary>
        private CancellationTokenSource m_tokensource = null;
        /// <summary>
        /// The m_scheduler.
        /// </summary>
        private readonly TaskScheduler m_scheduler = null;

        /// <summary>
        /// Gets a value indicating whether this instance is busy.
        /// </summary>
        /// <value><c>true</c> if this instance is busy; otherwise, <c>false</c>.</value>
        public bool IsBusy
        { 
            get;
            private set; 
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is position changed.
        /// </summary>
        /// <value><c>true</c> if this instance is position changed; otherwise, <c>false</c>.</value>
        public bool IsPositionChanged
        {
            get; 
            set; 
        }

        /// <summary>
        /// Gets a value indicating whether this instance is geolocation available.
        /// </summary>
        /// <value><c>true</c> if this instance is geolocation available; otherwise, <c>false</c>.</value>
        public bool IsGeolocationAvailable
        { 
            get
            {
                return m_geolocator.IsGeolocationAvailable; 
            } 
        }

        /// <summary>
        /// Gets a value indicating whether this instance is geolocation enabled.
        /// </summary>
        /// <value><c>true</c> if this instance is geolocation enabled; otherwise, <c>false</c>.</value>
        public bool IsGeolocationEnabled
        { 
            get
            {
                return m_geolocator.IsGeolocationEnabled; 
            } 
        }

        /// <summary>
        /// Gets a value indicating whether this instance is geolocation listening.
        /// </summary>
        /// <value><c>true</c> if this instance is geolocation listening; otherwise, <c>false</c>.</value>
        public bool IsGeolocationListening
        { 
            get
            { 
                return m_geolocator.IsListening; 
            } 
        }

        /// <summary>
        /// Starts the listening.
        /// </summary>
        /// <param name="minTimeIntervallInMilliSec">Minimum time intervall in milli sec.</param>
        /// <param name="minDistance">Minimum distance.</param>
        public void StartListening(uint minTimeIntervallInMilliSec, double minDistance)
        {
            m_geolocator.StartListening(minTimeIntervallInMilliSec, minDistance);
        }

        /// <summary>
        /// Stops the listening.
        /// </summary>
        public void StopListening()
        {
            m_geolocator.StopListening();
        }

        /// <summary>
        /// Raises the position changed event.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
        private async void OnPositionChanged(object sender, PositionEventArgs e)
        {
            await GetPosition();
            IsPositionChanged = true;
        }

        /// <summary>
        /// Gets the position.
        /// </summary>
        /// <returns>The position.</returns>
        private async Task GetPosition()
        {  
            if (m_geolocator == null)
                return;
            IsBusy = true;

            await m_geolocator.GetPositionAsync(timeout: 4000, cancelToken: m_tokensource.Token, includeHeading: true)
                .ContinueWith(t =>
                {
                    IsBusy = false;
                    if (t.IsFaulted)
                        Status = ((GeolocationException)t.Exception.InnerException).Error.ToString();
                    else if (t.IsCanceled)
                        Status = "Canceled";
                    else
                    {
                        Status = "ok";
                        CurrentPosition = t.Result as Position;
//                    if (CurrentPosition.Latitude < 10 && CurrentPosition.Longitude < 10)
                            
                    }

                }, m_scheduler);
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
        /// Gets or sets the current position.
        /// </summary>
        /// <value>The current position.</value>
        [Column("CurrentPosition")]
        public Position CurrentPosition
        { 
            get
            {
                return m_currentPosition; 
            }
            set
            {
                SetProperty(m_currentPosition, value, (val) =>
                    {
                        LastPosition = m_currentPosition;
                        m_currentPosition = val;
                    }, PropertyNameCurrentPosition);

                Latitude = m_currentPosition.Latitude.ToString("N4");
                Longitude = m_currentPosition.Longitude.ToString("N4");
                Altitude = m_currentPosition.Altitude.ToString();
                TimeStamp = m_currentPosition.Timestamp.ToString("G");
                Heading = m_currentPosition.Heading.ToString();
                Accuracy = m_currentPosition.Accuracy.ToString();
                CurrentGamePosition = new Core.Models.Position(new Core.Models.LatLon(m_currentPosition.Latitude, m_currentPosition.Longitude));
                StringGamePosition = string.Format("PosX = {0}, PosY = {1}", CurrentGamePosition.X, CurrentGamePosition.Y);
                CurrentRegionPosition = new Core.Models.RegionPosition(CurrentGamePosition);
                CurrentCellPosition = new Core.Models.CellPosition(CurrentGamePosition);

            }
        }

        /// <summary>
        /// The property name current position.
        /// </summary>
        public static string PropertyNameCurrentPosition = "CurrentPosition";
        /// <summary>
        /// The m_current position.
        /// </summary>
        private Position m_currentPosition;


        /// <summary>
        /// Gets or sets the current game position.
        /// </summary>
        /// <value>The current game position.</value>
        [Column("CurrentGamePosition")]
        public Core.Models.Position CurrentGamePosition
        { 
            get
            {
                return m_currentGamePosition; 
            }
            set
            {
                SetProperty(m_currentGamePosition, value, (val) =>
                    {
                        m_currentGamePosition = val;

                    }, PropertyNameCurrentGamePosition);
            }
        }

        /// <summary>
        /// The property name current game position.
        /// </summary>
        public static string PropertyNameCurrentGamePosition = "CurrentGamePosition";
        /// <summary>
        /// The m_current game position.
        /// </summary>
        private Core.Models.Position m_currentGamePosition;


        /// <summary>
        /// Gets or sets the current region position.
        /// </summary>
        /// <value>The current region position.</value>
        [Column("CurrentRegionPosition")]
        public Core.Models.RegionPosition CurrentRegionPosition
        { 
            get
            {
                return m_currentRegionPosition; 
            }
            set
            {
                SetProperty(m_currentRegionPosition, value, (val) =>
                    {
                        m_currentRegionPosition = val;

                    }, PropertyNameCurrentRegionPosition);
            }
        }

        /// <summary>
        /// The property name current region position.
        /// </summary>
        public static string PropertyNameCurrentRegionPosition = "CurrentRegionPosition";
        /// <summary>
        /// The m_current region position.
        /// </summary>
        private Core.Models.RegionPosition m_currentRegionPosition;

        /// <summary>
        /// Gets or sets the current cell position.
        /// </summary>
        /// <value>The current cell position.</value>
        [Column("CurrentCellPosition")]
        public Core.Models.CellPosition CurrentCellPosition
        { 
            get
            { 
                return m_currentCellPosition; 
            }
            set
            {
                SetProperty(m_currentCellPosition, value, (val) =>
                    {
                        m_currentCellPosition = val;

                    }, PropertyNameCurrentCellPosition);
            }
        }

        /// <summary>
        /// The property name current cell position.
        /// </summary>
        public static string PropertyNameCurrentCellPosition = "CurrentCellPosition";
        /// <summary>
        /// The m_current cell position.
        /// </summary>
        private Core.Models.CellPosition m_currentCellPosition;

        /// <summary>
        /// Gets or sets the last position.
        /// </summary>
        /// <value>The last position.</value>
        [Column("LastPosition")]
        public Position LastPosition
        { 
            get
            {
                return m_lastPosition; 
            }
            set
            {
                SetProperty(m_lastPosition, value, (val) =>
                    {
                        m_lastPosition = val;
                        LastGamePosition = new Core.Models.Position(new Core.Models.LatLon(val.Latitude, val.Longitude));
                        LastRegionPosition = new Core.Models.RegionPosition(LastGamePosition);
                        LastCellPosition = new Core.Models.CellPosition(LastGamePosition);

                    }, PropertyNameLastPosition);
            }
        }

        /// <summary>
        /// The property name last position.
        /// </summary>
        public static string PropertyNameLastPosition = "LastPosition";
        /// <summary>
        /// The m_last position.
        /// </summary>
        private Position m_lastPosition;

        /// <summary>
        /// Gets or sets the last game position.
        /// </summary>
        /// <value>The last game position.</value>
        [Column("LastGamePosition")]
        public Core.Models.Position LastGamePosition
        { 
            get
            { 
                return m_lastGamePosition; 
            }
            set
            {
                SetProperty(m_lastGamePosition, value, (val) =>
                    {
                        m_lastGamePosition = val;

                    }, PropertyNameLastGamePosition);
            }
        }

        /// <summary>
        /// The property name last game position.
        /// </summary>
        public static string PropertyNameLastGamePosition = "LastGamePosition";
        /// <summary>
        /// The m_last game position.
        /// </summary>
        private Core.Models.Position m_lastGamePosition;


        /// <summary>
        /// Gets or sets the last region position.
        /// </summary>
        /// <value>The last region position.</value>
        [Column("LastRegionPosition")]
        public Core.Models.RegionPosition LastRegionPosition
        { 
            get
            { 
                return m_lastRegionPosition; 
            }
            set
            {
                SetProperty(m_lastRegionPosition, value, (val) =>
                    {
                        m_lastRegionPosition = val;

                    }, PropertyNameLastRegionPosition);
            }
        }

        /// <summary>
        /// The property name last region position.
        /// </summary>
        public static string PropertyNameLastRegionPosition = "LastRegionPosition";
        /// <summary>
        /// The m_last region position.
        /// </summary>
        private Core.Models.RegionPosition m_lastRegionPosition;

        /// <summary>
        /// Gets or sets the last cell position.
        /// </summary>
        /// <value>The last cell position.</value>
        [Column("LastCellPosition")]
        public Core.Models.CellPosition LastCellPosition
        { 
            get
            { 
                return m_lastCellPosition;
            }
            set
            {
                SetProperty(m_lastCellPosition, value, (val) =>
                    {
                        m_lastCellPosition = val;

                    }, PropertyNameLastCellPosition);
            }
        }

        /// <summary>
        /// The property name last cell position.
        /// </summary>
        public static string PropertyNameLastCellPosition = "LastCellPosition";
        /// <summary>
        /// The m_last cell position.
        /// </summary>
        private Core.Models.CellPosition m_lastCellPosition;


        /// <summary>
        /// Gets or sets the latitude.
        /// </summary>
        /// <value>The latitude.</value>
        [Column("Latitude")]
        public string Latitude
        { 
            get
            { 
                return m_latitude; 
            }
            set
            {
                SetProperty(m_latitude, value, (val) =>
                    {
                        m_latitude = val;

                    }, PropertyNameLatitude);
            }
        }

        /// <summary>
        /// The property name latitude.
        /// </summary>
        public static string PropertyNameLatitude = "Latitude";
        /// <summary>
        /// The m_latitude.
        /// </summary>
        private string m_latitude;


        /// <summary>
        /// Gets or sets the longitude.
        /// </summary>
        /// <value>The longitude.</value>
        [Column("Longitude")]
        public string Longitude
        { 
            get
            {
                return m_longitude; 
            }
            set
            {
                SetProperty(m_longitude, value, (val) =>
                    {
                        m_longitude = val;

                    }, PropertyNameLongitude);
            }
        }

        /// <summary>
        /// The property name longitude.
        /// </summary>
        public static string PropertyNameLongitude = "Longitude";
        /// <summary>
        /// The m_longitude.
        /// </summary>
        private string m_longitude;


        /// <summary>
        /// Gets or sets the altitude.
        /// </summary>
        /// <value>The altitude.</value>
        [Column("Altitude")]
        public string Altitude
        { 
            get
            {
                return m_altitude;
            }
            set
            {
                SetProperty(m_altitude, value, (val) =>
                    {
                        m_altitude = val;

                    }, PropertyNameAltitude);
            }
        }

        /// <summary>
        /// The property name string game position.
        /// </summary>
        public static string PropertyNameStringGamePosition = "StringGamePosition";
        /// <summary>
        /// The m_string game position.
        /// </summary>
        private string m_stringGamePosition;

        /// <summary>
        /// Gets or sets the string game position.
        /// </summary>
        /// <value>The string game position.</value>
        [Column("StringGamePosition")]
        public string StringGamePosition
        { 
            get
            {
                return m_stringGamePosition; 
            }
            set
            {
                SetProperty(m_stringGamePosition, value, (val) =>
                    {
                        m_stringGamePosition = val;

                    }, PropertyNameStringGamePosition);
            }
        }

        /// <summary>
        /// The property name altitude.
        /// </summary>
        public static string PropertyNameAltitude = "Altitude";
        /// <summary>
        /// The m_altitude.
        /// </summary>
        private string m_altitude;


        /// <summary>
        /// Gets or sets the time stamp.
        /// </summary>
        /// <value>The time stamp.</value>
        [Column("TimeStamp")]
        public string TimeStamp
        { 
            get
            {
                return m_timeStamp;
            }
            set
            {
                SetProperty(m_timeStamp, value, (val) =>
                    {
                        m_timeStamp = val;

                    }, PropertyNameTimeStamp);
            }
        }

        /// <summary>
        /// The property name time stamp.
        /// </summary>
        public static string PropertyNameTimeStamp = "TimeStamp";
        /// <summary>
        /// The m time stamp.
        /// </summary>
        private string m_timeStamp;



        /// <summary>
        /// Gets or sets the heading in dergees relative to the north.
        /// </summary>
        /// <value>The heading.</value>
        [Column("Heading")]
        public string Heading
        { 
            get
            { 
                return m_heading;
            }
            set
            {
                SetProperty(m_heading, value, (val) =>
                    {
                        m_heading = val;

                    }, PropertyNameHeading);
            }
        }

        /// <summary>
        /// The property name heading.
        /// </summary>
        public static string PropertyNameHeading = "Heading";
        /// <summary>
        /// The m_heading.
        /// </summary>
        private string m_heading;



        /// <summary>
        /// Gets or sets the accuracy.The potential position error radius in meters
        /// </summary>
        /// <value>The accuracy.</value>
        [Column("Accuracy")]
        public string Accuracy
        { 
            get
            { 
                return m_accuracy;
            }
            set
            {
                SetProperty(m_accuracy, value, (val) =>
                    {
                        m_accuracy = val;

                    }, PropertyNameAccuracy);
            }
        }

        /// <summary>
        /// The property name accuracy.
        /// </summary>
        public static string PropertyNameAccuracy = "Accuracy";
        /// <summary>
        /// The m_accuracy.
        /// </summary>
        private string m_accuracy;


        /// <summary>
        /// Gets or sets the status.
        /// </summary>
        /// <value>The status.</value>
        [Column("Status")]
        public string Status
        { 
            get
            { 
                return m_status;
            }
            set
            {
                SetProperty(m_status, value, (val) =>
                    {
                        m_status = val;

                    }, PropertyNameStatus);
            }
        }

        /// <summary>
        /// The property name status.
        /// </summary>
        public static string PropertyNameStatus = "Status";
        /// <summary>
        /// The m_status.
        /// </summary>
        private string m_status;

        #endregion
    }
        
}
    
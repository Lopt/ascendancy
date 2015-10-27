namespace Client.Common.Models
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using SQLite;
    using Xamarin.Forms;
    using Xamarin.Forms.Xaml;
    using XLabs.Forms.Mvvm;
    using XLabs.Platform.Device;
    using XLabs.Platform.Services.Geolocation;

    /// <summary>
    /// The Geolocation as a singleton class for geolocation information.
    /// </summary>
    [Table("Geolocation")]
    public sealed class Geolocation
    {
        #region Singleton

        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <value>The instance.</value>
        public static Geolocation Instance
        { 
            get
            { 
                return Singleton.Value; 
            }
        }

        /// <summary>
        /// Prevents a default instance of the <see cref="Geolocation"/> class from being created.
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

            IsPositionChanged = true;
        }

        /// <summary>
        /// The Sinleton Instance.
        /// </summary>
        private static readonly Lazy<Geolocation> Singleton =
            new Lazy<Geolocation>(() => new Geolocation());

        #endregion

        #region Devicegeolocator

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
        /// <param name="sender">Sender is the event source.</param>
        /// <param name="e">E the event data. If there is no event data, this parameter will be null.</param>
        private async void OnPositionChanged(object sender, PositionEventArgs e)
        {
            await GetPosition();
            IsPositionChanged = true;
            LastPosition = CurrentPosition;
        }

        /// <summary>
        /// Gets the position.
        /// </summary>
        /// <returns>The position.</returns>
        private async Task GetPosition()
        {  
            if (m_geolocator == null)
            {
                return;
            }

            await m_geolocator.GetPositionAsync(timeout: 4000, cancelToken: m_tokensource.Token, includeHeading: true).ContinueWith(
                t =>
                {
                    if (t.IsFaulted)
                    {
                        Helper.Logging.Error(((GeolocationException)t.Exception.InnerException).Error.ToString());
                    }
                    else if (t.IsCanceled)
                    {
                        Helper.Logging.Warning("Geolocation get position async is Canceled");
                    }
                    else
                    {
                        CurrentPosition = t.Result as Position;                
                    }
                },
                m_scheduler);
        }

        #endregion

        #region ViewPoperties

        /// <summary>
        /// Gets the current position.
        /// </summary>
        /// <value>The current position.</value>
        public Position CurrentPosition
        {
            get;

            private set;
        }

        /// <summary>
        /// Gets the current game position.
        /// </summary>
        /// <value>The current game position.</value>
        public Core.Models.Position CurrentGamePosition
        { 
            get
            {
                return new Core.Models.Position(new Core.Models.LatLon(CurrentPosition.Latitude, CurrentPosition.Longitude)); 
            }
        }

        /// <summary>
        /// Gets the current region position.
        /// </summary>
        /// <value>The current region position.</value>
        public Core.Models.RegionPosition CurrentRegionPosition
        { 
            get
            {
                return new Core.Models.RegionPosition(CurrentGamePosition); 
            }
        }

        /// <summary>
        /// Gets the current cell position.
        /// </summary>
        /// <value>The current cell position.</value>
        public Core.Models.CellPosition CurrentCellPosition
        { 
            get
            { 
                return new Core.Models.CellPosition(CurrentGamePosition); 
            }
        }

        /// <summary>
        /// Gets the last position.
        /// </summary>
        /// <value>The last position.</value>
        public Position LastPosition
        { 
            get;

            private set;
        }

        /// <summary>
        /// Gets the last game position.
        /// </summary>
        /// <value>The last game position.</value>
        public Core.Models.Position LastGamePosition
        { 
            get
            { 
                return new Core.Models.Position(new Core.Models.LatLon(LastPosition.Latitude, LastPosition.Longitude)); 
            }
        }

        /// <summary>
        /// Gets the last region position.
        /// </summary>
        /// <value>The last region position.</value>
        public Core.Models.RegionPosition LastRegionPosition
        { 
            get
            {
                return new Core.Models.RegionPosition(LastGamePosition); 
            }
        }

        /// <summary>
        /// Gets the last cell position.
        /// </summary>
        /// <value>The last cell position.</value>
        public Core.Models.CellPosition LastCellPosition
        { 
            get
            { 
                return new Core.Models.CellPosition(LastGamePosition); 
            }
        }

        #endregion
    }
}   
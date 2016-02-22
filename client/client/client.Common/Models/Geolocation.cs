namespace Client.Common.Models
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Core.Models;
    using SQLite;
    using Xamarin.Forms;
    using Xamarin.Forms.Xaml;
    using XLabs.Forms.Mvvm;
    using XLabs.Platform.Device;
    using XLabs.Platform.Services.Geolocation;

    /// <summary>
    /// The Geo location as a singleton class for geo location information.
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
            m_geolocator = XLabs.Ioc.Resolver.Resolve<IGeolocator>();
            m_geolocator.DesiredAccuracy = 1.0;
            m_geolocator.PositionChanged += OnPositionChanged;
           
            CurrentGamePosition = null;
            FirstGamePosition = null;
        }

        /// <summary>
        /// The Singleton Instance.
        /// </summary>
        private static readonly Lazy<Geolocation> Singleton =
            new Lazy<Geolocation>(() => new Geolocation());

        #endregion

        #region Devicegeolocator

        /// <summary>
        /// The geo locator.
        /// </summary>
        private readonly IGeolocator m_geolocator = null;

        /// <summary>
        /// Gets a value indicating whether this instance is geo location available.
        /// </summary>
        /// <value><c>true</c> if this instance is geo location available; otherwise, <c>false</c>.</value>
        public bool IsGeolocationAvailable
        { 
            get
            {
                return m_geolocator.IsGeolocationAvailable; 
            } 
        }

        /// <summary>
        /// Gets a value indicating whether this instance is geo location enabled.
        /// </summary>
        /// <value><c>true</c> if this instance is geo location enabled; otherwise, <c>false</c>.</value>
        public bool IsGeolocationEnabled
        { 
            get
            {
                return m_geolocator.IsGeolocationEnabled; 
            } 
        }

        /// <summary>
        /// Gets a value indicating whether this instance is geo location listening.
        /// </summary>
        /// <value><c>true</c> if this instance is geo location listening; otherwise, <c>false</c>.</value>
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
        /// <param name="minTimeIntervallInMilliSec">Minimum time interval in milliseconds.</param>
        /// <param name="minDistance">Minimum distance.</param>
        public void StartListening(uint minTimeIntervallInMilliSec, double minDistance)
        {
            try
            {
                m_geolocator.StartListening(minTimeIntervallInMilliSec, minDistance);
            }
            catch (Exception ex)
            {
                var text = ex.Message;
            }
        }

        /// <summary>
        /// Stops the listening.
        /// </summary>
        public void StopListening()
        {
            try
            {
                m_geolocator.StopListening();
            }
            catch (Exception ex)
            {
                var text = ex.Message;
            }
        }

        /// <summary>
        /// Gets the position async.
        /// </summary>
        /// <returns>The position async.</returns>
        public async Task<Core.Models.Position> GetPositionAsync()
        {
            Core.Models.Position position = null;
            try
            {
                var latlon = await m_geolocator.GetPositionAsync(Client.Common.Constants.ClientConstants.GPS_GET_POSITION_TIMEOUT);
                position = new Core.Models.Position(latlon.Latitude, latlon.Longitude);
            }
            catch (Exception ex)
            {
                var text = ex.Message;
            }

            return position;
        }

        /// <summary>
        /// Raises the position changed event.
        /// </summary>
        /// <param name="sender">Sender is the event source.</param>
        /// <param name="eventPos">E the event data. If there is no event data, this parameter will be null.</param>
        private async void OnPositionChanged(object sender, PositionEventArgs eventPos)
        {
            try
            {
                CurrentGamePosition = new Core.Models.Position(new Core.Models.LatLon(eventPos.Position.Latitude, eventPos.Position.Longitude));
                if (FirstGamePosition == null)
                {
                    FirstGamePosition = CurrentGamePosition;
                }
            }
            catch (Exception ex)
            {
                var text = ex.Message;
            }
        }

        #endregion

        #region ViewPoperties

        /// <summary>
        /// Gets the current position.
        /// </summary>
        /// <value>The current position.</value>
        public Core.Models.Position CurrentGamePosition
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the first game position.
        /// </summary>
        /// <value>The first game position.</value>
        public Core.Models.Position FirstGamePosition
        {
            get;
            private set;
        }

        #endregion
    }
}   
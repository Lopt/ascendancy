using System;
using XLabs.Forms.Mvvm;
using client.Common.model;
using Xamarin.Forms;
using SQLite;
using XLabs.Platform.Services.Geolocation;
using Xamarin.Forms.Xaml;
using System.Threading.Tasks;
using System.Threading;
using XLabs.Platform.Device;


namespace client.Common
{
	[Table ("Geolocation")]
	public sealed class Geolocation : viewBaseModel
	{
		#region Singelton

		private static readonly Geolocation m_instance = new Geolocation ();

		private Geolocation ()
		{
			m_geolocator = DependencyService.Get<IGeolocator> ();
			m_geolocator.DesiredAccuracy = 1.0;
			m_geolocator.PositionChanged += OnPositionChanged;

			m_tokensource = new CancellationTokenSource ();
			m_scheduler = TaskScheduler.FromCurrentSynchronizationContext ();

			CurrentPosition = new Position ();
			LastPosition = new Position ();

			IsBusy = false;
			IsPositionChanged = false;
		}

		public static Geolocation GetInstance{ get { return m_instance; } }

		#endregion

		#region Geolocator

		private readonly IGeolocator m_geolocator = null;
		private CancellationTokenSource m_tokensource = null;
		private readonly TaskScheduler m_scheduler = null;

		public bool IsBusy{ get; private set; }

		public bool IsPositionChanged{ get; set; }

		public bool IsGeolocationAvailable{ get { return m_geolocator.IsGeolocationAvailable; } }

		public bool IsGeolocationEnabled{ get { return m_geolocator.IsGeolocationEnabled; } }

		public bool IsGeolocationListening{ get { return m_geolocator.IsListening; } }

		public void StartListening (uint _MinTimeIntervallInMilliSec, double _MinDistance)
		{
			m_geolocator.StartListening (_MinTimeIntervallInMilliSec, _MinDistance);
		}

		public void StopListening ()
		{
			m_geolocator.StopListening ();
		}

		private async void OnPositionChanged (object sender, PositionEventArgs e)
		{
			await GetPosition ();
			IsPositionChanged = true;
		}

		private async Task GetPosition ()
		{  
			if (m_geolocator == null)
				return;
			IsBusy = true;
			await m_geolocator.GetPositionAsync (timeout: 4000, cancelToken: m_tokensource.Token, includeHeading: true)
                .ContinueWith (t => {
				IsBusy = false;
				if (t.IsFaulted)
					Status = ((GeolocationException)t.Exception.InnerException).Error.ToString ();
				else if (t.IsCanceled)
					Status = "Canceled";
				else {
					Status = "ok";
					CurrentPosition = t.Result;
				}

			}, m_scheduler);
		}

		#endregion

		#region ViewPoperties

		[PrimaryKey, AutoIncrement]
		public int Id { get; set; }

		[Column ("CurrentPosition")]
		public Position CurrentPosition { 
			get { return m_currentPosition; }
			set {
				SetProperty (m_currentPosition, value, (val) => {
					LastPosition = m_currentPosition;
					m_currentPosition = val;
				}, PropertyNameCurrentPosition);

				Latitude = m_currentPosition.Latitude.ToString ("N4");
				Longitude = m_currentPosition.Longitude.ToString ("N4");
				Altitude = m_currentPosition.Altitude.ToString ();
				TimeStamp = m_currentPosition.Timestamp.ToString ("G");
				Heading = m_currentPosition.Heading.ToString ();
				Accuracy = m_currentPosition.Accuracy.ToString ();
				CurrentGamePosition = new @base.model.Position (new @base.model.LatLon (m_currentPosition.Latitude, m_currentPosition.Longitude));
				StringGamePosition = string.Format ("PosX = {0}, PosY = {1}", CurrentGamePosition.X, CurrentGamePosition.Y);
				CurrentRegionPosition = new @base.model.RegionPosition (CurrentGamePosition);
				CurrentCellPosition = new @base.model.CellPosition (CurrentGamePosition);

			}
		}

		public static string PropertyNameCurrentPosition = "CurrentPosition";
		private Position m_currentPosition;


		[Column ("CurrentGamePosition")]
		public @base.model.Position CurrentGamePosition { 
			get { return m_currentGamePosition; }
			set {
				SetProperty (m_currentGamePosition, value, (val) => {
					m_currentGamePosition = val;
				}, PropertyNameCurrentGamePosition);
			}
		}

		public static string PropertyNameCurrentGamePosition = "CurrentGamePosition";
		private @base.model.Position m_currentGamePosition;


		[Column ("CurrentRegionPosition")]
		public @base.model.RegionPosition CurrentRegionPosition { 
			get { return m_currentRegionPosition; }
			set {
				SetProperty (m_currentRegionPosition, value, (val) => {
					m_currentRegionPosition = val;
				}, PropertyNameCurrentRegionPosition);
			}
		}

		public static string PropertyNameCurrentRegionPosition = "CurrentRegionPosition";
		private @base.model.RegionPosition m_currentRegionPosition;


		[Column ("CurrentCellPosition")]
		public @base.model.CellPosition CurrentCellPosition { 
			get { return m_currentCellPosition; }
			set {
				SetProperty (m_currentCellPosition, value, (val) => {
					m_currentCellPosition = val;
				}, PropertyNameCurrentCellPosition);
			}
		}

		public static string PropertyNameCurrentCellPosition = "CurrentCellPosition";
		private @base.model.CellPosition m_currentCellPosition;


		[Column ("LastPosition")]
		public Position LastPosition { 
			get { return m_lastPosition; }
			set {
				SetProperty (m_lastPosition, value, (val) => {
					m_lastPosition = val;
					LastGamePosition = new @base.model.Position (new @base.model.LatLon (val.Latitude, val.Longitude));
					LastRegionPosition = new @base.model.RegionPosition (LastGamePosition);
					LastCellPosition = new @base.model.CellPosition (LastGamePosition);
				}, PropertyNameLastPosition);
			}
		}

		public static string PropertyNameLastPosition = "LastPosition";
		private Position m_lastPosition;


		[Column ("LastGamePosition")]
		public @base.model.Position LastGamePosition { 
			get { return m_lastGamePosition; }
			set {
				SetProperty (m_lastGamePosition, value, (val) => {
					m_lastGamePosition = val;
				}, PropertyNameLastGamePosition);
			}
		}

		public static string PropertyNameLastGamePosition = "LastGamePosition";
		private @base.model.Position m_lastGamePosition;


		[Column ("LastRegionPosition")]
		public @base.model.RegionPosition LastRegionPosition { 
			get { return m_lastRegionPosition; }
			set {
				SetProperty (m_lastRegionPosition, value, (val) => {
					m_lastRegionPosition = val;
				}, PropertyNameLastRegionPosition);
			}
		}

		public static string PropertyNameLastRegionPosition = "LastRegionPosition";
		private @base.model.RegionPosition m_lastRegionPosition;


		[Column ("LastCellPosition")]
		public @base.model.CellPosition LastCellPosition { 
			get { return m_lastCellPosition; }
			set {
				SetProperty (m_lastCellPosition, value, (val) => {
					m_lastCellPosition = val;
				}, PropertyNameLastCellPosition);
			}
		}

		public static string PropertyNameLastCellPosition = "LastCellPosition";
		private @base.model.CellPosition m_lastCellPosition;


		[Column ("Latitude")]
		public string Latitude { 
			get { return m_latitude; }
			set {
				SetProperty (m_latitude, value, (val) => {
					m_latitude = val;
				}, PropertyNameLatitude);
			}
		}

		public static string PropertyNameLatitude = "Latitude";
		private string m_latitude;


		[Column ("Longitude")]
		public string Longitude { 
			get { return m_longitude; }
			set {
				SetProperty (m_longitude, value, (val) => {
					m_longitude = val;
				}, PropertyNameLongitude);
			}
		}

		public static string PropertyNameLongitude = "Longitude";
		private string m_longitude;


		[Column ("Altitude")]
		public string Altitude { 
			get { return m_altitude; }
			set {
				SetProperty (m_altitude, value, (val) => {
					m_altitude = val;
				}, PropertyNameAltitude);
			}
		}

		public static string PropertyNameStringGamePosition = "StringGamePosition";
		private string m_stringGamePosition;


		[Column ("StringGamePosition")]
		public string StringGamePosition { 
			get { return m_stringGamePosition; }
			set {
				SetProperty (m_stringGamePosition, value, (val) => {
					m_stringGamePosition = val;
				}, PropertyNameStringGamePosition);
			}
		}

		public static string PropertyNameAltitude = "Altitude";
		private string m_altitude;


		[Column ("TimeStamp")]
		public string TimeStamp { 
			get { return m_timeStamp; }
			set {
				SetProperty (m_timeStamp, value, (val) => {
					m_timeStamp = val;
				}, PropertyNameTimeStamp);
			}
		}

		public static string PropertyNameTimeStamp = "TimeStamp";
		private string m_timeStamp;


		//Heading in dergees relative to the north
		[Column ("Heading")]
		public string Heading { 
			get { return m_heading; }
			set {
				SetProperty (m_heading, value, (val) => {
					m_heading = val;
				}, PropertyNameHeading);
			}
		}

		public static string PropertyNameHeading = "Heading";
		private string m_heading;


		//the potential position error radius in meters
		[Column ("Accuracy")]
		public string Accuracy { 
			get { return m_accuracy; }
			set {
				SetProperty (m_accuracy, value, (val) => {
					m_accuracy = val;
				}, PropertyNameAccuracy);
			}
		}

		public static string PropertyNameAccuracy = "Accuracy";
		private string m_accuracy;


		[Column ("Status")]
		public string Status { 
			get { return m_status; }
			set {
				SetProperty (m_status, value, (val) => {
					m_status = val;
				}, PropertyNameStatus);
			}
		}

		public static string PropertyNameStatus = "Status";
		private string m_status;

		#endregion
	}
        
}
    
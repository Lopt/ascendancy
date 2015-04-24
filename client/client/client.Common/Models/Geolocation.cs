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

		private static readonly Geolocation _instance = new Geolocation ();

		private Geolocation ()
		{
			_geolocator = DependencyService.Get<IGeolocator> ();
			_geolocator.DesiredAccuracy = 1.0;
			_geolocator.PositionChanged += OnPositionChanged;

			_tokensource = new CancellationTokenSource ();
			_scheduler = TaskScheduler.FromCurrentSynchronizationContext ();

			CurrentPosition = new Position ();
			LastPosition = new Position ();

			IsBusy = false;
			IsPositionChanged = false;
		}

		public static Geolocation GetInstance{ get { return _instance; } }

		#endregion

		#region Geolocator

		private readonly IGeolocator _geolocator = null;
		private CancellationTokenSource _tokensource = null;
		private readonly TaskScheduler _scheduler = null;

		public bool IsBusy{ get; private set; }

		public bool IsPositionChanged{ get; private set; }

		public bool IsGeolocationAvailable{ get { return _geolocator.IsGeolocationAvailable; } }

		public bool IsGeolocationEnabled{ get { return _geolocator.IsGeolocationEnabled; } }

		public bool IsGeolocationListening{ get { return _geolocator.IsListening; } }

		public void StartListening (uint _MinTimeIntervallInMilliSec, double _MinDistance)
		{
			_geolocator.StartListening (_MinTimeIntervallInMilliSec, _MinDistance);
		}

		public void StopListening ()
		{
			_geolocator.StopListening ();
		}

		private async void OnPositionChanged (object sender, PositionEventArgs e)
		{
			await GetPosition ();
			IsPositionChanged = true;
		}

		private async Task GetPosition ()
		{  
			if (_geolocator == null)
				return;
			IsBusy = true;
			await _geolocator.GetPositionAsync (timeout: 4000, cancelToken: _tokensource.Token, includeHeading: true)
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

			}, _scheduler);
		}

		#endregion

		#region ViewPoperties

		[PrimaryKey, AutoIncrement]
		public int Id { get; set; }

		[Column ("CurrentPosition")]
		public Position CurrentPosition { 
			get { return _currentPosition; }
			set {
				SetProperty (_currentPosition, value, (val) => {
					LastPosition = _currentPosition;
					_currentPosition = val;
				}, PropertyNameCurrentPosition);

				Latitude = _currentPosition.Latitude.ToString ("N4");
				Longitude = _currentPosition.Longitude.ToString ("N4");
				Altitude = _currentPosition.Altitude.ToString ();
				TimeStamp = _currentPosition.Timestamp.ToString ("G");
				Heading = _currentPosition.Heading.ToString ();
				Accuracy = _currentPosition.Accuracy.ToString ();
				CurrentGamePosition = new @base.model.Position (new @base.model.LatLon (_currentPosition.Latitude, _currentPosition.Longitude));
				StringGamePosition = string.Format ("PosX = {0}, PosY = {1}", CurrentGamePosition.X, CurrentGamePosition.Y);

			}
		}

		public static string PropertyNameCurrentPosition = "CurrentPosition";
		private Position _currentPosition;


		[Column ("CurrentGamePosition")]
		public @base.model.Position CurrentGamePosition { 
			get { return _currentGamePosition; }
			set {
				SetProperty (_currentGamePosition, value, (val) => {
					_currentGamePosition = val;
				}, PropertyNameCurrentGamePosition);
			}
		}

		public static string PropertyNameCurrentGamePosition = "CurrentGamePosition";
		private @base.model.Position _currentGamePosition;


		[Column ("LastPosition")]
		public Position LastPosition { 
			get { return _lastPosition; }
			set {
				SetProperty (_lastPosition, value, (val) => {
					_lastPosition = val;
					LastGamePosition = new @base.model.Position (new @base.model.LatLon (val.Latitude, val.Longitude));
				}, PropertyNameLastPosition);
			}
		}

		public static string PropertyNameLastPosition = "LastPosition";
		private Position _lastPosition;


		[Column ("LastGamePosition")]
		public @base.model.Position LastGamePosition { 
			get { return _lastGamePosition; }
			set {
				SetProperty (_lastGamePosition, value, (val) => {
					_lastGamePosition = val;
				}, PropertyNameLastGamePosition);
			}
		}

		public static string PropertyNameLastGamePosition = "LastGamePosition";
		private @base.model.Position _lastGamePosition;


		[Column ("Latitude")]
		public string Latitude { 
			get { return _latitude; }
			set {
				SetProperty (_latitude, value, (val) => {
					_latitude = val;
				}, PropertyNameLatitude);
			}
		}

		public static string PropertyNameLatitude = "Latitude";
		private string _latitude;

		[Column ("Longitude")]
		public string Longitude { 
			get { return _longitude; }
			set {
				SetProperty (_longitude, value, (val) => {
					_longitude = val;
				}, PropertyNameLongitude);
			}
		}

		public static string PropertyNameLongitude = "Longitude";
		private string _longitude;


		[Column ("Altitude")]
		public string Altitude { 
			get { return _altitude; }
			set {
				SetProperty (_altitude, value, (val) => {
					_altitude = val;
				}, PropertyNameAltitude);
			}
		}

		public static string PropertyNameStringGamePosition = "StringGamePosition";
		private string _stringGamePosition;


		[Column ("StringGamePosition")]
		public string StringGamePosition { 
			get { return _stringGamePosition; }
			set {
				SetProperty (_stringGamePosition, value, (val) => {
					_stringGamePosition = val;
				}, PropertyNameStringGamePosition);
			}
		}

		public static string PropertyNameAltitude = "Altitude";
		private string _altitude;

		[Column ("TimeStamp")]
		public string TimeStamp { 
			get { return _timeStamp; }
			set {
				SetProperty (_timeStamp, value, (val) => {
					_timeStamp = val;
				}, PropertyNameTimeStamp);
			}
		}

		public static string PropertyNameTimeStamp = "TimeStamp";
		private string _timeStamp;

		//Heading in dergees relative to the north
		[Column ("Heading")]
		public string Heading { 
			get { return _heading; }
			set {
				SetProperty (_heading, value, (val) => {
					_heading = val;
				}, PropertyNameHeading);
			}
		}

		public static string PropertyNameHeading = "Heading";
		private string _heading;

		//the potential position error radius in meters
		[Column ("Accuracy")]
		public string Accuracy { 
			get { return _accuracy; }
			set {
				SetProperty (_accuracy, value, (val) => {
					_accuracy = val;
				}, PropertyNameAccuracy);
			}
		}

		public static string PropertyNameAccuracy = "Accuracy";
		private string _accuracy;

		[Column ("Status")]
		public string Status { 
			get { return _status; }
			set {
				SetProperty (_status, value, (val) => {
					_status = val;
				}, PropertyNameStatus);
			}
		}

		public static string PropertyNameStatus = "Status";
		private string _status;

		#endregion
	}
        
}
    
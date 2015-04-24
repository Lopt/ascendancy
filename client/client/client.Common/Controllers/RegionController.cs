using System;
using @base.model;
using @base.control;
using client.Common.helper;

namespace client.Common.Controllers
{
	public sealed class RegionController
	{
		#region Singelton

		private static readonly RegionController _instance = new RegionController ();

		private RegionController ()
		{
			_network = NetworkController.GetInstance;
			_geolocation = Geolocation.GetInstance;
			_regionManager = new @base.model.RegionManager ();
			_controlRegionManager	= new @base.control.RegionManager ();

		}

		public static RegionController GetInstance{ get { return _instance; } }

		#endregion

		#region Region

		public Region LoadRegion ()
		{
			var geolocationPosition = _geolocation.CurrentPosition;

			return LoadRegion (geolocationPosition.Latitude, geolocationPosition.Longitude);
		}

		public Region LoadRegion (double _Latitude, double _Longitude)
		{
			LatLon latLon	= new LatLon (_Latitude, _Longitude);
			return LoadRegion (latLon);
		}

		public Region LoadRegion (LatLon _latlon)
		{
			Position gameWorldPosition = new Position (_latlon);
			return LoadRegion (gameWorldPosition);
		}

		public Region LoadRegion (Position _gameWorldPosition)
		{
			RegionPosition regionPosition	= new RegionPosition (_gameWorldPosition);

			return LoadRegion (regionPosition);
		}

		public Region LoadRegion (RegionPosition _regionPosition)
		{
			string path = _controlRegionManager.ReplacePath (ClientConstants.REGIONSERVERPATH, _regionPosition);

			_network.LoadTerrainsAsync (path);

			return _controlRegionManager.JsonToRegion (_network.JsonTerrainsString, _regionPosition);
		}

		public void AddRegion (Region _region)
		{
			_regionManager.AddRegion (_region);
		}

		public Region GetRegion (double _latitude, double _longitude)
		{
			return GetRegion (new LatLon (_latitude, _longitude));
		}

		public Region GetRegion (LatLon _latlon)
		{
			return GetRegion (new Position (_latlon));
		}

		public Region GetRegion (Position _gameWorldPosition)
		{
			return GetRegion (new RegionPosition (_gameWorldPosition));
		}

		public Region GetRegion (RegionPosition _regionPosition)
		{
			return _regionManager.GetRegion (_regionPosition);
		}

		#endregion

		#region private Fields

		private NetworkController _network;
		private Geolocation _geolocation;
		private @base.model.RegionManager _regionManager;
		private @base.control.RegionManager _controlRegionManager;

		#endregion
	}
}


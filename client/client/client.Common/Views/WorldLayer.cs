using System;
using CocosSharp;
using client.Common.Controllers;
using @base.control;
using System.Collections.Generic;
using client.Common.Models;
using client.Common.helper;
using @base.model;
using client.Common.Helper;
using System.ComponentModel.DataAnnotations;
using Microsoft.Xna.Framework;


namespace client.Common.Views
{
	public class WorldLayer : CCLayerColor
	{
		public WorldLayer (RegionPosition regionPosition)
			: base ()
		{
			m_RegContr = Controller.Instance.RegionManagerController as RegionController;

			m_WorldTileMap = new CCTileMap (ClientConstants.TILEMAP_FILE);
			m_Geolocation = Geolocation.GetInstance;

			m_CurrentPosition = new DrawNode ();
			m_WorldTileMap.TileLayersContainer.AddChild (m_CurrentPosition);

			m_Terrainlayer = m_WorldTileMap.LayerNamed (ClientConstants.LAYER_TERRAIN);

			SetRegionsOnce (m_Geolocation.CurrentRegionPosition);
			SetCurrentPositionOnce (m_Geolocation.CurrentGamePosition);

			this.AddChild (m_WorldTileMap);

			this.Schedule (CheckGeolocation);


			var TouchListener = new CCEventListenerTouchAllAtOnce ();
			TouchListener.OnTouchesMoved = onTouchesMoved;
			TouchListener.OnTouchesBegan = onTouchesBegan;
			TouchListener.OnTouchesEnded = onTouchesEnded;


			this.AddEventListener (TouchListener);
		}

		#region overide

		protected override void AddedToScene ()
		{
			base.AddedToScene ();

			var TileCoordinate = m_RegContr.GetCurrentTileInMap (m_Geolocation.CurrentGamePosition);
			var mapcell = Modify.TilePosToMapCellPos (TileCoordinate);
			var anchor = Modify.TilePosToMapPoint (TileCoordinate);
			m_WorldTileMap.TileLayersContainer.AnchorPoint = anchor;
			m_WorldTileMap.TileLayersContainer.PositionX = VisibleBoundsWorldspace.MidX;
			m_WorldTileMap.TileLayersContainer.PositionY = VisibleBoundsWorldspace.MidY;
			m_WorldTileMap.TileLayersContainer.Scale = 0.125f;
			m_WorldTileMap.TileLayersContainer.AnchorPoint = new CCPoint (0.5f, 0.25f);

		}

		#endregion

		#region Listener

		void onTouchesMoved (List<CCTouch> touches, CCEvent touchEvent)
		{
			var touch = touches [0];
			CCPoint diff = touch.Delta;
			m_WorldTileMap.TileLayersContainer.Position += diff;

		}

		void onTouchesBegan (List<CCTouch> touches, CCEvent touchEvent)
		{

		}

		void onTouchesEnded (List<CCTouch> touches, CCEvent touchEvent)
		{

		}


		#endregion

		#region Scheduling

		void SetEntitys (float FrameTimesInSecond)
		{
			//TODO 
			throw new NotImplementedException ();
		}


		void CheckGeolocation (float FrameTimesInSecond)
		{
			if (m_Geolocation.IsPositionChanged) {
				SetRegionsOnce (m_Geolocation.CurrentRegionPosition);
				SetCurrentPositionOnce (m_Geolocation.CurrentGamePosition);
				counter++;
			}
//			if (GameAppDelegate.m_Loading >= GameAppDelegate.Loading.RegionLoaded) {
//				m_Geolocation.IsPositionChanged = false;
//			}

		}

		#endregion

		#region

		void SetRegionsOnce (RegionPosition regionPosition)
		{		
			m_RegContr.LoadRegions (regionPosition);
			m_RegContr.SetTilesINMap160 (m_Terrainlayer, m_RegContr.GetRegion (regionPosition));
		}

		void SetCurrentPositionOnce (Position position)
		{
			var TileCoordinate = m_RegContr.GetCurrentTileInMap (position);
			m_CurrentPosition.DrawHexagonForIsoStagMap (ClientConstants.TILE_IMAGE_WIDTH, m_Terrainlayer,
				TileCoordinate, new CCColor4F (CCColor3B.Red), 255, 3.0f);
		}

		#endregion

		#region Properties

		CCTileMap m_WorldTileMap;
		CCTileMapLayer m_Terrainlayer;

		RegionController m_RegContr;

		DrawNode m_CurrentPosition;
		Geolocation m_Geolocation;

		float m_Scale = 1.0f;
		int counter = 0;

		#endregion
	}
}


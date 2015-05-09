using System;
using CocosSharp;
using client.Common.Controllers;
using @base.control;
using System.Collections.Generic;
using client.Common.Models;
using client.Common.helper;
using @base.model;
using client.Common.Helper;


namespace client.Common.Views
{
	public class WorldLayer : CCLayerColor
	{
		public WorldLayer (RegionPosition RegionPosition)
			: base ()
		{
			m_RegContr = Controller.Instance.RegionManagerController as RegionController;
			m_MiddleRegionPosition = RegionPosition;
			m_TopMiddleRegionPosition = new RegionPosition (m_MiddleRegionPosition.RegionX, m_MiddleRegionPosition.RegionY + 1);

			m_RegContr.GetRegion (m_MiddleRegionPosition);
			m_RegContr.GetRegion (m_TopMiddleRegionPosition);

			m_WorldTileMap = new CCTileMap (ClientConstants.TILEMAP_FILE);

			m_MapIsChanged = true;
			m_Geolocation = Geolocation.GetInstance;
			m_CurrentPosition = new DrawNode ();
			m_Terrainlayer = m_WorldTileMap.LayerNamed (ClientConstants.LAYER_TERRAIN);

			m_WorldTileMap.TileLayersContainer.AddChild (m_CurrentPosition);

			this.AddChild (m_WorldTileMap);

			this.Schedule (SetRegions);
			this.Schedule (CheckGeolocation);
			this.Schedule (SetCurrentPosition);

			var MoveWorldListener = new CCEventListenerTouchAllAtOnce ();
			MoveWorldListener.OnTouchesMoved = OnMoveWorld;


			this.AddEventListener (MoveWorldListener);

		}

		#region overide

		protected override void AddedToScene ()
		{
			base.AddedToScene ();

			m_WorldTileMap.TileLayersContainer.PositionX = VisibleBoundsWorldspace.MaxX / 2;
			m_WorldTileMap.TileLayersContainer.PositionY = VisibleBoundsWorldspace.MaxY / 2;
			m_WorldTileMap.TileLayersContainer.AnchorPoint = new CCPoint (0.5f, 0.25f);

		}

		#endregion

		#region Listener

		void OnMoveWorld (List<CCTouch> touches, CCEvent touchEvent)
		{
			var touch = touches [0];
			CCPoint diff = touch.Delta;
			m_WorldTileMap.TileLayersContainer.Position += diff;
		}

		#endregion

		#region Scheduling

		void SetRegions (float FrameTimesInSecond)
		{
			if (m_RegContr.GetRegion (m_MiddleRegionPosition).Exist && m_MapIsChanged) {
				m_RegContr.SetTilesInMap (m_Terrainlayer, new CCTileMapCoordinates (64, 64), m_RegContr.GetRegion (m_MiddleRegionPosition));
				m_RegContr.SetTilesInMap (m_Terrainlayer, new CCTileMapCoordinates (64, 32), m_RegContr.GetRegion (m_TopMiddleRegionPosition));
				m_MapIsChanged = false;
			}

		}

		void SetEntitys (float FrameTimesInSecond)
		{
			//TODO 
			throw new NotImplementedException ();
		}

		void SetCurrentPosition (float FrameTimesInSecond)
		{
			if (m_Geolocation.IsPositionChanged && m_Geolocation.CurrentRegionPosition.Equals (m_MiddleRegionPosition)) {
				var CellPos = m_Geolocation.CurrentCellPosition;
				var TileCoordinate = Modify.MapCellPosToTilePos (CellPos.CellX + 64, CellPos.CellY + 64);
				m_CurrentPosition.DrawHexagonForIsoStagMap (ClientConstants.TILE_IMAGE_Width, m_Terrainlayer,
					TileCoordinate, new CCColor4F (CCColor3B.Red), 255, 3.0f);
			}

		}

		void CheckGeolocation (float FrameTimesInSecond)
		{
			m_MapIsChanged = m_Geolocation.IsPositionChanged;
		}

		#endregion

		#region Properties

		CCTileMap m_WorldTileMap;
		CCTileMapLayer m_Terrainlayer;

		RegionController m_RegContr;
		RegionPosition m_MiddleRegionPosition;
		RegionPosition m_TopMiddleRegionPosition;

		bool m_MapIsChanged;
		DrawNode m_CurrentPosition;
		Geolocation m_Geolocation;

		#endregion
	}
}


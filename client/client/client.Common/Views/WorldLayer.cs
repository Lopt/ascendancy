using System;
using CocosSharp;
using client.Common.Controllers;
using @base.control;
using System.Collections.Generic;
using client.Common.Models;
using @base.model;
using client.Common.Helper;
using System.ComponentModel.DataAnnotations;
using Microsoft.Xna.Framework;
using System.Threading.Tasks;


namespace client.Common.Views
{
    public class WorldLayer : CCLayerColor
    {
        public WorldLayer (RegionPosition regionPosition)
            : base ()
        {
            m_RegionC = Controller.Instance.RegionStatesController.Curr as RegionController;

            m_WorldTileMap = new CCTileMap (ClientConstants.TILEMAP_FILE);
            m_Geolocation = Geolocation.GetInstance;

            m_CurrentPosition = new DrawNode ();
            m_WorldTileMap.TileLayersContainer.AddChild (m_CurrentPosition);

            m_TerrainLayer = m_WorldTileMap.LayerNamed (ClientConstants.LAYER_TERRAIN);

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

            var tileCoordinate = m_RegionC.GetCurrentTileInMap (m_Geolocation.CurrentGamePosition);
            var mapCellPosition = new MapCellPosition (tileCoordinate);
            var anchor = mapCellPosition.GetMapPoint ();
//			m_WorldTileMap.PositionX = 0;
//			m_WorldTileMap.PositionY = 0;

//			var point = new CCPoint3 (anchor.X * m_WorldTileMap.TileLayersContainer.ContentSize.Width + VisibleBoundsWorldspace.MidX, anchor.Y * m_WorldTileMap.TileLayersContainer.ContentSize.Height + VisibleBoundsWorldspace.MidY, Camera.CenterInWorldspace.Z);
//			this.Camera.TargetInWorldspace = point;

            // m_WorldTileMap.TileLayersContainer.AnchorPoint = anchor;
            m_WorldTileMap.TileLayersContainer.PositionX = VisibleBoundsWorldspace.MidX;
            m_WorldTileMap.TileLayersContainer.PositionY = VisibleBoundsWorldspace.MidY;
            m_WorldTileMap.TileLayersContainer.Scale = 0.2f;
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

        void SetEntitys (float frameTimesInSecond)
        {
            //TODO 
            throw new NotImplementedException ();
        }


        void CheckGeolocation (float frameTimesInSecond)
        {
            if (m_Geolocation.IsPositionChanged) {
                DrawRegions (m_Geolocation.CurrentRegionPosition);
                m_Geolocation.IsPositionChanged = false;
            }

        }

        #endregion

        #region


        void SetCurrentPositionOnce (Position position)
        {
            var tileCoordinate = m_RegionC.GetCurrentTileInMap (position);
            m_CurrentPosition.DrawHexagonForIsoStagMap (ClientConstants.TILE_IMAGE_WIDTH, m_TerrainLayer,
                tileCoordinate, new CCColor4F (CCColor3B.Red), 255, 3.0f);
        }

        async Task DrawRegions (RegionPosition regionPosition)
        {
            GameAppDelegate.LoadingState = GameAppDelegate.Loading.RegionLoading;
            await m_RegionC.LoadRegionsAsync ();
            GameAppDelegate.LoadingState = GameAppDelegate.Loading.RegionLoaded;
            m_RegionC.SetTilesINMap160 (m_TerrainLayer, m_RegionC.GetRegion (m_Geolocation.CurrentRegionPosition));
            SetCurrentPositionOnce (m_Geolocation.CurrentGamePosition);
        }

        #endregion

        #region Properties

        CCTileMap m_WorldTileMap;
        CCTileMapLayer m_TerrainLayer;

        RegionController m_RegionC;

        DrawNode m_CurrentPosition;
        Geolocation m_Geolocation;

        float m_Scale = 1.0f;
        int counter = 0;

        #endregion
    }
}


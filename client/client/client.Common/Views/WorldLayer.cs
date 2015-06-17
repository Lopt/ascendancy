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

            m_CurrentPositionNode = new DrawNode ();
            m_WorldTileMap.TileLayersContainer.AddChild (m_CurrentPositionNode);

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

            //SetMapAnchor (m_Geolocation.CurrentGamePosition);
            //m_WorldTileMap.TileLayersContainer.PositionX = VisibleBoundsWorldspace.MinX;
            //m_WorldTileMap.TileLayersContainer.PositionY = VisibleBoundsWorldspace.MinY;
            m_WorldTileMap.TileLayersContainer.Scale = m_Scale;

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
            var touch = touches [0];
            var touchfirst = touch.StartLocation;
            var diff = touch.Delta;
            CheckCenterRegion (touch.Location);
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
                DrawRegions (m_Geolocation.CurrentGamePosition);
                m_Geolocation.IsPositionChanged = false;
            }

        }

        #endregion

        #region


        void SetCurrentPositionOnce (Position position)
        {
            var tileCoordinate = m_RegionC.GetCurrentTileInMap (position);
            m_CurrentPositionNode.DrawHexagonForIsoStagMap (ClientConstants.TILE_IMAGE_WIDTH, m_TerrainLayer,
                tileCoordinate, new CCColor4F (CCColor3B.Red), 255, 3.0f);
        }

        async Task DrawRegions (Position gamePosition)
        {
            GameAppDelegate.LoadingState = GameAppDelegate.Loading.RegionLoading;
            await m_RegionC.LoadRegionsAsync ();
            GameAppDelegate.LoadingState = GameAppDelegate.Loading.RegionLoaded;
            m_RegionC.SetTilesINMap160 (m_TerrainLayer, m_RegionC.GetRegionByGamePosition (gamePosition));
            // SetMapAnchor (gamePosition);
            SetCurrentPositionOnce (gamePosition);
            m_CenterRegionPosition = new RegionPosition (gamePosition);

        }

        void CheckCenterRegion (CCPoint location)
        {            
            var mapCell = GetMapCell (m_WorldTileMap.LayerNamed ("Layer 0"), location);

//            if (m_RegionC.IsCellInOutsideRegion (mapCell)) {
//                var position = m_RegionC.GetCurrentGamePosition (mapCell, m_CenterRegionPosition);
//                DrawRegions (position);
//            }

        }

        void SetMapAnchor (Position currentPosition)
        {
            var tileCoordinate = m_RegionC.GetCurrentTileInMap (currentPosition);
            var mapCellPosition = new MapCellPosition (tileCoordinate);
            SetMapAnchor (mapCellPosition.GetAnchor ());
        }

        void SetMapAnchor (CCPoint anchorPoint)
        {
            m_WorldTileMap.TileLayersContainer.AnchorPoint = anchorPoint;
        }

        MapCellPosition GetMapCell (CCTileMapLayer layer, CCPoint location)
        {
  
            var anchor = this.Parent.AnchorPoint;
             
            var position = new CCPoint (anchor.X * m_WorldTileMap.TileLayersContainer.ContentSize.Width - VisibleBoundsWorldspace.MidX + location.X * m_Scale, 
                               (anchor.Y * m_WorldTileMap.TileLayersContainer.ContentSize.Height - VisibleBoundsWorldspace.MidY + location.Y * m_Scale));

            var tileMapCooardinate = layer.ClosestTileCoordAtNodePosition (position);
            var erg = new MapCellPosition (tileMapCooardinate);
            return erg;
        }


        #endregion

        #region Properties

        CCTileMap m_WorldTileMap;
        CCTileMapLayer m_TerrainLayer;

        RegionController m_RegionC;
        RegionPosition m_CenterRegionPosition;

        DrawNode m_CurrentPositionNode;
        Geolocation m_Geolocation;

        float m_Scale = 0.10f;
        int counter = 0;

        #endregion
    }
}


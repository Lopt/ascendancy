using System;
using System.Diagnostics;
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
            //m_PointedPositionNode = new DrawNode ();
            m_WorldTileMap.TileLayersContainer.AddChild (m_CurrentPositionNode);
            //m_WorldTileMap.TileLayersContainer.AddChild (m_PointedPositionNode);

            m_TerrainLayer = m_WorldTileMap.LayerNamed (ClientConstants.LAYER_TERRAIN);
            //m_BuildingLayer   = m_WorldTileMap.LayerNamed (ClientConstants.LAYER_BUILDING);
            //m_UnitLayer       = m_WorldTileMap.LayerNamed (ClientConstants.LAYER_UNIT);
            //m_MenuLayer       = m_WorldTileMap.LayerNamed (ClientConstants.LAYER_MENU);

            this.AddChild (m_WorldTileMap);

            this.Schedule (CheckGeolocation);

            m_Timer = new Stopwatch ();

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

            SetMapAnchor (m_Geolocation.CurrentGamePosition);
            m_WorldTileMap.TileLayersContainer.PositionX = VisibleBoundsWorldspace.MidX;
            m_WorldTileMap.TileLayersContainer.PositionY = VisibleBoundsWorldspace.MidY;
            m_WorldTileMap.TileLayersContainer.Scale = 0.5f;

        }

        #endregion

        #region Listener

        void onTouchesMoved (List<CCTouch> touches, CCEvent touchEvent)
        {
            if (touches.Count == 1) {
                var touch = touches [0];
                CCPoint diff = touch.Delta;
                m_WorldTileMap.TileLayersContainer.Position += diff;
            }
            if (touches.Count >= 2) {
                //do zoom stuff

                //get StartPositons
                /*
                CCPoint StartPoint0 = touches[0].StartLocation;
                CCPoint StartPoint1 = touches[1].StartLocation;
                float xDist = (StartPoint1.X - StartPoint0.X);
                float yDist = (StartPoint1.Y - StartPoint0.Y);
                float StartDistance = (xDist * xDist) + (yDist * yDist);


                CCPoint CurrentPoint0 = touches[0].Location;
                CCPoint CurrentPoint1 = touches[1].Location;

                //calculate distance 
                xDist = (CurrentPoint1.X - CurrentPoint0.X);
                yDist = (CurrentPoint1.Y - CurrentPoint0.Y);
                float CurrentDistance = (xDist * xDist) + (yDist * yDist);


                */


                CCPoint ScreenStart0 = touches[0].StartLocationOnScreen;
                CCPoint ScreenStart1 = touches[1].StartLocationOnScreen;

                float xDist = (ScreenStart1.X - ScreenStart0.X);
                float yDist = (ScreenStart1.Y - ScreenStart0.Y);
                float StartDistance = (xDist * xDist) + (yDist * yDist);

                //calculate Start distance



                //Get Current Position

                CCPoint CurrentPoint0 = touches[0].LocationOnScreen;
                CCPoint CurrentPoint1 = touches[1].LocationOnScreen;

                //calculate distance 
                xDist = (CurrentPoint1.X - CurrentPoint0.X);
                yDist = (CurrentPoint1.Y - CurrentPoint0.Y);
                float CurrentDistance = (xDist * xDist) + (yDist * yDist);

                xDist = VisibleBoundsWorldspace.MaxX;
                yDist = VisibleBoundsWorldspace.MaxY;

                float ScreenDistance = (xDist * xDist) + (yDist * yDist);

                float relation = (CurrentDistance - StartDistance)/ ScreenDistance;

                var newScale = m_Scale + relation;
                if (0.3f < newScale && newScale < 2.0f)
                {
                    m_newScale = newScale;
                    m_WorldTileMap.TileLayersContainer.Scale = m_newScale;
                }

                /*
                if ( StartDistance > CurrentDistance && m_Scale > 0.3f)
                {
                    //zoom--
                    m_Scale += relation;
                    m_WorldTileMap.TileLayersContainer.Scale = m_Scale;

                }
                if (StartDistance < CurrentDistance && m_Scale < 2.0f)
                {
                    //Zoom++
                    m_Scale += relation;
                    m_WorldTileMap.TileLayersContainer.Scale = m_Scale;

                }
                */



            }


        }

        void onTouchesBegan (List<CCTouch> touches, CCEvent touchEvent)
        {
            m_Timer.Reset ();
            m_Timer.Start ();
        }

        void onTouchesEnded (List<CCTouch> touches, CCEvent touchEvent)
        {
            m_Scale = m_newScale;

            m_Timer.Stop ();
            if (m_Timer.ElapsedMilliseconds > 2000) {
                //get Touch
                var touch = touches [0];
                //get selected Tile
                //var location = m_TerrainLayer.WorldToParentspace (touch.Location);
                //var tileCoordinate = m_TerrainLayer.ClosestTileCoordAtNodePosition (location);
                 
                /*
                if(unit selected/unit move command){
                    //layer auf NULL checken
                    if( m_BuildingLayer == m_UnitLayer == NULL){
                        //move
                    }
                    
                    if( m_UnitLayer == TRUE ){
                        //Check if your own Unit
                        if(Unit.ownerid != userid){
                            //combat Menu
                        }else{
                            //move invalid
                        }
                    }
                    
                    if ( m_BuildingLayer == TRUE){
                        //check Building owner
                        if ( building.ownerid != userid ){
                            //Conquest Menu
                        }
                    }
                }
                */

                /*
                if (m_MenuLayer != NULL){

                    switch(MenuItems){
                    case unit_action1:
                        //do Stuff
                    break;
                    case building_action2:
                        //do Stuff
                    break;
                    }
                
                }
                */

                /*
                if (m_UnitLayer != NULL)
                {

                    //UnitMenu
                    DrawMenu();

                }
                */

                /*
                if (m_BuildingLayer != NULL){

                    //BuildingMenu
                
                }
                */
            }

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
            SetMapAnchor (gamePosition);
            SetCurrentPositionOnce (gamePosition);
        }

        void SetMapAnchor (Position currentPosition)
        {
            var tileCoordinate = m_RegionC.GetCurrentTileInMap (currentPosition);
            var mapCellPosition = new MapCellPosition (tileCoordinate);
            m_WorldTileMap.TileLayersContainer.AnchorPoint = mapCellPosition.GetAnchor ();
        }

        #endregion

        #region Properties

        CCTileMap m_WorldTileMap;
        CCTileMapLayer m_TerrainLayer;
        //CCTileMapLayer m_BuildingLayer;
        //CCTileMapLayer m_UnitLayer;
        //CCTileMapLayer m_MenuLayer;

        RegionController m_RegionC;

        DrawNode m_CurrentPositionNode;
        Geolocation m_Geolocation;

        float m_newScale = 0.5f;
        float m_Scale = 0.5f;
        int counter = 0;

        Stopwatch m_Timer;

        #endregion
    }
}


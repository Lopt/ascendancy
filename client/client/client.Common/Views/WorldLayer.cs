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
            m_WorldTileMap.TileLayersContainer.AddChild (m_CurrentPositionNode);

            m_TerrainLayer = m_WorldTileMap.LayerNamed (ClientConstants.LAYER_TERRAIN);
            m_BuildingLayer   = m_WorldTileMap.LayerNamed (ClientConstants.LAYER_BUILDING);
            m_UnitLayer       = m_WorldTileMap.LayerNamed (ClientConstants.LAYER_UNIT);
            m_MenuLayer       = m_WorldTileMap.LayerNamed (ClientConstants.LAYER_MENU);

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
              
                //calculate Start distance
                CCPoint ScreenStart0 = touches[0].StartLocationOnScreen;
                CCPoint ScreenStart1 = touches[1].StartLocationOnScreen;

                float xDist = (ScreenStart1.X - ScreenStart0.X);
                float yDist = (ScreenStart1.Y - ScreenStart0.Y);
                float StartDistance = (xDist * xDist) + (yDist * yDist);

                //calculate Current Position
                CCPoint CurrentPoint0 = touches[0].LocationOnScreen;
                CCPoint CurrentPoint1 = touches[1].LocationOnScreen;

                xDist = (CurrentPoint1.X - CurrentPoint0.X);
                yDist = (CurrentPoint1.Y - CurrentPoint0.Y);
                float CurrentDistance = (xDist * xDist) + (yDist * yDist);

                //calculate screen relation 
                xDist = VisibleBoundsWorldspace.MaxX;
                yDist = VisibleBoundsWorldspace.MaxY;

                float ScreenDistance = (xDist * xDist) + (yDist * yDist);
                float relation = (CurrentDistance - StartDistance)/ ScreenDistance;

                //scale
                var newScale = m_Scale + relation;
                if (0.3f < newScale && newScale < 2.0f)
                {
                    m_newScale = newScale;
                    m_WorldTileMap.TileLayersContainer.Scale = m_newScale;
                }
            }
        }

        void onTouchesBegan (List<CCTouch> touches, CCEvent touchEvent)
        {
            m_Timer.Reset ();
            m_Timer.Start ();
        }

        void onTouchesEnded (List<CCTouch> touches, CCEvent touchEvent)
        {
            //Set Current Scale
            m_Scale = m_newScale;

            //Stop Timer for Longtap
            m_Timer.Stop ();
            //get Touch location and Corresponding TilePosition
            var touch = touches [0];

            var location = m_TerrainLayer.WorldToParentspace(touch.Location);
            var tileCoordinate = m_TerrainLayer.ClosestTileCoordAtNodePosition (location);

            //Menu Handling
            /*
            if(m_MenuLayer.TileGIDandFlags(tileCoordinate) != 0)
            {
                Command = GetMenuCommandfromDefinition(Position);
                SendCommandToServer(Command);
                break ???
            }
            */

            //Movement Handling 
            /*
            if(MoveUI == true)
            {
                //do Movementstuff
                if (GetTileState(Position) == true)
                {
                    SendMoveToServer(UnitID,Position)
                }
            }
            */

            //LongTap Handling 
            if (m_Timer.ElapsedMilliseconds > 400 && m_Timer.ElapsedMilliseconds < 2000) {
                
                //Draw the taped ISO Tile
                //m_CurrentPositionNode.DrawISOForIsoStagMap(131, m_UnitLayer,tileCoordinate, new CCColor4F (CCColor3B.Blue), 255, 3.0f);
                if (MenuDrawn == true) 
                {
                    DrawMenu (LastCoord, 0);
                }
                DrawMenu(tileCoordinate, 1);
                //TODO find a way to update the menu layer
                //m_MenuLayer.Update (0.0f);

                //Check if something exists @ this position for Unit or Building
                /*
                if(m_UnitLayer.TileGIDandFlags(tileCoordinate) != 0)
                {
                    
                    UnitID = GetUnitID(Position);
                    if(isBuilder(UnitID) == true)
                    {
                        DrawMenu(Position, /Builder/);
                    }
                    else
                    {
                        AvailableMovement = GetUnitMovement(UnitID);
                        Move(AvailableMovement, UnitID);
                        MoveUI = TRUE;
                    }
                }
                else if(m_BuildingLayer.TileGIDandFlags(tileCoordinate) != 0)
                {
                    BuildingID = GetBuildingID(Position);
                    if(isFactory(BuildingID) == true)
                    {
                        DrawMenu(Position, /Factory/);
                    }
                }
                */
                 

                /*
                if (m_UnitLayer.TileGIDandFlags(tileCoordinate) != NULL)
                {

                    //UnitMenu
                    DrawMenu();

                }
                */

                /*
                if (m_BuildingLayer.TileGIDandFlags(tileCoordinate) != 0){

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

        //clears a Layer
        void ClearLayer (CCTileMapLayer _Layer)
        {
            GIDHelper1.Gid = 0;
            for (int i = 0; i < ClientConstants.TILEMAP_WIDTH; i++) 
            {
                CoordHelper1.Column = i;
                for (int j = 0; j < ClientConstants.TILEMAP_HIGH; j++) 
                {
                    CoordHelper1.Row = j;
                    _Layer.SetTileGID (GIDHelper1, CoordHelper1);
                }
            }
        }

        void DrawMenu(CCTileMapCoordinates _location, int _menutype)
        {
            CoordHelper1.Column = _location.Column + (_location.Row) % 2;
            CoordHelper1.Row    = _location.Row - 1;

            CoordHelper2.Column = _location.Column + (_location.Row) % 2;
            CoordHelper2.Row    = _location.Row + 1;        

            CoordHelper3.Column = _location.Column;
            CoordHelper3.Row    = _location.Row + 2;

            CoordHelper4.Column = _location.Column - (_location.Row+1) % 2;
            CoordHelper4.Row    = _location.Row + 1;

            CoordHelper5.Column = _location.Column - (_location.Row+1) % 2;
            CoordHelper5.Row    = _location.Row - 1;

            CoordHelper6.Column = _location.Column;
            CoordHelper6.Row    = _location.Row - 2;

            GIDHelper1.Gid = 52;
            GIDHelper2.Gid = 53;
            GIDHelper3.Gid = 54;
            GIDHelper4.Gid = 55;
            GIDHelper5.Gid = 56;
            GIDHelper6.Gid = 57;
            switch (_menutype) 
            {
            //clears the Menu at around a given Position
            case 0:
                GIDHelper1.Gid = 0;
                m_MenuLayer.SetTileGID (GIDHelper1, CoordHelper1);
                m_MenuLayer.SetTileGID (GIDHelper1, CoordHelper2);
                m_MenuLayer.SetTileGID (GIDHelper1, CoordHelper3);
                m_MenuLayer.SetTileGID (GIDHelper1, CoordHelper4);
                m_MenuLayer.SetTileGID (GIDHelper1, CoordHelper5);
                m_MenuLayer.SetTileGID (GIDHelper1, CoordHelper6);
                break;
            default:
                m_MenuLayer.SetTileGID (GIDHelper1, CoordHelper1);
                m_MenuLayer.SetTileGID (GIDHelper2, CoordHelper2);
                m_MenuLayer.SetTileGID (GIDHelper3, CoordHelper3);
                m_MenuLayer.SetTileGID (GIDHelper4, CoordHelper4);
                m_MenuLayer.SetTileGID (GIDHelper5, CoordHelper5);
                m_MenuLayer.SetTileGID (GIDHelper6, CoordHelper6);
                LastCoord = _location;
                MenuDrawn = true;
                break;
            }
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
        CCTileMapLayer m_BuildingLayer;
        CCTileMapLayer m_UnitLayer;
        CCTileMapLayer m_MenuLayer;

        RegionController m_RegionC;

        DrawNode m_CurrentPositionNode;
        Geolocation m_Geolocation;

        float m_newScale = 0.5f;
        float m_Scale = 0.5f;
        int counter = 0;
        bool UnitMove = false;
        bool MenuDrawn = false;


        CCTileGidAndFlags GIDHelper1, GIDHelper2, GIDHelper3, GIDHelper4, GIDHelper5, GIDHelper6;
        CCTileMapCoordinates CoordHelper1, CoordHelper2, CoordHelper3, CoordHelper4, CoordHelper5, CoordHelper6, LastCoord;

        Stopwatch m_Timer;

        #endregion
    }
}


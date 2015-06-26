using @base.control;
using @base.model;
using client.Common.Helper;
using client.Common.Models;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using CocosSharp;
using Microsoft.Xna.Framework;
using client.Common.Manager;




namespace client.Common.Views
{
    public class WorldLayer : CCLayerColor
    {
        public WorldLayer (RegionPosition regionPosition)
            : base ()
        {
            m_RegionView = new RegionView ();
            m_RegionManagerController = Controller.Instance.RegionStatesController.Curr as client.Common.Manager.RegionManagerController;
            m_EntityManagerController = Controller.Instance.DefinitionManagerController as client.Common.Manager.EntityManagerController;

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
            m_WorldTileMap.TileLayersContainer.Scale = m_Scale;

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
                CCPoint ScreenStart0 = touches [0].StartLocationOnScreen;
                CCPoint ScreenStart1 = touches [1].StartLocationOnScreen;


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

                float relation = (CurrentDistance - StartDistance) / ScreenDistance;

                //scale
                var newScale = m_Scale + relation;
                if (0.3f < newScale && newScale < 2.0f) {
                    m_NewScale = newScale;
                    m_WorldTileMap.TileLayersContainer.Scale = m_NewScale;
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
            var touchEnd = touches [0];
            CheckCenterRegion (touchEnd.Location);
            //Set Current Scale
            m_Scale = m_NewScale;

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
                DrawRegionsAsync (m_Geolocation.CurrentGamePosition);
                DrawEntitiesAsync (m_Geolocation.CurrentGamePosition);
                m_Geolocation.IsPositionChanged = false;
            }

        }

        #endregion

        #region


        void SetCurrentPositionOnce (Position position)
        {
            var tileCoordinate = m_RegionView.GetCurrentTileInMap (position);
            m_CurrentPositionNode.DrawHexagonForIsoStagMap (ClientConstants.TILE_IMAGE_WIDTH, m_TerrainLayer,
                tileCoordinate, new CCColor4F (CCColor3B.Red), 255, 3.0f);
        }

        async Task DrawRegionsAsync (Position gamePosition)
        {
            GameAppDelegate.LoadingState = GameAppDelegate.Loading.RegionLoading;
            await m_RegionManagerController.LoadRegionsAsync ();
            GameAppDelegate.LoadingState = GameAppDelegate.Loading.RegionLoaded;
            m_RegionView.SetTilesInMap160 (m_TerrainLayer, m_RegionManagerController.GetRegionByGamePosition (gamePosition));
            SetCurrentPositionOnce (gamePosition);
            SetMapAnchor (gamePosition);
            m_CenterRegionPosition = new RegionPosition (gamePosition);

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
            
        async Task DrawEntitiesAsync (Position gamePosition)
        {
            GameAppDelegate.LoadingState = GameAppDelegate.Loading.EntitiesLoading;
            await m_EntityManagerController.LoadEntitiesAsync (gamePosition, m_CenterRegionPosition);
            GameAppDelegate.LoadingState = GameAppDelegate.Loading.EntitiesLoaded;
            m_RegionView.SetTilesInMap160 (m_UnitLayer, m_RegionManagerController.GetRegion (m_CenterRegionPosition));
            m_RegionView.SetTilesInMap160 (m_BuildingLayer, m_RegionManagerController.GetRegion (m_CenterRegionPosition));
        }

        void CheckCenterRegion (CCPoint location)
        {            
            var mapCell = GetMapCell (m_WorldTileMap.LayerNamed ("Layer 0"), location);

//            if (m_RegionC.IsCellInOutsideRegion (mapCell)) {
//                var position = m_RegionC.GetCurrentGamePosition (mapCell, m_CenterRegionPosition);
//                DrawRegions (position);
//            }

        }


        void SetMapAnchor (Position anchorPosition)
        {
            var mapCellPosition = new MapCellPosition (m_RegionView.GetCurrentTileInMap (anchorPosition));
            var anchor = mapCellPosition.GetAnchor ();
            m_WorldTileMap.TileLayersContainer.AnchorPoint = anchor;
        }

        MapCellPosition GetMapCell (CCTileMapLayer layer, CCPoint location)
        {
            var point = layer.WorldToParentspace (location);
            var tileMapCooardinate = layer.ClosestTileCoordAtNodePosition (point);
            return new MapCellPosition (tileMapCooardinate);
        }


        #endregion

        #region Properties

        CCTileMap m_WorldTileMap;
        CCTileMapLayer m_TerrainLayer;
        CCTileMapLayer m_BuildingLayer;
        CCTileMapLayer m_UnitLayer;
        CCTileMapLayer m_MenuLayer;

        RegionView m_RegionView;
        RegionPosition m_CenterRegionPosition;
        client.Common.Manager.RegionManagerController m_RegionManagerController;
        client.Common.Manager.EntityManagerController m_EntityManagerController;

        DrawNode m_CurrentPositionNode;
        Geolocation m_Geolocation;

        float m_NewScale = 0.5f;
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


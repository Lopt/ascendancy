using @base.control;
using @base.model;
using client.Common.Helper;
using client.Common.Models;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.ComponentModel.DataAnnotations;
using CocosSharp;
using Microsoft.Xna.Framework;
using client.Common.Manager;




namespace client.Common.Views
{
    public class WorldLayer : CCLayerColor
    {
        public enum TouchGesture
        {
            None,
            Start,
            Menu,
            Move,
            Walk,
            Zoom
        }

        public WorldLayer (RegionPosition regionPosition)
            : base ()
        {
            RegionView = new RegionView ();
            m_regionManagerController = Controller.Instance.RegionManagerController as client.Common.Manager.RegionManagerController;
            m_entityManagerController = Controller.Instance.DefinitionManagerController as client.Common.Manager.DefinitionManagerController;

            m_worldTileMap = new CCTileMap (ClientConstants.TILEMAP_FILE);
            m_geolocation = Geolocation.GetInstance;

            m_currentPositionNode = new DrawNode ();
            m_worldTileMap.TileLayersContainer.AddChild (m_currentPositionNode);

            m_terrainLayer = m_worldTileMap.LayerNamed (ClientConstants.LAYER_TERRAIN);
            m_buildingLayer   = m_worldTileMap.LayerNamed (ClientConstants.LAYER_BUILDING);
            m_unitLayer       = m_worldTileMap.LayerNamed (ClientConstants.LAYER_UNIT);
            m_menuLayer       = m_worldTileMap.LayerNamed (ClientConstants.LAYER_MENU);

            RegionView.TerrainLayer = m_terrainLayer;
            RegionView.BuildingLayer = m_buildingLayer;
            RegionView.UnitLayer = m_unitLayer;
            RegionView.MenuLayer = m_menuLayer;

            m_touchGesture = TouchGesture.None;

            this.AddChild (m_worldTileMap);


            this.Schedule (CheckGeolocation);



            m_timer = new Stopwatch ();

            var TouchListener = new CCEventListenerTouchAllAtOnce ();
            TouchListener.OnTouchesMoved = onTouchesMoved;
            TouchListener.OnTouchesBegan = onTouchesBegan;
            TouchListener.OnTouchesEnded = onTouchesEnded;

            AddEventListener (TouchListener);

            m_worker = new Controllers.Worker (this);
            Schedule (m_worker.Schedule);


        }

        #region overide

        protected override void AddedToScene ()
        {
            base.AddedToScene ();

            SetMapAnchor (m_geolocation.CurrentGamePosition);
            m_worldTileMap.TileLayersContainer.PositionX = VisibleBoundsWorldspace.MidX;
            m_worldTileMap.TileLayersContainer.PositionY = VisibleBoundsWorldspace.MidY;
            m_worldTileMap.TileLayersContainer.Scale = m_scale;

        }

        #endregion

        #region Listener
       

        void onTouchesMoved (List<CCTouch> touches, CCEvent touchEvent)
        {
            if (touches.Count == 1 && 
                (m_touchGesture == TouchGesture.Start ||
                m_touchGesture == TouchGesture.Move))
            {
                    m_touchGesture = TouchGesture.Move;
                    var touch = touches [0];
                    CCPoint diff = touch.Delta;
                    m_worldTileMap.TileLayersContainer.Position += diff;
            }

            if (touches.Count >= 2 &&
                (m_touchGesture == TouchGesture.Start ||
                m_touchGesture == TouchGesture.Zoom))
            {
                m_touchGesture = TouchGesture.Zoom;

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
                var newScale = m_scale + relation;
                if (0.3f < newScale && newScale < 2.0f) {
                    m_newScale = newScale;
                    m_worldTileMap.TileLayersContainer.Scale = m_newScale;
                }
            }
        }

        void onTouchesBegan (List<CCTouch> touches, CCEvent touchEvent)
        {
            switch(m_touchGesture)
            {
            case (TouchGesture.Menu):
                ShowMenu (m_startCoord, 0);
                break;
            }

            m_touchGesture = TouchGesture.Start;
            m_timer.Reset ();
            m_timer.Start ();

            //get Touch location and Corresponding TilePosition
            var location = m_terrainLayer.WorldToParentspace(touches [0].Location);
            m_startCoord = m_terrainLayer.ClosestTileCoordAtNodePosition (location);
        }


        void onTouchesEnded (List<CCTouch> touches, CCEvent touchEvent)
        {
            m_timer.Stop ();

            switch (m_touchGesture)
            {

            case(TouchGesture.Zoom):
                //Set Current Scale
                m_scale = m_newScale;
                m_touchGesture = TouchGesture.None;
                break;

            case(TouchGesture.Move):
                CheckCenterRegion (touches [0].Location);
                m_touchGesture = TouchGesture.None;
                break;

            case(TouchGesture.Start):
                m_touchGesture = TouchGesture.Menu;
                //Draw the taped ISO Tile
                //m_CurrentPositionNode.DrawISOForIsoStagMap(131, m_UnitLayer,tileCoordinate, new CCColor4F (CCColor3B.Blue), 255, 3.0f);
                ShowMenu(m_startCoord, 1);
                break;
            }


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

        }

        public void ShowMenu(CCTileMapCoordinates location, int menutype) 
        {
            CCTileMapCoordinates coordHelper1, coordHelper2, coordHelper3, coordHelper4, coordHelper5, coordHelper6; 
            CCTileGidAndFlags gidHelper1, gidHelper2, gidHelper3, gidHelper4, gidHelper5, gidHelper6;

            coordHelper1.Column = location.Column + (location.Row) % 2;
            coordHelper1.Row    = location.Row - 1;

            coordHelper2.Column = location.Column + (location.Row) % 2;
            coordHelper2.Row    = location.Row + 1;        

            coordHelper3.Column = location.Column;
            coordHelper3.Row    = location.Row + 2;

            coordHelper4.Column = location.Column - (location.Row+1) % 2;
            coordHelper4.Row    = location.Row + 1;

            coordHelper5.Column = location.Column - (location.Row+1) % 2;
            coordHelper5.Row    = location.Row - 1;

            coordHelper6.Column = location.Column;
            coordHelper6.Row    = location.Row - 2;

            switch (menutype) 
            {
            //clears the Menu at around a given Position
            case 0:
                gidHelper1.Gid = 0;
                m_menuLayer.SetTileGID (gidHelper1, coordHelper1);
                m_menuLayer.SetTileGID (gidHelper1, coordHelper2);
                m_menuLayer.SetTileGID (gidHelper1, coordHelper3);
                m_menuLayer.SetTileGID (gidHelper1, coordHelper4);
                m_menuLayer.SetTileGID (gidHelper1, coordHelper5);
                m_menuLayer.SetTileGID (gidHelper1, coordHelper6);
                break;
            default:
                gidHelper1.Gid = 52;
                gidHelper2.Gid = 53;
                gidHelper3.Gid = 54;
                gidHelper4.Gid = 55;
                gidHelper5.Gid = 56;
                gidHelper6.Gid = 57;

                m_menuLayer.SetTileGID (gidHelper1, coordHelper1);
                m_menuLayer.SetTileGID (gidHelper2, coordHelper2);
                m_menuLayer.SetTileGID (gidHelper3, coordHelper3);
                m_menuLayer.SetTileGID (gidHelper4, coordHelper4);
                m_menuLayer.SetTileGID (gidHelper5, coordHelper5);
                m_menuLayer.SetTileGID (gidHelper6, coordHelper6);
                break;
            }

            //TODO: find better solution
            m_worldTileMap.TileLayersContainer.Position += new CCPoint(0.0001f, 0.0001f);

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
            if (m_geolocation.IsPositionChanged) {
                DrawRegionsAsync (m_geolocation.CurrentGamePosition);
                m_geolocation.IsPositionChanged = false;
            }

        }

        #endregion

        #region


        void SetCurrentPositionOnce (Position position)
        {
            var tileCoordinate = RegionView.GetCurrentTileInMap (position);
            m_currentPositionNode.DrawHexagonForIsoStagMap (ClientConstants.TILE_IMAGE_WIDTH, m_terrainLayer,
                tileCoordinate, new CCColor4F (CCColor3B.Red), 255, 3.0f);
        }

        async Task DrawRegionsAsync (Position gamePosition)
        {
            CenterPosition = gamePosition;
            GameAppDelegate.LoadingState = GameAppDelegate.Loading.RegionLoading;
            await m_regionManagerController.LoadRegionsAsync ();
            GameAppDelegate.LoadingState = GameAppDelegate.Loading.RegionLoaded;
            GameAppDelegate.LoadingState = GameAppDelegate.Loading.EntitiesLoading;
            await EntityManagerController.Instance.LoadEntitiesAsync (gamePosition, CenterPosition.RegionPosition);
            GameAppDelegate.LoadingState = GameAppDelegate.Loading.EntitiesLoaded;

            RegionView.SetTilesInMap160 (m_regionManagerController.GetRegionByGamePosition (gamePosition));
            SetCurrentPositionOnce (gamePosition);
            SetMapAnchor (gamePosition);


        }
            

        //clears a Layer
        void ClearLayer (CCTileMapLayer _Layer)
        {
            CCTileGidAndFlags gidHelper1;
            CCTileMapCoordinates coordHelper1;

            gidHelper1.Gid = 0;
            for (int i = 0; i < ClientConstants.TILEMAP_WIDTH; i++) 
            {
                coordHelper1.Column = i;
                for (int j = 0; j < ClientConstants.TILEMAP_HIGH; j++) 
                {
                    coordHelper1.Row = j;
                    _Layer.SetTileGID (gidHelper1, coordHelper1);
                }
            }
        }

            

        void CheckCenterRegion (CCPoint location)
        {            
            var mapCell = GetMapCell (m_worldTileMap.LayerNamed ("Layer 0"), location);

//            if (m_RegionC.IsCellInOutsideRegion (mapCell)) {
//                var position = m_RegionC.GetCurrentGamePosition (mapCell, m_CenterRegionPosition);
//                DrawRegions (position);
//            }

        }


        void SetMapAnchor (Position anchorPosition)
        {
            var mapCellPosition = new MapCellPosition (RegionView.GetCurrentTileInMap (anchorPosition));
            var anchor = mapCellPosition.GetAnchor ();
            m_worldTileMap.TileLayersContainer.AnchorPoint = anchor;
        }

        MapCellPosition GetMapCell (CCTileMapLayer layer, CCPoint location)
        {
            var point = layer.WorldToParentspace (location);
            var tileMapCooardinate = layer.ClosestTileCoordAtNodePosition (point);
            return new MapCellPosition (tileMapCooardinate);
        }

        public CCTileMapCoordinates PositionToTileMapCoordinates(PositionI position)
        {
            var leftTop = new PositionI((int) CenterPosition.X, (int) CenterPosition.Y) - new PositionI ((int) (Constants.REGION_SIZE_X * 2.5), (int) (Constants.REGION_SIZE_Y * 2.5));
            var cellPosition = position - leftTop;
            var MapPosition = new MapCellPosition (cellPosition.X, cellPosition.Y);

            return MapPosition.GetTileMapCoordinates();
        }

        #endregion

        #region Properties

        CCTileMap m_worldTileMap;
        CCTileMapLayer m_terrainLayer;
        CCTileMapLayer m_buildingLayer;
        CCTileMapLayer m_unitLayer;
        CCTileMapLayer m_menuLayer;

        client.Common.Manager.RegionManagerController m_regionManagerController;
        client.Common.Manager.DefinitionManagerController m_entityManagerController;

        DrawNode m_currentPositionNode;
        Geolocation m_geolocation;



        float m_newScale = 0.5f;
        float m_scale = 0.5f;
        Controllers.Worker m_worker;

        int counter = 0;
        bool UnitMove = false;
        bool MenuDrawn = false;


        CCTileMapCoordinates m_startCoord;
        Stopwatch m_timer;
        TouchGesture m_touchGesture;

        public RegionView RegionView
        {
            private set;
            get;
        }


        public Position CenterPosition
        {
            private set;
            get;
        }


        #endregion
    }
}


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
            m_regionView = new RegionView ();
            m_regionManagerController = Controller.Instance.RegionManagerController as client.Common.Manager.RegionManagerController;
            m_entityManagerController = Controller.Instance.DefinitionManagerController as client.Common.Manager.EntityManagerController;

            m_worldTileMap = new CCTileMap (ClientConstants.TILEMAP_FILE);
            m_geolocation = Geolocation.GetInstance;

            m_currentPositionNode = new DrawNode ();
            m_worldTileMap.TileLayersContainer.AddChild (m_currentPositionNode);

            m_terrainLayer = m_worldTileMap.LayerNamed (ClientConstants.LAYER_TERRAIN);
            m_buildingLayer = m_worldTileMap.LayerNamed (ClientConstants.LAYER_BUILDING);
            m_unitLayer = m_worldTileMap.LayerNamed (ClientConstants.LAYER_UNIT);
            m_menuLayer = m_worldTileMap.LayerNamed (ClientConstants.LAYER_MENU);

            m_touchGesture = TouchGesture.None;

            this.AddChild (m_worldTileMap);

            this.Schedule (CheckGeolocation);


            m_timer = new Stopwatch ();

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
                m_touchGesture == TouchGesture.Move)) {
                m_touchGesture = TouchGesture.Move;
                var touch = touches [0];
                CCPoint diff = touch.Delta;

                var anchor = m_worldTileMap.TileLayersContainer.AnchorPoint;
                diff.X = diff.X / m_worldTileMap.TileLayersContainer.ContentSize.Width;
                diff.Y = diff.Y / m_worldTileMap.TileLayersContainer.ContentSize.Height;
                anchor.X -= diff.X;
                anchor.Y -= diff.Y;

                m_worldTileMap.TileLayersContainer.AnchorPoint = anchor;

            }

            if (touches.Count >= 2 &&
                (m_touchGesture == TouchGesture.Start ||
                m_touchGesture == TouchGesture.Zoom)) {
                m_touchGesture = TouchGesture.Zoom;
            

                CCPoint ScreenStart0 = touches [0].StartLocationOnScreen;
                CCPoint ScreenStart1 = touches [1].StartLocationOnScreen;


                float xDist = (ScreenStart1.X - ScreenStart0.X);
                float yDist = (ScreenStart1.Y - ScreenStart0.Y);
                float StartDistance = (xDist * xDist) + (yDist * yDist);

                //calculate Current Position
                CCPoint CurrentPoint0 = touches [0].LocationOnScreen;
                CCPoint CurrentPoint1 = touches [1].LocationOnScreen;

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
                if (ClientConstants.TILEMAP_MIN_SCALE < newScale && newScale < ClientConstants.TILEMAP_MAX_SCALE) {
                    m_newScale = newScale;
                    m_worldTileMap.TileLayersContainer.Scale = m_newScale;
                }
              
            }
        }

        void onTouchesBegan (List<CCTouch> touches, CCEvent touchEvent)
        {
            switch (m_touchGesture) {
            case (TouchGesture.Menu):
                ShowMenu (m_startCoord, 0);
                break;
            }

            m_touchGesture = TouchGesture.Start;
            m_timer.Reset ();
            m_timer.Start ();

            //get Touch location and Corresponding TilePosition
            var location = m_terrainLayer.WorldToParentspace (touches [0].Location);
            m_startCoord = m_terrainLayer.ClosestTileCoordAtNodePosition (location);
        }


        void onTouchesEnded (List<CCTouch> touches, CCEvent touchEvent)
        {
            m_timer.Stop ();

            switch (m_touchGesture) {

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
                ShowMenu (m_startCoord, 1);
                break;
            }


            //Menu Handling
            if(m_menuLayer.TileGIDAndFlags(m_startCoord) != 0)
            {
                
                //Action = new @base.control.action ();
                //@base.control.action.CreateUnit newAction;

                //newAction.Model.



                switch(m_menuLayer.TileGIDAndFlags(m_startCoord))
                {
                case 58:
                   //set action to create headquater
                    //newAction = new @base.model.Action(account, @base.model.Action.ActionType.CreateHeadquarter, System);
                break;
                case 59:
                    //set action to create unit legolas
                    //newAction = new @base.model.Action(account, @base.model.Action.ActionType.CreateUnit, System);
                break;
                case 60:
                    //set action to create unit warrior
                    //newAction = new @base.model.Action(account, @base.model.Action.ActionType.CreateUnit, System);
                break;
                case 61:
                    //set action to create unit mage
                    //newAction = new @base.model.Action(account, @base.model.Action.ActionType.CreateUnit, System);
                break;
                case 62:
                    //set action to create unit scout
                    //newAction = new @base.model.Action(account, @base.model.Action.ActionType.CreateUnit, System);

                break;
                }
                //if(newAction.== true)
                //{
                //  action.do();
                //}
                return;
            }

            if (m_unitLayer.TileGIDAndFlags (m_startCoord) != 0) 
            {
                m_oldunitCoord = m_startCoord;
                m_unitmove = true;
                return;
            }
               
            //Movement Handling 
            if(m_unitmove == true)
            {
                //
                //if (Action.possible (move(m_oldunitCoord, m_startCoord)) == true) 
                //{
                //    Action.do(move(m_oldunitCoord, m_startCoord));
                //}
                m_unitmove = false;
                return;
            }

        }

        public void ShowMenu (CCTileMapCoordinates location, int menutype)
        {
            CCTileMapCoordinates coordHelper1, coordHelper2, coordHelper3, coordHelper4, coordHelper5, coordHelper6; 
            CCTileGidAndFlags gidHelper1, gidHelper2, gidHelper3, gidHelper4, gidHelper5, gidHelper6;

            coordHelper1.Column = location.Column + (location.Row) % 2;
            coordHelper1.Row = location.Row - 1;

            coordHelper2.Column = location.Column + (location.Row) % 2;
            coordHelper2.Row = location.Row + 1;        

            coordHelper3.Column = location.Column;
            coordHelper3.Row = location.Row + 2;

            coordHelper4.Column = location.Column - (location.Row + 1) % 2;
            coordHelper4.Row = location.Row + 1;

            coordHelper5.Column = location.Column - (location.Row + 1) % 2;
            coordHelper5.Row = location.Row - 1;

            coordHelper6.Column = location.Column;
            coordHelper6.Row = location.Row - 2;

            switch (menutype) {
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
            case 1: //UnitMenu
                gidHelper1.Gid = ClientConstants.MenueBogenschütze;
                gidHelper2.Gid = ClientConstants.MenueKrieger;
                gidHelper3.Gid = ClientConstants.MenueMagier;
                gidHelper4.Gid = ClientConstants.MenueSpäher;
                m_menuLayer.SetTileGID (gidHelper1, coordHelper1);
                m_menuLayer.SetTileGID (gidHelper2, coordHelper2);
                m_menuLayer.SetTileGID (gidHelper3, coordHelper3);
                m_menuLayer.SetTileGID (gidHelper4, coordHelper4);
                break;
            case 2: //BuildingMenu
                   
                break;
            default:
                gidHelper1.Gid = ClientConstants.MenueErde;
                gidHelper2.Gid = ClientConstants.MenueFeuer;
                gidHelper3.Gid = ClientConstants.MenueGold;
                gidHelper4.Gid = ClientConstants.MenueLuft;
                gidHelper5.Gid = ClientConstants.MenueMana;
                gidHelper6.Gid = ClientConstants.MenueWasser;

                m_menuLayer.SetTileGID (gidHelper1, coordHelper1);
                m_menuLayer.SetTileGID (gidHelper2, coordHelper2);
                m_menuLayer.SetTileGID (gidHelper3, coordHelper3);
                m_menuLayer.SetTileGID (gidHelper4, coordHelper4);
                m_menuLayer.SetTileGID (gidHelper5, coordHelper5);
                m_menuLayer.SetTileGID (gidHelper6, coordHelper6);
                break;
            }
           
            //TODO: find better solution
            m_worldTileMap.TileLayersContainer.Position += new CCPoint (0.0001f, 0.0001f);

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
                DrawEntitiesAsync (m_geolocation.CurrentGamePosition);
                m_geolocation.IsPositionChanged = false;
            }

        }

        #endregion

        #region


        void SetCurrentPositionOnce (Position position)
        {
            var tileCoordinate = m_regionView.GetCurrentTileInMap (position);
            if (tileCoordinate.Column > -1) {
                m_currentPositionNode.DrawHexagonForIsoStagMap (ClientConstants.TILE_IMAGE_WIDTH, m_terrainLayer,
                    tileCoordinate, new CCColor4F (CCColor3B.Red), 255, 3.0f);
            }
        }

        async Task DrawRegionsAsync (Position gamePosition)
        {
            GameAppDelegate.LoadingState = GameAppDelegate.Loading.RegionLoading;
            await m_regionManagerController.LoadRegionsAsync ();
            GameAppDelegate.LoadingState = GameAppDelegate.Loading.RegionLoaded;
            m_regionView.SetTilesInMap160 (m_terrainLayer, m_regionManagerController.GetRegionByGamePosition (gamePosition));
            SetCurrentPositionOnce (m_geolocation.CurrentGamePosition);
            SetMapAnchor (gamePosition);
            m_centerRegionPosition = new RegionPosition (gamePosition);

        }
            

        //clears a Layer
        void ClearLayer (CCTileMapLayer _Layer)
        {
            CCTileGidAndFlags gidHelper1;
            CCTileMapCoordinates coordHelper1;

            gidHelper1.Gid = 0;
            for (int i = 0; i < ClientConstants.TILEMAP_WIDTH; i++) {
                coordHelper1.Column = i;
                for (int j = 0; j < ClientConstants.TILEMAP_HIGH; j++) {
                    coordHelper1.Row = j;
                    _Layer.SetTileGID (gidHelper1, coordHelper1);
                }
            }
        }

            
        async Task DrawEntitiesAsync (Position gamePosition)
        {
            GameAppDelegate.LoadingState = GameAppDelegate.Loading.EntitiesLoading;
            await m_entityManagerController.LoadEntitiesAsync (gamePosition, m_centerRegionPosition);
            GameAppDelegate.LoadingState = GameAppDelegate.Loading.EntitiesLoaded;
            m_regionView.SetTilesInMap160 (m_unitLayer, m_regionManagerController.GetRegion (m_centerRegionPosition));
            m_regionView.SetTilesInMap160 (m_buildingLayer, m_regionManagerController.GetRegion (m_centerRegionPosition));
        }

        void CheckCenterRegion (CCPoint location)
        {            
            var mapCell = GetMapCell (m_terrainLayer, location);

//            if (m_regionView.IsCellInOutsideRegion (mapCell)) {
//                var position = m_regionView.GetCurrentGamePosition (mapCell, m_centerRegionPosition);
//                DrawRegionsAsync (position);
//                DrawEntitiesAsync (position);
//            }

        }


        void SetMapAnchor (Position anchorPosition)
        {
            var mapCellPosition = new MapCellPosition (m_regionView.GetCurrentTileInMap (anchorPosition));
            var anchor = mapCellPosition.GetAnchor ();
            m_worldTileMap.TileLayersContainer.AnchorPoint = anchor;
        }

        MapCellPosition GetMapCell (CCTileMapLayer layer, CCPoint location)
        {
            var point = layer.WorldToParentspace (location);
            var tileMapCooardinate = layer.ClosestTileCoordAtNodePosition (point);
            return new MapCellPosition (tileMapCooardinate);
        }


        #endregion

        #region Properties

        CCTileMap m_worldTileMap;
        CCTileMapLayer m_terrainLayer;
        CCTileMapLayer m_buildingLayer;
        CCTileMapLayer m_unitLayer;
        CCTileMapLayer m_menuLayer;

        RegionView m_regionView;
        RegionPosition m_centerRegionPosition;
        client.Common.Manager.RegionManagerController m_regionManagerController;
        client.Common.Manager.EntityManagerController m_entityManagerController;

        DrawNode m_currentPositionNode;
        Geolocation m_geolocation;


        float m_newScale = 0.5f;
        float m_scale = 0.5f;
        int counter = 0;
        bool m_unitmove = false;
        bool MenuDrawn = false;


        CCTileMapCoordinates m_startCoord, m_oldunitCoord;
        Stopwatch m_timer;
        TouchGesture m_touchGesture;

        #endregion
    }
}


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

        public WorldLayer(RegionPosition regionPosition)
            : base()
        {
            RegionView = new RegionView();
            m_regionManagerController = Controller.Instance.RegionManagerController as client.Common.Manager.RegionManagerController;
            m_entityManagerController = Controller.Instance.DefinitionManagerController as client.Common.Manager.DefinitionManagerController;

            m_worldTileMap = new CCTileMap(ClientConstants.TILEMAP_FILE);
            m_geolocation = Geolocation.Instance;
            CenterPosition = m_geolocation.CurrentGamePosition;

            m_currentPositionNode = new DrawNode();
            m_worldTileMap.TileLayersContainer.AddChild(m_currentPositionNode);

            m_terrainLayer = m_worldTileMap.LayerNamed(ClientConstants.LAYER_TERRAIN);
            m_buildingLayer = m_worldTileMap.LayerNamed(ClientConstants.LAYER_BUILDING);
            m_unitLayer = m_worldTileMap.LayerNamed(ClientConstants.LAYER_UNIT);
            m_menuLayer = m_worldTileMap.LayerNamed(ClientConstants.LAYER_MENU);


            ClearLayers();

            RegionView.TerrainLayer = m_terrainLayer;
            RegionView.BuildingLayer = m_buildingLayer;
            RegionView.UnitLayer = m_unitLayer;
            RegionView.MenuLayer = m_menuLayer;


            m_touchGesture = TouchGesture.None;
                
            this.AddChild(m_worldTileMap);


            this.Schedule(CheckGeolocation);



            m_timer = new Stopwatch();

            var TouchListener = new CCEventListenerTouchAllAtOnce();
            TouchListener.OnTouchesMoved = onTouchesMoved;
            TouchListener.OnTouchesBegan = onTouchesBegan;
            TouchListener.OnTouchesEnded = onTouchesEnded;

            AddEventListener(TouchListener);

            m_worker = new Views.Worker(this);
            EntityManagerController.Worker = m_worker;

            Schedule(m_worker.Schedule);


        }

        #region overide

        protected override void AddedToScene()
        {
            base.AddedToScene();

            SetMapAnchor(m_geolocation.CurrentGamePosition);
            m_worldTileMap.TileLayersContainer.PositionX = VisibleBoundsWorldspace.MidX;
            m_worldTileMap.TileLayersContainer.PositionY = VisibleBoundsWorldspace.MidY;
            m_worldTileMap.TileLayersContainer.Scale = m_scale;

        }

        #endregion

        #region Listener


        void onTouchesMoved(List<CCTouch> touches, CCEvent touchEvent)
        {
            if (touches.Count == 1 &&
                (m_touchGesture == TouchGesture.Start ||
                m_touchGesture == TouchGesture.Move))
            {
                m_touchGesture = TouchGesture.Move;
                var touch = touches[0];
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
                m_touchGesture == TouchGesture.Zoom))
            {
                m_touchGesture = TouchGesture.Zoom;
            

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

                float relation = (CurrentDistance - StartDistance) / ScreenDistance;

                //scale
                var newScale = m_scale + relation;
                if (ClientConstants.TILEMAP_MIN_SCALE < newScale && newScale < ClientConstants.TILEMAP_MAX_SCALE)
                {
                    m_newScale = newScale;
                    m_worldTileMap.TileLayersContainer.Scale = m_newScale;
                }
              
            }
        }

        void onTouchesBegan(List<CCTouch> touches, CCEvent touchEvent)
        {
            switch (m_touchGesture)
            {
                case (TouchGesture.Menu):
                //ShowMenu (m_startCoord, 0);
                    break;
                case (TouchGesture.None):
                    m_touchGesture = TouchGesture.Start;
                    break;
            }

            m_timer.Reset();
            m_timer.Start();

            //get Touch location and Corresponding TilePosition
            var location = m_terrainLayer.WorldToParentspace(touches[0].Location);
            m_startLocation = location;
            m_startCoord = m_terrainLayer.ClosestTileCoordAtNodePosition(location);
            if (RegionView.UnitLayer.TileGIDAndFlags(m_startCoord).Gid != 0 && m_touchGesture == TouchGesture.Start && RegionView.BuildingLayer.TileGIDAndFlags(m_startCoord).Gid == 0)
            {
                m_touchGesture = TouchGesture.Walk;
            }
        }


        void onTouchesEnded(List<CCTouch> touches, CCEvent touchEvent)
        {
            m_timer.Stop();

            switch (m_touchGesture)
            {

                case(TouchGesture.Zoom):
                //Set Current Scale
                    m_scale = m_newScale;
                    m_touchGesture = TouchGesture.None;
                    break;

                case(TouchGesture.Move):
                    CheckCenterRegion();
                    m_touchGesture = TouchGesture.None;
                    break;

                case(TouchGesture.Start):
                    if (!m_menueDrawn)
                    { 
                        if (m_buildingLayer.TileGIDAndFlags(m_startCoord).Gid != 0)
                        {
                            ShowMenu(m_startCoord, 1);
                            m_touchGesture = TouchGesture.Menu;
                        }
                        else
                        {
                            ShowMenu(m_startCoord, 2);
                            m_touchGesture = TouchGesture.Menu;
                        }
                 
                    }
                    break;
                case(TouchGesture.Menu):
                /*
                var dictParam = new System.Collections.Concurrent.ConcurrentDictionary<string,object> ();
                MapCellPosition tapMapCellPosition = new MapCellPosition (m_startCoord);//GetMapCell(m_menuLayer, m_startLocation);
                Position tapPosition = RegionView.GetCurrentGamePosition (tapMapCellPosition, CenterPosition.RegionPosition);
                PositionI tapPositionI = new PositionI ((int)tapPosition.X, (int)tapPosition.Y);
                dictParam [@base.control.action.CreateUnit.CREATE_POSITION] = tapPositionI; 
                @base.model.Action newAction = null;
                */
                    switch (m_menuLayer.TileGIDAndFlags(m_startCoord).Gid)
                    {
                        case ClientConstants.CROSS_GID:
                            ShowMenu(m_coordHelper, 0);
                            break;
                        case ClientConstants.MENUEEARTH_GID:
                        case ClientConstants.MENUEAIR_GID:
                        case ClientConstants.MENUEWATER_GID:
                        case ClientConstants.MENUEGOLD_GID:
                        case ClientConstants.MENUEMANA_GID:
                        case ClientConstants.MENUEFIRE_GID:    
                    //set action to create headquater
                            CreateBuilding(m_coordHelper, 276);
                    //clears the menu after taped
                            ShowMenu(m_coordHelper, 0);
                            break;
                        case ClientConstants.MENUEBOWMAN_GID:
                    //set action to create unit legolas
                            CreateUnit(m_coordHelper, 78);
                    //clears the menu after taped
                            ShowMenu(m_coordHelper, 0);
                            break;
                        case ClientConstants.MENUEWARRIOR_GID:
                    //set action to create unit warrior
                            CreateUnit(m_coordHelper, 72);
                    //clears the menu after taped
                            ShowMenu(m_coordHelper, 0);
                            break;
                        case ClientConstants.MENUEMAGE_GID:
                    //set action to create unit mage
                            CreateUnit(m_coordHelper, 66);
                    //clears the menu after taped
                            ShowMenu(m_coordHelper, 0);
                            break;
                        case ClientConstants.MENUESCOUT_GID:
                    //set action to create unit scout (unknown1 at the moment)
                            CreateUnit(m_coordHelper, 84);
                    //clears the menu after taped
                            ShowMenu(m_coordHelper, 0);
                            break;
                        case ClientConstants.MENUEHERO_GID:
                            CreateUnit(m_coordHelper, 60);
                    //clears the menu after taped
                            ShowMenu(m_coordHelper, 0);
                            break;
                        case ClientConstants.MENUEUNKNOWN_GID:
                            CreateUnit(m_coordHelper, 90);
                    //clears the menu after taped
                            ShowMenu(m_coordHelper, 0);
                            break;
                        case 0:
                            ShowMenu(m_coordHelper, 0);
                            break;
                    }
                /*
                 * //uncomment to see create unit action
                if (m_menuLayer.TileGIDAndFlags (m_startCoord).Gid != ClientConstants.CROSS_GID)
                {
                    var actionC = (@base.control.action.Action)newAction.Control;
                    var possible = actionC.Possible (m_regionManagerController);
                    if (possible)
                    {
                        m_worker.Queue.Enqueue (newAction);
                    }
                }
                */



                    break;



                case TouchGesture.Walk:
                    if (!m_unitmove)
                    {
                        m_coordHelper = m_startCoord;
                        m_unitmove = true;
                    }
                    else
                    {   
                        var dictParam = new System.Collections.Generic.Dictionary<string,object>();

                        var startMapCellPosition = new MapCellPosition(m_coordHelper);
                        var startPosition = RegionView.GetCurrentGamePosition(startMapCellPosition, CenterPosition.RegionPosition);
                        var startPositionI = new PositionI((int)startPosition.X, (int)startPosition.Y);
                        dictParam[@base.control.action.MoveUnit.START_POSITION] = startPositionI;

                        var location = m_terrainLayer.WorldToParentspace(touches[0].Location);
                        var endCoord = m_terrainLayer.ClosestTileCoordAtNodePosition(location);

                        var endMapCellPosition = new MapCellPosition(endCoord);
                        var endPosition = RegionView.GetCurrentGamePosition(endMapCellPosition, CenterPosition.RegionPosition);
                        var endPositionI = new PositionI((int)endPosition.X, (int)endPosition.Y);
                        dictParam[@base.control.action.MoveUnit.END_POSITION] = endPositionI;

                        var action = new @base.model.Action(GameAppDelegate.Account, @base.model.Action.ActionType.MoveUnit, dictParam);
                        var actionC = (@base.control.action.Action)action.Control;
                        var possible = actionC.Possible(m_regionManagerController);
                        if (possible)
                        {
                            DoAction(action);
                        }
                        m_unitmove = false;
                        m_touchGesture = TouchGesture.None;
                    }
                    break;
            }
        }

        public void CreateUnit(CCTileMapCoordinates location, int type)
        {
            var dictParam = new System.Collections.Generic.Dictionary<string,object>();
            var tapMapCellPosition = new MapCellPosition(location);
            var tapPosition = RegionView.GetCurrentGamePosition(tapMapCellPosition, CenterPosition.RegionPosition);
            var tapPositionI = new PositionI((int)tapPosition.X, (int)tapPosition.Y);
            dictParam[@base.control.action.CreateUnit.CREATE_POSITION] = tapPositionI; 
            dictParam[@base.control.action.CreateUnit.CREATION_TYPE] = (long)type;
            var newAction = new @base.model.Action(GameAppDelegate.Account, @base.model.Action.ActionType.CreateUnit, dictParam);
            var actionC = (@base.control.action.Action)newAction.Control;
            var possible = actionC.Possible(m_regionManagerController);

            if (possible)
            {
                //actionC.Do (m_regionManagerController);
                //m_worker.Queue.Enqueue (newAction);
                DoAction(newAction);
            }
        }

        public void CreateBuilding(CCTileMapCoordinates location, long type)
        {
            var dictParam = new System.Collections.Generic.Dictionary<string,object>();
            var tapMapCellPosition = new MapCellPosition(location);
            var tapPosition = RegionView.GetCurrentGamePosition(tapMapCellPosition, CenterPosition.RegionPosition);
            var tapPositionI = new PositionI((int)tapPosition.X, (int)tapPosition.Y);
            dictParam[@base.control.action.CreatBuilding.CREATE_POSITION] = tapPositionI; 
            dictParam[@base.control.action.CreatBuilding.CREATION_TYPE] = (long)type;
            var newAction = new @base.model.Action(GameAppDelegate.Account, @base.model.Action.ActionType.CreateBuilding, dictParam);
            var actionC = (@base.control.action.Action)newAction.Control;
            var possible = actionC.Possible(m_regionManagerController);

            if (possible)
            {
                //actionC.Do (m_regionManagerController);
                //m_worker.Queue.Enqueue (newAction);      
                DoAction(newAction);
            }
        }

        public void ShowMenu(CCTileMapCoordinates location, int menutype)
        {
            CCTileMapCoordinates coordHelper1, coordHelper2, coordHelper3, coordHelper4, coordHelper5, coordHelper6; 
            CCTileGidAndFlags gidHelper1, gidHelper2, gidHelper3, gidHelper4, gidHelper5, gidHelper6, gidHelpercenter;

            m_coordHelper = location;

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

            switch (menutype)
            {
            //clears the Menu at around a given Position
                case 0:
                    m_menuLayer.RemoveTile(location);
                    m_menuLayer.RemoveTile(coordHelper1);
                    m_menuLayer.RemoveTile(coordHelper2);
                    m_menuLayer.RemoveTile(coordHelper3);
                    m_menuLayer.RemoveTile(coordHelper4);
                    m_menuLayer.RemoveTile(coordHelper5);
                    m_menuLayer.RemoveTile(coordHelper6);
                    m_menueDrawn = false;
                    m_touchGesture = TouchGesture.None;
                    break;
                case 1: //UnitMenu
                    gidHelpercenter.Gid = ClientConstants.CROSS_GID;
                    gidHelper1.Gid = ClientConstants.MENUEBOWMAN_GID;
                    gidHelper2.Gid = ClientConstants.MENUEHERO_GID;
                    gidHelper3.Gid = ClientConstants.MENUEWARRIOR_GID;
                    gidHelper4.Gid = ClientConstants.MENUEMAGE_GID;
                    gidHelper5.Gid = ClientConstants.MENUESCOUT_GID;
                    gidHelper6.Gid = ClientConstants.MENUEUNKNOWN_GID;
                    m_menuLayer.SetTileGID(gidHelpercenter, location);
                    m_menuLayer.SetTileGID(gidHelper1, coordHelper1);
                    m_menuLayer.SetTileGID(gidHelper2, coordHelper2);
                    m_menuLayer.SetTileGID(gidHelper3, coordHelper3);
                    m_menuLayer.SetTileGID(gidHelper4, coordHelper4);
                    m_menuLayer.SetTileGID(gidHelper5, coordHelper5);
                    m_menuLayer.SetTileGID(gidHelper6, coordHelper6);
                    m_menueDrawn = true;
                    break;
                case 2: //BuildingMenu
                    gidHelpercenter.Gid = ClientConstants.CROSS_GID;
                    gidHelper1.Gid = ClientConstants.MENUEEARTH_GID;
                    gidHelper2.Gid = ClientConstants.MENUEFIRE_GID;
                    gidHelper3.Gid = ClientConstants.MENUEGOLD_GID;
                    gidHelper4.Gid = ClientConstants.MENUEAIR_GID;
                    gidHelper5.Gid = ClientConstants.MENUEMANA_GID;
                    gidHelper6.Gid = ClientConstants.MENUEWATER_GID;
                    m_menuLayer.SetTileGID(gidHelpercenter, location);
                    m_menuLayer.SetTileGID(gidHelper1, coordHelper1);
                    m_menuLayer.SetTileGID(gidHelper2, coordHelper2);
                    m_menuLayer.SetTileGID(gidHelper3, coordHelper3);
                    m_menuLayer.SetTileGID(gidHelper4, coordHelper4);
                    m_menuLayer.SetTileGID(gidHelper5, coordHelper5);
                    m_menuLayer.SetTileGID(gidHelper6, coordHelper6);
                    m_menueDrawn = true;
                    break;
                default:
                    gidHelpercenter.Gid = ClientConstants.CROSS_GID;
                    gidHelper1.Gid = ClientConstants.MENUEEARTH_GID;
                    gidHelper2.Gid = ClientConstants.MENUEFIRE_GID;
                    gidHelper3.Gid = ClientConstants.MENUEGOLD_GID;
                    gidHelper4.Gid = ClientConstants.MENUEAIR_GID;
                    gidHelper5.Gid = ClientConstants.MENUEMANA_GID;
                    gidHelper6.Gid = ClientConstants.MENUEWATER_GID;
                    m_menuLayer.SetTileGID(gidHelpercenter, location);
                    m_menuLayer.SetTileGID(gidHelper1, coordHelper1);
                    m_menuLayer.SetTileGID(gidHelper2, coordHelper2);
                    m_menuLayer.SetTileGID(gidHelper3, coordHelper3);
                    m_menuLayer.SetTileGID(gidHelper4, coordHelper4);
                    m_menuLayer.SetTileGID(gidHelper5, coordHelper5);
                    m_menuLayer.SetTileGID(gidHelper6, coordHelper6);
                    m_menueDrawn = true;
                    break;

            }
            UglyDraw();

        }


        #endregion

        #region Scheduling

        public void UglyDraw()
        {
            //TODO: find better solution
            m_worldTileMap.TileLayersContainer.Position += new CCPoint(0.0001f, 0.0001f);
        }
            
        void CheckGeolocation(float frameTimesInSecond)
        {
            if (m_geolocation.IsPositionChanged)
            {
                DrawRegionsAsync(m_geolocation.CurrentGamePosition);
                m_geolocation.IsPositionChanged = false;
            }

        }

        #endregion

        #region


        void SetCurrentPositionOnce(Position position)
        {
            var tileCoordinate = Helper.PositionHelper.PositionToTileMapCoordinates(CenterPosition, new PositionI(position));
            m_currentPositionNode.DrawHexagonForIsoStagMap(ClientConstants.TILE_IMAGE_WIDTH, m_terrainLayer,
                tileCoordinate, new CCColor4F(CCColor3B.Red), 255, 3.0f);
//            var tileCoordinate = m_regionView.GetCurrentTileInMap (m_geolocation.CurrentGamePosition);

            bool isInWorld = false;
            m_currentPositionNode.Visible = false;

            if (CenterPosition.RegionPosition.Equals(m_geolocation.CurrentRegionPosition))
                isInWorld = true;

            if (tileCoordinate.Column > -1 && isInWorld)
            {
                m_currentPositionNode.Visible = true;
                m_currentPositionNode.DrawHexagonForIsoStagMap(ClientConstants.TILE_IMAGE_WIDTH, m_terrainLayer,
                    tileCoordinate, new CCColor4F(CCColor3B.Red), 255, 3.0f);
            } 
        }

        async Task DrawRegionsAsync(Position gamePosition)
        {
            CenterPosition = gamePosition;
            GameAppDelegate.LoadingState = GameAppDelegate.Loading.RegionLoading;
            await m_regionManagerController.LoadRegionsAsync(new RegionPosition(gamePosition));
            GameAppDelegate.LoadingState = GameAppDelegate.Loading.RegionLoaded;
            GameAppDelegate.LoadingState = GameAppDelegate.Loading.EntitiesLoading;
            await EntityManagerController.Instance.LoadEntitiesAsync(gamePosition, CenterPosition.RegionPosition);
            GameAppDelegate.LoadingState = GameAppDelegate.Loading.EntitiesLoaded;


            RegionView.SetTilesInMap160(m_regionManagerController.GetRegionByGamePosition(gamePosition));
            SetCurrentPositionOnce(gamePosition);
            SetMapAnchor(gamePosition);

            CenterPosition = gamePosition;
            //SetMapAnchor (gamePosition);
            UglyDraw();
        }

        void DoAction(@base.model.Action action)
        {
            var actions = new List<@base.model.Action>();
            actions.Add(action);
            m_regionManagerController.DoActionAsync(m_geolocation.CurrentGamePosition, actions.ToArray());
            var mapCell = GetMapCell(m_terrainLayer, new CCPoint(VisibleBoundsWorldspace.MidX, VisibleBoundsWorldspace.MidY));
            var position = RegionView.GetCurrentGamePosition(mapCell, CenterPosition.RegionPosition);
            //DrawRegionsAsync (position);
        }

        //clears a Layer
        void ClearLayers()
        {
            var coordHelper = new CCTileMapCoordinates(0, 0);
            m_buildingLayer.RemoveTile(coordHelper);
            m_unitLayer.RemoveTile(coordHelper);
            m_menuLayer.RemoveTile(coordHelper);
        }



        void CheckCenterRegion()
        {  
            var mapCell = GetMapCell(m_terrainLayer, new CCPoint(VisibleBoundsWorldspace.MidX, VisibleBoundsWorldspace.MidY));

            if (RegionView.IsCellInOutsideRegion(mapCell))
            {
                CenterPosition = RegionView.GetCurrentGamePosition(mapCell, CenterPosition.RegionPosition);
                DrawRegionsAsync(CenterPosition);
            }

        }


        void SetMapAnchor(Position anchorPosition)
        {
            var mapCellPosition = PositionHelper.PositionToMapCellPosition(CenterPosition, new PositionI(anchorPosition));//new MapCellPosition(RegionView.GetCurrentTileInMap(anchorPosition));
            var anchor = mapCellPosition.GetAnchor();
            m_worldTileMap.TileLayersContainer.AnchorPoint = anchor;
        }

        MapCellPosition GetMapCell(CCTileMapLayer layer, CCPoint location)
        {
            var point = layer.WorldToParentspace(location);
            var tileMapCooardinate = layer.ClosestTileCoordAtNodePosition(point);
            return new MapCellPosition(tileMapCooardinate);
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

        Worker m_worker;

        float m_newScale = ClientConstants.TILEMAP_NORM_SCALE;
        float m_scale = ClientConstants.TILEMAP_NORM_SCALE;
        bool m_unitmove = false;
        bool m_menueDrawn = false;

        CCTileMapCoordinates m_startCoord, m_coordHelper;
        CCPoint m_startLocation;
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


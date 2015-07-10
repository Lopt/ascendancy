using System;
using CocosSharp;
using System.Collections.Generic;
using System.Collections;
using System.Diagnostics;
using System.Threading.Tasks;
using client.Common.Helper;

namespace client.Common.Views
{
    public class TouchHandler
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

        TouchGesture m_touchGesture;
        Stopwatch m_timer;
        CCPoint m_startLocation;
        WorldLayer m_worldLayer;

        float m_newScale = ClientConstants.TILEMAP_NORM_SCALE;
        float m_scale = ClientConstants.TILEMAP_NORM_SCALE;
        bool m_unitmove = false;
        bool m_menueDrawn = false;

        CCTileMapCoordinates m_startCoord;
        CCTileMapCoordinates m_coordHelper;


        public TouchHandler(WorldLayer worldLayer)
        {
            m_timer = new Stopwatch();
            m_touchGesture = TouchGesture.None;
            m_worldLayer = worldLayer;
        }


        public void OnTouchesMoved(List<CCTouch> touches, CCEvent touchEvent)
        {
            if (touches.Count == 1 &&
                (m_touchGesture == TouchGesture.Start ||
                    m_touchGesture == TouchGesture.Move))
            {
                m_touchGesture = TouchGesture.Move;
                var touch = touches[0];

                CCPoint diff = touch.Delta;
                m_worldLayer.MoveWorld(diff);
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
                xDist = m_worldLayer.VisibleBoundsWorldspace.MaxX;
                yDist = m_worldLayer.VisibleBoundsWorldspace.MaxY;

                float ScreenDistance = (xDist * xDist) + (yDist * yDist);

                float relation = (CurrentDistance - StartDistance) / ScreenDistance;

                //scale
                var newScale = m_scale + relation;

                m_worldLayer.ScaleWorld(newScale);
            }
        }

        public void OnTouchesBegan(List<CCTouch> touches, CCEvent touchEvent)
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
            var location = m_worldLayer.WorldToParentspace(touches[0].Location);
            m_startLocation = location;
            m_startCoord = m_worldLayer.ClosestTileCoordAtNodePosition(location);
            if (m_worldLayer.UnitLayer.TileGIDAndFlags(m_startCoord).Gid != 0 &&
                m_touchGesture == TouchGesture.Start && m_worldLayer.BuildingLayer.TileGIDAndFlags(m_startCoord).Gid == 0)
            {
                m_touchGesture = TouchGesture.Walk;
            }
        }


        public void OnTouchesEnded(List<CCTouch> touches, CCEvent touchEvent)
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
                    m_worldLayer.CheckCenterRegion();
                    m_touchGesture = TouchGesture.None;
                    break;

                case(TouchGesture.Start):
                    if (!m_menueDrawn)
                    { 
                        if (m_worldLayer.BuildingLayer.TileGIDAndFlags(m_startCoord).Gid != 0)
                        {
                            m_worldLayer.ShowMenu(m_startCoord, 1);
                        }
                        else
                        {
                            m_worldLayer.ShowMenu(m_startCoord, 2);
                        }

                        m_touchGesture = TouchGesture.Menu;
                        m_menueDrawn = true;
                        m_coordHelper = m_startCoord;

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
                    switch (m_worldLayer.MenuLayer.TileGIDAndFlags(m_startCoord).Gid)
                    {
                        case ClientConstants.CROSS_GID:
                            m_worldLayer.ShowMenu(m_coordHelper, 0);
                            break;
                        case ClientConstants.MENUEEARTH_GID:
                        case ClientConstants.MENUEAIR_GID:
                        case ClientConstants.MENUEWATER_GID:
                        case ClientConstants.MENUEGOLD_GID:
                        case ClientConstants.MENUEMANA_GID:
                        case ClientConstants.MENUEFIRE_GID:    
                            //set action to create headquater
                            m_worldLayer.CreateBuilding(m_coordHelper, 276);
                            //clears the menu after taped
                            m_worldLayer.ShowMenu(m_coordHelper, 0);
                            break;
                        case ClientConstants.MENUEBOWMAN_GID:
                            //set action to create unit legolas
                            m_worldLayer.CreateUnit(m_coordHelper, 78);
                            //clears the menu after taped
                            m_worldLayer.ShowMenu(m_coordHelper, 0);
                            break;
                        case ClientConstants.MENUEWARRIOR_GID:
                            //set action to create unit warrior
                            m_worldLayer.CreateUnit(m_coordHelper, 72);
                            //clears the menu after taped
                            m_worldLayer.ShowMenu(m_coordHelper, 0);
                            break;
                        case ClientConstants.MENUEMAGE_GID:
                            //set action to create unit mage
                            m_worldLayer.CreateUnit(m_coordHelper, 66);
                            //clears the menu after taped
                            m_worldLayer.ShowMenu(m_coordHelper, 0);
                            break;
                        case ClientConstants.MENUESCOUT_GID:
                            //set action to create unit scout (unknown1 at the moment)
                            m_worldLayer.CreateUnit(m_coordHelper, 84);
                            //clears the menu after taped
                            m_worldLayer.ShowMenu(m_coordHelper, 0);
                            break;
                        case ClientConstants.MENUEHERO_GID:
                            m_worldLayer.CreateUnit(m_coordHelper, 60);
                            //clears the menu after taped
                            m_worldLayer.ShowMenu(m_coordHelper, 0);
                            break;
                        case ClientConstants.MENUEUNKNOWN_GID:
                            m_worldLayer.CreateUnit(m_coordHelper, 90);
                            //clears the menu after taped
                            m_worldLayer.ShowMenu(m_coordHelper, 0);
                            break;
                        case 0:
                            m_worldLayer.ShowMenu(m_coordHelper, 0);
                            break;
                    }

                    m_menueDrawn = false;
                    //m_coordHelper = location;
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
                        var gameApp = GameAppDelegate.Instance; 
                        var dictParam = new System.Collections.Generic.Dictionary<string,object>();

                        var startMapCellPosition = new client.Common.Models.MapCellPosition(m_coordHelper);
                        var startPosition = m_worldLayer.RegionView.GetCurrentGamePosition(startMapCellPosition, m_worldLayer.CenterPosition.RegionPosition);
                        var startPositionI = new @base.model.PositionI((int)startPosition.X, (int)startPosition.Y);
                        dictParam[@base.control.action.MoveUnit.START_POSITION] = startPositionI;

                        var location = m_worldLayer.WorldToParentspace(touches[0].Location);
                        var endCoord = m_worldLayer.ClosestTileCoordAtNodePosition(location);

                        var endMapCellPosition = new client.Common.Models.MapCellPosition(endCoord);
                        var endPosition = m_worldLayer.RegionView.GetCurrentGamePosition(endMapCellPosition, m_worldLayer.CenterPosition.RegionPosition);
                        var endPositionI = new @base.model.PositionI((int)endPosition.X, (int)endPosition.Y);
                        dictParam[@base.control.action.MoveUnit.END_POSITION] = endPositionI;

                        var action = new @base.model.Action(gameApp.Account, @base.model.Action.ActionType.MoveUnit, dictParam);
                        var actionC = (@base.control.action.Action)action.Control;
                        var possible = actionC.Possible(@base.control.Controller.Instance.RegionManagerController);
                        if (possible)
                        {
                            m_worldLayer.DoAction(action);
                        }
                        m_unitmove = false;
                        m_touchGesture = TouchGesture.None;
                    }
                    break;
            }
        }
    }
}


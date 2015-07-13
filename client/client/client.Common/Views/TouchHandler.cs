using System;
using CocosSharp;
using System.Collections.Generic;
using System.Collections;
using System.Diagnostics;
using System.Threading.Tasks;
using client.Common.Helper;
using Core.Models.Definitions;

namespace client.Common.Views
{
    public class TouchHandler
    {

        public enum TouchGesture
        {
            None,
            Start,
            Menu,
            MenuDrawn,
            Move,
            MoveUnit,
            Zoom
        }

        TouchGesture m_touchGesture;
        Stopwatch m_timer;
        WorldLayer m_worldLayer;

        float m_newScale = ClientConstants.TILEMAP_NORM_SCALE;
        float m_scale = ClientConstants.TILEMAP_NORM_SCALE;

        CCPoint m_startLocation;
        MenuView m_menuView;


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

                CCPoint screenStart0 = touches[0].StartLocationOnScreen;
                CCPoint screenStart1 = touches[1].StartLocationOnScreen;

                //calculate Current Position
                CCPoint currentPoint0 = touches[0].LocationOnScreen;
                CCPoint currentPoint1 = touches[1].LocationOnScreen;

                var screen = new CCPoint(m_worldLayer.VisibleBoundsWorldspace.MaxX,
                     m_worldLayer.VisibleBoundsWorldspace.MaxY); 

                float StartDistance = screenStart0.DistanceSquared(ref screenStart1);
                float CurrentDistance = currentPoint0.DistanceSquared(ref currentPoint1);
                float ScreenDistance = screen.LengthSquared;

                float relation = (CurrentDistance - StartDistance) / ScreenDistance;

                m_newScale = m_scale + (relation * m_newScale);
                m_worldLayer.ScaleWorld(m_newScale);
            }
        }

        public void OnTouchesBegan(List<CCTouch> touches, CCEvent touchEvent)
        {
            var oldStart = m_startLocation;
            m_startLocation = m_worldLayer.LayerWorldToParentspace(touches[0].Location);
            var coord = m_worldLayer.ClosestTileCoordAtNodePosition(m_startLocation);
            var oldCoord = m_worldLayer.ClosestTileCoordAtNodePosition(oldStart);

            switch (m_touchGesture)
            {
                case TouchGesture.MoveUnit:
					
                    var dictParam = new System.Collections.Generic.Dictionary<string,object>();

                    var startMapCellPosition = new client.Common.Models.MapCellPosition(oldCoord);
                    var startPosition = m_worldLayer.RegionView.GetCurrentGamePosition(startMapCellPosition, m_worldLayer.CenterPosition.RegionPosition);
                    var startPositionI = new Core.Models.PositionI((int)startPosition.X, (int)startPosition.Y);
                    dictParam[Core.Controllers.Actions.MoveUnit.START_POSITION] = startPositionI;

                    var location = m_worldLayer.LayerWorldToParentspace(touches[0].Location);
                    var endCoord = m_worldLayer.ClosestTileCoordAtNodePosition(location);

                    var endMapCellPosition = new client.Common.Models.MapCellPosition(endCoord);
                    var endPosition = m_worldLayer.RegionView.GetCurrentGamePosition(endMapCellPosition, m_worldLayer.CenterPosition.RegionPosition);
                    var endPositionI = new Core.Models.PositionI((int)endPosition.X, (int)endPosition.Y);
                    dictParam[Core.Controllers.Actions.MoveUnit.END_POSITION] = endPositionI;

                    var action = new Core.Models.Action(GameAppDelegate.Account, Core.Models.Action.ActionType.MoveUnit, dictParam);
                    var actionC = (Core.Controllers.Actions.Action)action.Control;
                    var possible = actionC.Possible(Core.Controllers.Controller.Instance.RegionManagerController);
                    if (possible)
                    {
                        m_worldLayer.DoAction(action);
                    }
                    m_touchGesture = TouchGesture.None;
                    break;

                case (TouchGesture.Menu):


                    switch (m_worldLayer.MenuLayer.TileGIDAndFlags(coord).Gid)
                    {
                        case ClientConstants.CROSS_GID:
                            break;
                        case ClientConstants.MENUEEARTH_GID:
                        case ClientConstants.MENUEAIR_GID:
                        case ClientConstants.MENUEWATER_GID:
                        case ClientConstants.MENUEGOLD_GID:
                        case ClientConstants.MENUEMANA_GID:
                        case ClientConstants.MENUEFIRE_GID:    
							//set action to create headquater
                            m_worldLayer.CreateBuilding(oldCoord, 276);
                            break;
                        case ClientConstants.MENUEBOWMAN_GID:
							//set action to create unit legolas
                            m_worldLayer.CreateUnit(oldCoord, 78);
                            break;
                        case ClientConstants.MENUEWARRIOR_GID:
							//set action to create unit warrior
                            m_worldLayer.CreateUnit(oldCoord, 72);
                            break;
                        case ClientConstants.MENUEMAGE_GID:
							//set action to create unit mage
                            m_worldLayer.CreateUnit(oldCoord, 66);
                            break;
                        case ClientConstants.MENUESCOUT_GID:
							//set action to create unit scout (unknown1 at the moment)
                            m_worldLayer.CreateUnit(oldCoord, 84);
                            break;
                        case ClientConstants.MENUEHERO_GID:
                            m_worldLayer.CreateUnit(oldCoord, 60);
                            break;
                        case ClientConstants.MENUEUNKNOWN_GID:
                            m_worldLayer.CreateUnit(oldCoord, 90);
                            break;
                        case 0:
                            break;
                    }
                    m_touchGesture = TouchGesture.None;


                    m_menuView.CloseMenu();
                    m_menuView = null;
                    //m_worldLayer.ShowMenu(oldCoord, 0);
                    return;

                case (TouchGesture.None):
                    m_touchGesture = TouchGesture.Start;
                    break;
            }

            m_timer.Reset();
            m_timer.Start();

            m_scale = m_worldLayer.GetScale();
        }


        public void OnTouchesEnded(List<CCTouch> touches, CCEvent touchEvent)
        {
            m_timer.Stop();
            var coord = m_worldLayer.ClosestTileCoordAtNodePosition(m_startLocation);

            switch (m_touchGesture)
            {

                case(TouchGesture.Zoom):
                    //Set Current Scale
                    m_worldLayer.ScaleWorld(m_newScale);
                    m_touchGesture = TouchGesture.None;
                    break;

                case(TouchGesture.Move):
                    m_worldLayer.CheckCenterRegion();
                    m_touchGesture = TouchGesture.None;
                    break;

                case(TouchGesture.Start):
                    if (m_worldLayer.UnitLayer.TileGIDAndFlags(coord).Gid != 0)
                    {
                        m_touchGesture = TouchGesture.MoveUnit;
                    }
                    else if (m_worldLayer.BuildingLayer.TileGIDAndFlags(coord).Gid != 0)
                    {
                        var types = new Core.Models.Definitions.Definition[6];
                        var defM = Core.Models.World.Instance.DefinitionManager;

                        types[0] = defM.GetDefinition(EntityType.Archer);
                        types[1] = defM.GetDefinition(EntityType.Hero);
                        types[2] = defM.GetDefinition(EntityType.Warrior);
                        types[3] = defM.GetDefinition(EntityType.Mage);
                        types[4] = defM.GetDefinition(EntityType.Scout);
                        types[5] = defM.GetDefinition(EntityType.Unknown3);

                        m_menuView = new MenuView(m_worldLayer.MenuLayer, coord, types);
                        m_menuView.DrawMenu();
                        m_touchGesture = TouchGesture.Menu;
                    }
                    else
                    {
                        var types = new Core.Models.Definitions.Definition[6];
                        var defM = Core.Models.World.Instance.DefinitionManager;

                        types[0] = defM.GetDefinition(EntityType.Headquarter);
                        types[1] = defM.GetDefinition(EntityType.Headquarter);
                        types[2] = defM.GetDefinition(EntityType.Headquarter);
                        types[3] = defM.GetDefinition(EntityType.Headquarter);
                        types[4] = defM.GetDefinition(EntityType.Headquarter);
                        types[5] = defM.GetDefinition(EntityType.Headquarter);

                        m_menuView = new MenuView(m_worldLayer.MenuLayer, coord, types);
                        m_menuView.DrawMenu();
                        m_worldLayer.UglyDraw();

                        m_touchGesture = TouchGesture.Menu;
                    }

                    break;
            }

        }
    }
}


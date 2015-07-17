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
            m_startLocation = new CCPoint(1, 1);
        }


        public void OnTouchesMoved(List<CCTouch> touches, CCEvent touchEvent)
        {
            if (touches.Count == 1 && m_touchGesture == TouchGesture.Zoom)
            {
                m_touchGesture = TouchGesture.Move;
            }
            else if (touches.Count == 2 && m_touchGesture == TouchGesture.Move)
            {
                m_touchGesture = TouchGesture.Zoom;
            }
                    

            if (touches.Count == 1 &&
                (m_touchGesture == TouchGesture.Start ||
                m_touchGesture == TouchGesture.Move))
            {
                m_touchGesture = TouchGesture.Move;
                var touch = touches[0];

                CCPoint diff = touch.Delta;
                m_worldLayer.MoveWorld(diff);
            }
            else if (touches.Count >= 2 &&
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
            var oldCoord = m_worldLayer.ClosestTileCoordAtNodePosition(oldStart);

            var oldMapCell = new Models.MapCellPosition(oldCoord);
            var oldPosition = m_worldLayer.RegionView.GetCurrentGamePosition(oldMapCell, m_worldLayer.CenterPosition.RegionPosition);


            m_startLocation = m_worldLayer.LayerWorldToParentspace(touches[0].Location);
            var coord = m_worldLayer.ClosestTileCoordAtNodePosition(m_startLocation);


            switch (m_touchGesture)
            {
                case TouchGesture.MoveUnit:
					
                    var startMapCellPosition = new client.Common.Models.MapCellPosition(oldCoord);
                    var startPosition = m_worldLayer.RegionView.GetCurrentGamePosition(startMapCellPosition, m_worldLayer.CenterPosition.RegionPosition);
                    var startPositionI = new Core.Models.PositionI((int)startPosition.X, (int)startPosition.Y);

                    var location = m_worldLayer.LayerWorldToParentspace(touches[0].Location);
                    var endCoord = m_worldLayer.ClosestTileCoordAtNodePosition(location);

                    var endMapCellPosition = new client.Common.Models.MapCellPosition(endCoord);
                    var endPosition = m_worldLayer.RegionView.GetCurrentGamePosition(endMapCellPosition, m_worldLayer.CenterPosition.RegionPosition);
                    var endPositionI = new Core.Models.PositionI((int)endPosition.X, (int)endPosition.Y);

                    var oldPositionI = new Core.Models.PositionI((int)oldPosition.X, (int)oldPosition.Y);
                    var action = ActionHelper.MoveUnit(oldPositionI, endPositionI);

                    var actionC = (Core.Controllers.Actions.Action)action.Control;
                    var possible = actionC.Possible();
                    if (possible)
                    {
                        m_worldLayer.DoAction(action);
                    }
                    m_touchGesture = TouchGesture.None;
                    break;

                case (TouchGesture.Menu):
                    var def = m_menuView.GetSelectedDefinition(coord);
                    if (def != null)
                    {
                        var oldPositionI2 = new Core.Models.PositionI((int)oldPosition.X, (int)oldPosition.Y);
                        var action2 = ActionHelper.CreateEntity(oldPositionI2, def);
                        var actionC2 = (Core.Controllers.Actions.Action) action2.Control;
                        if (actionC2.Possible())
                        {
                            m_worldLayer.DoAction(action2);
                        }
                    }
                    m_menuView.CloseMenu();
                    m_menuView = null;
                    m_touchGesture = TouchGesture.None;

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
                    //m_worldLayer.CheckCenterRegion();
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
                        types[4] = defM.GetDefinition(EntityType.Archer);
                        types[5] = defM.GetDefinition(EntityType.Archer);

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
                        m_touchGesture = TouchGesture.Menu;
                    }

                    break;
            }

            m_worldLayer.UglyDraw();
        }
    }
}


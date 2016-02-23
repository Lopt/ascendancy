namespace Client.Common.Views
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Threading.Tasks;
    using Client.Common.Helper;
    using Client.Common.Views.Effects;
    using CocosSharp;
    using Core.Models;
    using Core.Models.Definitions;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    /// <summary>
    /// Touch handler.
    /// </summary>
    public class TileTouchHandler
    {
        /// <summary>
        /// Touch gesture states.
        /// </summary>
        public enum TouchGesture
        {
            None,
            Start,
            Menu,
            MenuDrawn,
            Move,
            MoveUnit,
            Zoom,
            Area
        }

        /// <summary>
        /// The ownership area.
        /// </summary>
        public enum Area
        {
            Movement,
            OwnTerritory,
            EnemyTerritory,
            AllyTerritory
        }

        /// <summary>
        /// The m_touch gesture.
        /// </summary>
        public TouchGesture Gesture;

        /// <summary>
        /// The m_timer.
        /// </summary>
        private Stopwatch m_timer;

        /// <summary>
        /// The m_world layer.
        /// </summary>
        private WorldLayerHex m_worldLayer;

        /// <summary>
        /// The m_new scale.
        /// </summary>
        private float m_newZoom = Common.Constants.ClientConstants.TILEMAP_NORM_ZOOM;

        /// <summary>
        /// The m_scale.
        /// </summary>
        private float m_zoom = Common.Constants.ClientConstants.TILEMAP_NORM_ZOOM;

        /// <summary>
        /// The m_start location.
        /// </summary>
        private CCPoint m_startLocation;

        /// <summary>
        /// The m_menu view.
        /// </summary>
        private MenuView m_menuView;

        /// <summary>
        /// The indicator view.
        /// </summary>
        private IndicatorView m_indicator;

        /// <summary>
        /// Initializes a new instance of the <see cref="Client.Common.Views.TileTouchHandler"/> class.
        /// </summary>
        /// <param name="worldLayer">World layer.</param>
        public TileTouchHandler(WorldLayerHex worldLayer)
        {
            m_timer = new Stopwatch();
            Gesture = TouchGesture.None;
            m_worldLayer = worldLayer;
            m_startLocation = new CCPoint(1, 1);

            TouchHandler.Instance.ListenBegan(m_worldLayer, OnTouchesBegan);
            TouchHandler.Instance.ListenEnded(m_worldLayer, OnTouchesEnded);
            TouchHandler.Instance.ListenMoved(m_worldLayer, OnTouchesMoved);
        }

        /// <summary>
        /// On the touches moved event. Set the gesture to move or scale and scale or move the map.
        /// </summary>
        /// <param name="touches">Touches, where the user touched.</param>
        /// <param name="touchEvent">Touch event.</param>
        /// <returns>Returns a true after processed the event. </returns>
        public bool OnTouchesMoved(List<CCTouch> touches, CCEvent touchEvent)
        {
            /*
            if (touches.Count == 1 && m_touchGesture == TouchGesture.Zoom)
            {
                m_touchGesture = TouchGesture.Move;
            }
            */
            if (touches.Count >= 2 && Gesture == TouchGesture.Move)
            {
                Gesture = TouchGesture.Zoom;
            }

            if (touches.Count == 1 &&
                (Gesture == TouchGesture.Start ||
                Gesture == TouchGesture.Move))
            {
                Gesture = TouchGesture.Move;

                m_worldLayer.ViewMode = WorldLayerHex.ViewModes.CameraPosition;
               
                // if there is more than one click (as example at zooming) then take the average for moving
                CCPoint realLocationOnScreen = CCPoint.Zero;
                CCPoint realStartLocationOnScreen = CCPoint.Zero;
                foreach (var touch in touches)
                {
                    realLocationOnScreen += touch.LocationOnScreen;
                    realStartLocationOnScreen += touch.StartLocationOnScreen;
                }
                realLocationOnScreen /= touches.Count;
                realStartLocationOnScreen /= touches.Count;

                var diff = realLocationOnScreen - realStartLocationOnScreen;
                
                var move = new CCPoint(-diff.X, diff.Y) * m_worldLayer.GetZoom();
                var cameraDiff = realLocationOnScreen - m_worldLayer.Scene.VisibleBoundsScreenspace.Center;
                var cameraMove = new CCPoint(-cameraDiff.X, cameraDiff.Y) * m_worldLayer.GetZoom();
                m_worldLayer.SetWorldPosition(m_startLocation + cameraMove + move);
            }
            else if (touches.Count >= 2 &&
                     (Gesture == TouchGesture.Start ||
                     Gesture == TouchGesture.Zoom))
            {
                Gesture = TouchGesture.Zoom;

                CCPoint screenStart0 = touches[0].StartLocationOnScreen;
                CCPoint screenStart1 = touches[1].StartLocationOnScreen;

                // calculate Current Position
                CCPoint currentPoint0 = touches[0].LocationOnScreen;
                CCPoint currentPoint1 = touches[1].LocationOnScreen;

                var screen = new CCPoint(
                                 m_worldLayer.Scene.VisibleBoundsScreenspace.MaxX,
                                 m_worldLayer.Scene.VisibleBoundsScreenspace.MaxY); 

                float startDistance = screenStart0.DistanceSquared(ref screenStart1);
                float currentDistance = currentPoint0.DistanceSquared(ref currentPoint1);
                float screenDistance = screen.LengthSquared;

                float relation = (startDistance - currentDistance) / screenDistance;

                m_newZoom = m_zoom + (relation * m_newZoom);
                m_worldLayer.ZoomWorld(m_newZoom);
            }
            return true;
        }

        /// <summary>
        /// On the touches began event.
        /// </summary>
        /// <param name="touches">Touches, where the user touched.</param>
        /// <param name="touchEvent">Touch event.</param>
        /// <returns> Returns true after processing the touches.</returns>
        public bool OnTouchesBegan(List<CCTouch> touches, CCEvent touchEvent)
        {
            var oldWorldPosition = m_startLocation;
            var oldGamePositionI = Helper.PositionHelper.WorldspaceToPositionI(oldWorldPosition, m_worldLayer);

            m_startLocation = m_worldLayer.ConvertToWorldspace(touches[0].Location);
            var gamePositionI = Helper.PositionHelper.WorldspaceToPositionI(m_startLocation, m_worldLayer);

            switch (Gesture)
            {
                case TouchGesture.Area:
                    m_indicator.RemoveIndicator();
                    Gesture = TouchGesture.None;
                    break;
                case TouchGesture.MoveUnit:
                    var action = ActionHelper.MoveUnit(oldGamePositionI, gamePositionI);

                    var actionC = (Core.Controllers.Actions.Action)action.Control;
                    var possible = actionC.Possible();
                    if (possible)
                    {
                        m_worldLayer.DoAction(action);
                    }
                    m_indicator.RemoveIndicator();
                    Gesture = TouchGesture.None;
                    break;

                case TouchGesture.Menu:
                    var def = m_menuView.GetSelectedDefinition(gamePositionI);
                    if (def != null)
                    {
                        var action2 = ActionHelper.CreateEntity(m_menuView.GetCenterPosition(), def, GameAppDelegate.Account);
                        var actionC2 = (Core.Controllers.Actions.Action)action2.Control;
                        if (actionC2.Possible())
                        {
                            m_worldLayer.DoAction(action2);
                        }
                    }

                    if (m_menuView.IsExtended())
                    {
                        var regionView = m_worldLayer.GetRegionViewHex(gamePositionI.RegionPosition);
                        var gid = m_menuView.GetSelectedKategory(gamePositionI);
                        switch (gid.Gid)
                        {
                            case Client.Common.Constants.BuildingMenuGid.MILITARY:
                            case Client.Common.Constants.BuildingMenuGid.CIVIL:
                            case Client.Common.Constants.BuildingMenuGid.UPGRADE:
                            case Client.Common.Constants.BuildingMenuGid.RESOURCES:
                            case Client.Common.Constants.BuildingMenuGid.STORAGE:
                                m_menuView.ExtendMenu((short)gid.Gid, gamePositionI);
                                break;
                            default:
                                m_menuView.CloseMenu();
                                m_menuView = null;
                                Gesture = TouchGesture.None;
                                break;
                        }
                    }
                    else
                    {
                        m_menuView.CloseMenu(); 
                        Gesture = TouchGesture.None;
                    }

                    m_worldLayer.UglyDraw();
                    return true;

                case TouchGesture.None:
                    Gesture = TouchGesture.Start;
                    break;
            }

            m_timer.Reset();
            m_timer.Start();

            m_zoom = m_worldLayer.GetZoom();
            m_worldLayer.UglyDraw();
            return true;
        }

        /// <summary>
        /// On the touches ended event.
        /// </summary>
        /// <param name="touches">Touches, where the user touched.</param>
        /// <param name="touchEvent">Touch event.</param>
        /// <returns>Return true after processing the touch events and drawing.</returns>
        public bool OnTouchesEnded(List<CCTouch> touches, CCEvent touchEvent)
        {
            m_timer.Stop();
            var startPosI = Helper.PositionHelper.WorldspaceToPositionI(m_startLocation, m_worldLayer);
            switch (Gesture)
            {
                case TouchGesture.Zoom:
                    // Set Current Scale
                    m_worldLayer.ZoomWorld(m_newZoom);
                    Gesture = TouchGesture.None;
                    break;

                case TouchGesture.Move:
                    Gesture = TouchGesture.None;
                    break;

                case TouchGesture.Start:
                    var region = Core.Controllers.Controller.Instance.RegionManagerController.GetRegion(startPosI.RegionPosition);
                    var entity = region.GetEntity(startPosI.CellPosition);
                    int range = 0;
                    Area area = Area.Movement;
                    m_indicator = new IndicatorView(m_worldLayer);

                    if (entity != null && entity.Definition.Category == Category.Unit)
                    {
                        Gesture = TouchGesture.MoveUnit;
                        range = Core.Controllers.Controller.Instance.RegionManagerController.GetRegion(startPosI.RegionPosition).GetEntity(startPosI.CellPosition).Move;
                        m_indicator.ShowUnitIndicator(startPosI, range, area);
                    }
                    else if (entity != null && (entity.Definition.SubType == EntityType.Headquarter || entity.Definition.SubType == EntityType.GuardTower))
                    {
                        Gesture = TouchGesture.Area;
                        var owner = Core.Controllers.Controller.Instance.RegionManagerController.GetRegion(startPosI.RegionPosition).GetEntity(startPosI.CellPosition).Owner;
                        area = Area.OwnTerritory;
                        if (owner != GameAppDelegate.Account)
                        {
                            area = Area.EnemyTerritory;
                        }
                        m_indicator.ShowBuildingIndicator(startPosI, area);
                    }
                    else if (entity != null && entity.DefinitionID == (long)EntityType.Barracks && entity.Owner == GameAppDelegate.Account)
                    {
                        m_menuView = new MenuView(m_worldLayer, startPosI, MenuView.MenuType.Unity);
                        m_menuView.DrawMenu(MenuView.MenuType.Unity);
                        Gesture = TouchGesture.Menu;
                    }
                    else if (entity == null)
                    {
                        var defM = Core.Models.World.Instance.DefinitionManager;
                        var action = ActionHelper.CreateEntity(startPosI, defM.GetDefinition(EntityType.Headquarter), GameAppDelegate.Account);
                        var actionC = (Core.Controllers.Actions.CreateTerritoryBuilding)action.Control;
                        if (actionC.Possible())
                        {
                            m_menuView = new MenuView(m_worldLayer, startPosI, MenuView.MenuType.Headquarter);
                            m_menuView.DrawMenu(MenuView.MenuType.Headquarter);
                        }
                        else if (region.GetClaimedTerritory(startPosI) == action.Account)
                        {                             
                            m_menuView = new MenuView(m_worldLayer, startPosI, MenuView.MenuType.Major);
                            m_menuView.DrawMenu(MenuView.MenuType.Major);
                        }
                        else
                        {
                            // create an empty menu but didnt show it and set the m_menuView for the next touche
                            m_menuView = new MenuView(m_worldLayer, startPosI, MenuView.MenuType.Empty);
                            Gesture = TouchGesture.None;
                            return true;
                        }
                        Gesture = TouchGesture.Menu;
                    }
                    break;
            }
            m_worldLayer.UglyDraw();
            return true;
        }
    }
}
//namespace Client.Common.Views
//{
//    using System;
//    using System.Collections;
//    using System.Collections.Generic;
//    using System.Diagnostics;
//    using System.Threading.Tasks;
//    using Client.Common.Helper;
//    using CocosSharp;
//    using Core.Models.Definitions;
//
//    /// <summary>
//    /// Touch handler.
//    /// </summary>
//    public class TileTouchHandler
//    {
//        /// <summary>
//        /// Touch gesture states.
//        /// </summary>
//        public enum TouchGesture
//        {
//            None,
//            Start,
//            Menu,
//            MenuDrawn,
//            Move,
//            MoveUnit,
//            Zoom
//        }
//
//        /// <summary>
//        /// The m_touch gesture.
//        /// </summary>
//        private TouchGesture m_touchGesture;
//
//        /// <summary>
//        /// The m_timer.
//        /// </summary>
//        private Stopwatch m_timer;
//
//        /// <summary>
//        /// The m_world layer.
//        /// </summary>
//        private WorldLayerHex m_worldLayerHex;
//
//        /// <summary>
//        /// The m_new scale.
//        /// </summary>
//        private float m_newScale = Common.Constants.ClientConstants.TILEMAP_NORM_SCALE;
//
//        /// <summary>
//        /// The m_scale.
//        /// </summary>
//        private float m_scale = Common.Constants.ClientConstants.TILEMAP_NORM_SCALE;
//
//        /// <summary>
//        /// The m_start location.
//        /// </summary>
//        private CCPoint m_startLocation;
//
//        /// <summary>
//        /// The m_menu view.
//        /// </summary>
//        private MenuView m_menuView;
//
//        /// <summary>
//        /// Initializes a new instance of the <see cref="Client.Common.Views.TileTouchHandler"/> class.
//        /// </summary>
//        /// <param name="scene">The entire scene.</param>
//        public TileTouchHandler(GameScene scene)
//        {
//            m_timer = new Stopwatch();
//            m_touchGesture = TouchGesture.None;
//            m_worldLayerHex = scene.WorldLayerHex;
//            m_startLocation = new CCPoint(1, 1);
//
//            TouchHandler.Instance.ListenBegan(m_worldLayerHex, OnTouchesBegan);
//            TouchHandler.Instance.ListenEnded(m_worldLayerHex, OnTouchesEnded);
//            TouchHandler.Instance.ListenMoved(m_worldLayerHex, OnTouchesMoved);
//        }
//
//        /// <summary>
//        /// On the touches moved event. Set the gesture to move or scale and scale or move the map.
//        /// </summary>
//        /// <param name="touches">Touches, where the user touched.</param>
//        /// <param name="touchEvent">Touch event.</param>
//        /// <returns>Returns a true after processed the event. </returns>
//        public bool OnTouchesMoved(List<CCTouch> touches, CCEvent touchEvent)
//        {
//            if (touches.Count == 1 && m_touchGesture == TouchGesture.Zoom)
//            {
//                m_touchGesture = TouchGesture.Move;
//            }
//            else if (touches.Count == 2 && m_touchGesture == TouchGesture.Move)
//            {
//                m_touchGesture = TouchGesture.Zoom;
//            }
//
//            if (touches.Count == 1 &&
//                (m_touchGesture == TouchGesture.Start ||
//                m_touchGesture == TouchGesture.Move))
//            {
//                m_touchGesture = TouchGesture.Move;
//                var touch = touches[0];
//
//                CCPoint diff = touch.Delta;
//                m_worldLayerHex.MoveWorld(diff);
//            }
//            else if (touches.Count >= 2 &&
//                     (m_touchGesture == TouchGesture.Start ||
//                     m_touchGesture == TouchGesture.Zoom))
//            {
//                m_touchGesture = TouchGesture.Zoom;
//
//                CCPoint screenStart0 = touches[0].StartLocationOnScreen;
//                CCPoint screenStart1 = touches[1].StartLocationOnScreen;
//
//                // calculate Current Position
//                CCPoint currentPoint0 = touches[0].LocationOnScreen;
//                CCPoint currentPoint1 = touches[1].LocationOnScreen;
//
//                var screen = new CCPoint(
//                                 m_worldLayerHex.VisibleBoundsWorldspace.MaxX,
//                                 m_worldLayerHex.VisibleBoundsWorldspace.MaxY); 
//
//                float startDistance = screenStart0.DistanceSquared(ref screenStart1);
//                float currentDistance = currentPoint0.DistanceSquared(ref currentPoint1);
//                float screenDistance = screen.LengthSquared;
//
//                float relation = (currentDistance - startDistance) / screenDistance;
//
//                m_newScale = m_scale + (relation * m_newScale);
//                m_worldLayerHex.ScaleWorld(m_newScale);
//            }
//            return true;
//        }
//
//        /// <summary>
//        /// On the touches began event.
//        /// </summary>
//        /// <param name="touches">Touches, where the user touched.</param>
//        /// <param name="touchEvent">Touch event.</param>
//        /// <returns> Returns true after processing the touches.</returns>
//        public bool OnTouchesBegan(List<CCTouch> touches, CCEvent touchEvent)
//        {
//            var oldStart = m_startLocation;
//            var oldCoord = m_worldLayerHex.ConvertToWorldspace(oldStart);
//
//            m_startLocation = m_worldLayerHex.ConvertToWorldspace(touches[0].Location);
//            var coord = m_worldLayerHex.ClosestTileCoordAtNodePosition(m_startLocation);
//
//            switch (m_touchGesture)
//            {
//                case TouchGesture.MoveUnit:
//                    var startMapCellPosition = new Client.Common.Models.MapCellPosition(oldCoord);
//                    var startPosition = m_worldLayerHex.RegionViewHex.GetCurrentGamePosition(startMapCellPosition, m_worldLayerHex.CurrentPosition.RegionPosition);
//                    var startPositionI = new Core.Models.PositionI((int)startPosition.X, (int)startPosition.Y);
//
//                    var location = m_worldLayerHex.LayerWorldToParentspace(touches[0].Location);
//                    var endCoord = m_worldLayerHex.ClosestTileCoordAtNodePosition(location);
//
//                    var endMapCellPosition = new Client.Common.Models.MapCellPosition(endCoord);
//                    var endPosition = m_worldLayerHex.RegionViewHex.GetCurrentGamePosition(endMapCellPosition, m_worldLayerHex.CurrentPosition.RegionPosition);
//                    var endPositionI = new Core.Models.PositionI((int)endPosition.X, (int)endPosition.Y);
//
//                    var oldPositionI = new Core.Models.PositionI((int)oldPosition.X, (int)oldPosition.Y);
//                    var action = ActionHelper.MoveUnit(oldPositionI, endPositionI);
//
//                    var actionC = (Core.Controllers.Actions.Action)action.Control;
//                    var possible = actionC.Possible();
//                    if (possible)
//                    {
//                        m_worldLayerHex.DoAction(action);
//                    }
//                    m_touchGesture = TouchGesture.None;
//                    break;
//
//                case TouchGesture.Menu:
//                    var def = m_menuView.GetSelectedDefinition(coord);
//                    if (def != null)
//                    {
//                        var oldPositionI2 = new Core.Models.PositionI((int)oldPosition.X, (int)oldPosition.Y);
//                        var action2 = ActionHelper.CreateEntity(oldPositionI2, def);
//                        var actionC2 = (Core.Controllers.Actions.Action)action2.Control;
//                        if (actionC2.Possible())
//                        {
//                            m_worldLayerHex.DoAction(action2);
//                        }
//                    }
//                    m_menuView.CloseMenu();
//                    m_menuView = null;
//                    m_touchGesture = TouchGesture.None;
//
//                    return true;
//
//                case TouchGesture.None:
//                    m_touchGesture = TouchGesture.Start;
//                    break;
//            }
//
//            m_timer.Reset();
//            m_timer.Start();
//
//            m_scale = m_worldLayerHex.GetScale();
//
//            return true;
//        }
//
//        /// <summary>
//        /// On the touches ended event.
//        /// </summary>
//        /// <param name="touches">Touches, where the user touched.</param>
//        /// <param name="touchEvent">Touch event.</param>
//        /// <returns>Return true after processing the touch events and drawing.</returns>
//        public bool OnTouchesEnded(List<CCTouch> touches, CCEvent touchEvent)
//        {
//            m_timer.Stop();
//            var coord = m_worldLayerHex.ClosestTileCoordAtNodePosition(m_startLocation);
//
//            switch (m_touchGesture)
//            {
//                case TouchGesture.Zoom:
//                    // Set Current Scale
//                    m_worldLayerHex.ScaleWorld(m_newScale);
//                    m_touchGesture = TouchGesture.None;
//                    break;
//
//                case TouchGesture.Move:
//                    m_worldLayerHex.CheckCenterRegion();
//                    m_touchGesture = TouchGesture.None;
//                    break;
//
//                case TouchGesture.Start:
//                    if (m_worldLayerHex.UnitLayer.TileGIDAndFlags(coord).Gid != 0)
//                    {
//                        m_touchGesture = TouchGesture.MoveUnit;
//                    }
//                    else if (m_worldLayerHex.BuildingLayer.TileGIDAndFlags(coord).Gid != 0 && m_worldLayerHex.BuildingLayer.TileGIDAndFlags(coord).Gid <= 67)
//                    {
//                        var types = new Core.Models.Definitions.Definition[6];
//                        var defM = Core.Models.World.Instance.DefinitionManager;
//
//                        types[0] = defM.GetDefinition(EntityType.Archer);
//                        types[1] = defM.GetDefinition(EntityType.Hero);
//                        types[2] = defM.GetDefinition(EntityType.Warrior);
//                        types[3] = defM.GetDefinition(EntityType.Mage);
//                        types[4] = defM.GetDefinition(EntityType.Archer);
//                        types[5] = defM.GetDefinition(EntityType.Archer);
//
//                        m_menuView = new MenuView(m_worldLayerHex.MenuLayer, coord, types);
//                        m_menuView.DrawMenu();
//                        m_touchGesture = TouchGesture.Menu;
//                    }
//                    else if (m_worldLayerHex.BuildingLayer.TileGIDAndFlags(coord).Gid == 0)
//                    {
//                        var types = new Core.Models.Definitions.Definition[6];
//                        var defM = Core.Models.World.Instance.DefinitionManager;
//
//                        types[0] = defM.GetDefinition(EntityType.Headquarter);
//                        types[1] = defM.GetDefinition(EntityType.Headquarter);
//                        types[2] = defM.GetDefinition(EntityType.Headquarter);
//                        types[3] = defM.GetDefinition(EntityType.Headquarter);
//                        types[4] = defM.GetDefinition(EntityType.Headquarter);
//                        types[5] = defM.GetDefinition(EntityType.Headquarter);
//
//                        m_menuView = new MenuView(m_worldLayerHex.MenuLayer, coord, types);
//                        m_menuView.DrawMenu();
//                        m_touchGesture = TouchGesture.Menu;
//                    }
//
//                    break;
//            }
//                    
//            m_worldLayerHex.UglyDraw();
//            return true;
//        }
//    }
//}
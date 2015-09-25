namespace Client.Common.Views
{
    using System;
    using System.Collections.Generic;
    using Client.Common.Models;
    using CocosSharp;

    /// <summary>
    /// The Game scene.
    /// </summary>
    public class GameScene : CCScene
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Client.Common.Views.GameScene"/> class.
        /// </summary>
        /// <param name="mainWindow">Main window.</param>
        public GameScene(CCWindow mainWindow)
            : base(mainWindow)
        {
            
            WorldLayer = new WorldLayer(this);
            AddChild(WorldLayer);

            HUD = new Client.Common.Views.HUD.HUDLayer(this);
            AddChild(HUD);

            DebugLayer = new DebugLayer();
            AddChild(DebugLayer);

            m_touchHandler = new TouchHandler(this); 
            var touchListener = new CCEventListenerTouchAllAtOnce();
            touchListener.OnTouchesMoved = m_touchHandler.OnTouchesMoved;
            touchListener.OnTouchesBegan = m_touchHandler.OnTouchesBegan;
            touchListener.OnTouchesEnded = m_touchHandler.OnTouchesEnded;
            WorldLayer.AddEventListener(touchListener);
        }
            
        #region Properties

        /// <summary>
        /// The m_touch handler.
        /// </summary>
        private TouchHandler m_touchHandler;

        /// <summary>
        /// The world (whole gamefield).
        /// </summary>
        public WorldLayer WorldLayer;

        /// <summary>
        /// The debug layer (shows logging information).
        /// </summary>
        public DebugLayer DebugLayer;

        public HUD.HUDLayer HUD;

        #endregion
    }
}
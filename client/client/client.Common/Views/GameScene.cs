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
            m_world = new WorldLayer(this);
            this.AddChild(m_world);

            m_touchHandler = new TouchHandler(m_world); 
            var touchListener = new CCEventListenerTouchAllAtOnce();
            touchListener.OnTouchesMoved = m_touchHandler.OnTouchesMoved;
            touchListener.OnTouchesBegan = m_touchHandler.OnTouchesBegan;
            touchListener.OnTouchesEnded = m_touchHandler.OnTouchesEnded;

            m_world.AddEventListener(touchListener);
        }

        #region Properties

        /// <summary>
        /// The m_touch handler.
        /// </summary>
        private TouchHandler m_touchHandler;

        /// <summary>
        /// The m_world.
        /// </summary>
        private WorldLayer m_world;

        #endregion
    }
}
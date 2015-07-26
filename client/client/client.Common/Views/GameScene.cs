using System;
using CocosSharp;
using System.Collections.Generic;
using Client.Common.Models;

namespace Client.Common.Views
{
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
            var TouchListener = new CCEventListenerTouchAllAtOnce();
            TouchListener.OnTouchesMoved = m_touchHandler.OnTouchesMoved;
            TouchListener.OnTouchesBegan = m_touchHandler.OnTouchesBegan;
            TouchListener.OnTouchesEnded = m_touchHandler.OnTouchesEnded;

            m_world.AddEventListener(TouchListener);

        }


        #region Properties

        /// <summary>
        /// The m_touch handler.
        /// </summary>
        TouchHandler m_touchHandler;
        /// <summary>
        /// The m_world.
        /// </summary>
        WorldLayer m_world;

        #endregion
    }
}


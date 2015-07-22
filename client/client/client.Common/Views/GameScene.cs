﻿using System;
using CocosSharp;
using System.Collections.Generic;
using Client.Common.Models;

namespace Client.Common.Views
{
    public class GameScene : CCScene
    {


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

        TouchHandler m_touchHandler;
        WorldLayer m_world;

        #endregion
    }
}


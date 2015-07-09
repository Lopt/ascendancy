using System;
using CocosSharp;

namespace client.Common.Views
{
    public class StartScene : CCScene
    {
        public StartScene(CCWindow mainWindow)
            : base(mainWindow)
        {
            m_LogoLayer = new LogoLayer();
            this.AddChild(m_LogoLayer);
        }

        #region Properties

        LogoLayer m_LogoLayer;

        #endregion
    }
}

